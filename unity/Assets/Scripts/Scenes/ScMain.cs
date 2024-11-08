using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UXT;
using UXT.Attr;
using UXT.UI;
using UXT.Extensions.Unity;
using TMPro;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    public class ScMain : SceneManager
    {
        //~ some features might actually be better in Ticker

        // ▶ Prop

        public Ticker   Ticker => _Ticker;
        public float    BatteryPhaseStart { get; private set; } = 0f; // Battery level at the start/end of a phase

        // ≡ Sf

        [SettingsHeader]
        [Sf, Min(1)] float  _StateSnapshotInterval; // s, An interval between automatic Ticker state snapshots

        [ConstructHeader]
        [Sf, Construct] WmMain      _Wm;
        [Sf, Construct] AudioMain   _Audio;
        [Sf, Construct] Ticker      _Ticker;

        [HeaderDev]
        [Sf] bool   _TickerLogEnabled = false;
        [Sf] bool   _DisableAutoSave  = false;

        // ScMain

        private void ConstructUI ()
        {
            _Wm.FormChoosePhase.OnPhaseButton += (phaseDef) => {
                _Wm.FormChoosePhase.Close();
                Ticker.SwitchToPhase(new (phaseDef));
                _Wm.Refresh(); // Refresh works here and does not in Phase change handler, because inside it is still paused
            };

            _Wm.FormProgress.OnPauseToggle += () => {
                Ticker.Run(!Ticker.IsRunning);
            };

            _Wm.FormStatus.OnIdle += () => {
                Ticker.SwitchToIdle();
                _Wm.Refresh();
            };

            _Wm.FormStatus.OnFlipScreen += () => {
                _Wm.FlipUi();
            };

            _Wm.FormChoosePhase.OnResetProgress += () => {
                Ticker.ResetProgress();
                _Wm.Refresh();
                WriteTickerSnapshot(); //> appropriate? or use some event? Do not write data if progress reset decides to include state switching
            };
        }

        // Ticker events

        private void OnTickerRun ()
        {
            if (_TickerLogEnabled)
                Log.Msg($"--> OnTickerRun: {Ticker.IsRunning}");

            WriteTickerSnapshot();

            _Wm.FormStatus.Refresh();
        }

        private void OnTickerPhaseChange ()
        {
            if (_TickerLogEnabled)
                Log.Msg($"<b>OnTickerPhaseChange: { Ticker.ActivePhaseType }</b>");

            WriteTickerSnapshot();

            if (Ticker.ActivePhase != null)
            {
                BatteryPhaseStart = SystemInfo.batteryLevel;

                _Wm.FormProgress.Open();
            }
            else
            {
                _Wm.FormProgress.Close();
                _Wm.FormChoosePhase.Open();
            }
        }

        private void OnTickerWorkCompleted ()
        {
            if (_TickerLogEnabled)
                Log.Msg($"■ OnTickerWorkCompleted");

            _Audio.PlayPhaseCompleted();

            Ticker.SwitchToIdle();

            _Wm.FormProgress.Close();
            _Wm.FormChoosePhase.Open();

            _Wm.Refresh();
        }

        private void OnTickerRestCompleted ()
        {
            if (_TickerLogEnabled)
                Log.Msg($"■ OnTickerRestCompleted: { Ticker.ActivePhaseType }");

            _Audio.PlayPhaseCompleted();

            Ticker.SwitchToIdle();

            _Wm.FormProgress.Close();
            _Wm.FormChoosePhase.Open();

            _Wm.Refresh();
        }

        //> format

        // Serialization

        //~ move this functionality into App or a completely independent module?

        private const string    TickerDataFileName = "ticker.json";
        private string          TickerDataFileLocation => Application.persistentDataPath + "/" + TickerDataFileName;
        private bool            TickerWritingData = false; // `true` if Ticker data is currently being written
        private Coroutine       TickerSnapshotCoroutine = null;

        private void WriteTickerSnapshot (string logTag = "")
        {
            //~ refine safety

            Log.Msg($"↓ { (String.IsNullOrEmpty(logTag) ? "" : $"[{logTag}] ") }Writing data to <b>{TickerDataFileLocation}</b>");

            if (TickerWritingData) //~ is there some safer way to do it without any extra properties
            {
                Log.Warn("Writing in progress. Skipping write");
                return;
            }

            try
            {
                TickerWritingData = true;

                var data = Ticker.GetStoredData();
                var dataString = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(TickerDataFileLocation, dataString);
            }
            catch (Exception e)
            {
                Log.Warn($"Failed to write data ({e.Message})");
            }
            finally
            {
                TickerWritingData = false;
            }
        }

        private void ReadTickerSnapshot ()
        {
            //~ refine safety

            Log.Msg($"↑ Reading data from <b>{TickerDataFileLocation}</b>");

            try
            {
                var dataString = System.IO.File.ReadAllText(TickerDataFileLocation);
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Ticker.StoredData>(dataString);

                Ticker.ApplyStoredData(data);
            }
            catch (Exception e)
            {
                Log.Warn($"Failed to read data ({e.Message})");
            }
        }

        private void BeginTickerSnapshotLoop ()
        {
            EndTickerSnapshotLoop();
            TickerSnapshotCoroutine = StartCoroutine(TickerSnapshotLoop());
        }

        private void EndTickerSnapshotLoop ()
        {
            if (TickerSnapshotCoroutine != null)
            {
                StopCoroutine(TickerSnapshotCoroutine);
                TickerSnapshotCoroutine = null;
            }
        }

        private IEnumerator TickerSnapshotLoop ()
        {
            /// Do not run directly

            while (true)
            {
                yield return new WaitForSecondsRealtime(_StateSnapshotInterval);

                #if PROGRESSOR_DEVELOPMENT
                if (!_DisableAutoSave)
                    WriteTickerSnapshot("auto");
                #else
                    WriteTickerSnapshot("auto");
                #endif
            }
        }

        // ↑ SceneManager

        public override void FirstUpdate ()
        {
            // #if PROGRESSOR_DEVELOPMENT

            // if (Input.GetKeyDown(KeyCode.W))
            //     WriteTickerSnapshot();

            // if (Input.GetKeyDown(KeyCode.R))
            //     ReadTickerSnapshot();

            // #endif
        }

        public override void FirstStart ()
        {
            ReadTickerSnapshot();
            BeginTickerSnapshotLoop();

            _Wm.FormStatus.Open();
            _Wm.FormSteps.Open();

            _Wm.FormStatus.Refresh();
        }

        public override void FirstAwake ()
        {
            // Ticker

            Ticker.OnRun += (ticker => {
                OnTickerRun();
            });

            Ticker.OnPhaseChange += (ticker => {
                OnTickerPhaseChange();
            });

            Ticker.OnWorkCompleted += (ticker => {
                OnTickerWorkCompleted();
            });

            Ticker.OnRestCompleted += (ticker => {
                OnTickerRestCompleted();
            });

            ConstructUI();
        }
    }
}
