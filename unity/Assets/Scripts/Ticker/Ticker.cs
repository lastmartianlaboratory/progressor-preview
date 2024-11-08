using System;
using System.Collections;
using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    public class Ticker : Managed
    {
        // ▶ Prop

        public PhaseType    ActivePhaseType    => (ActivePhase != null) ? ActivePhase.Type : PhaseType.None; // Safer type access: is ative phase is null its type is `None`
        public TickerPhase  ActivePhase        { get; private set; } = null;
        public int          StepsPerSession    => _StepsPerSession;

        public bool         IsRunning          { get; private set; } = false;
        public int          StepsCompleted     { get; private set; } = 0; // 'Step' is a single completed Work Phase

        // ⚡ Events

        public event Action<Ticker>   OnRun;
        public event Action<Ticker>   OnPhaseChange;
        public event Action<Ticker>   OnWorkCompleted;
        public event Action<Ticker>   OnRestCompleted;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Range(1, 16)] int  _StepsPerSession;

        // ╍ Private

        private Coroutine   TickerLoop = null;

        // Ticker

        public void Run (bool doRun)
        {
            RunUneventful(doRun);

            OnRun?.Invoke(this);
        }

        private void RunUneventful (bool doRun)
        {
            // Run only if active state present.
            // Cannot run/pause if already running/paused

            if ((ActivePhase != null) && (doRun != IsRunning))
            {
                IsRunning = doRun;
            }
        }

        public bool IsPhaseIdSupported (string phaseId)
        {
            //~ don't like asking App for that. Should not go out ouside directly Ticker-related classes

            return (App.Data.Settings.GetPhaseById(phaseId) != null);
        }

        public void SwitchToPhase (TickerPhase phase)
        {
            /// Switch to phase and run it.
            /// `phase` can be null

            if (phase != null)
            {
                if (!IsPhaseIdSupported(phase.Id))
                {
                    Log.Warn($"Phase Id <b>{phase.Id}</b> is not supported");
                    phase = null;
                }
            }

            ActivePhase = phase;

            OnPhaseChange?.Invoke(this);

            RunUneventful(IsRunning);
        }

        public void SwitchToIdle ()
        {
            SwitchToPhase(null);
        }

        public void ResetProgress ()
        {
            //> are events properly fired? Should phase change?

            StepsCompleted = 0;
            
            Run(false);
        }

        // Ticker loop

        private void CreateUpdateLoop ()
        {
            if (TickerLoop != null)
                TerminateUpdateLoop();
            
            TickerLoop = StartCoroutine(UpdateLoop());
        }

        private void TerminateUpdateLoop ()
        {
            if (TickerLoop != null)
            {
                StopCoroutine(TickerLoop);
                TickerLoop = null;
            }
        }

        private IEnumerator UpdateLoop ()
        {
            // ...

            while (true)
            {
                if (IsRunning && (ActivePhase != null))
                {
                    if (ActivePhase.Advance(Time.unscaledDeltaTime))
                    {
                        // Phase completed

                        if (ActivePhase.Type == PhaseType.Work)
                        {
                            StepsCompleted ++;

                            OnWorkCompleted?.Invoke(this);
                        }
                        else if (ActivePhase.Type == PhaseType.Rest)
                        {
                            OnRestCompleted?.Invoke(this);
                        }
                        else
                        {
                            Log.Warn($"Unhandled phase type: { ActivePhase.Type }");
                        }

                        RunUneventful(false);
                    }

                    yield return null;
                }
                else
                {
                    // Doing nothing

                    yield return null;
                }
            }
        }

        // Serialization //~ use 'se/deserialize' names?

        public class StoredData
        {
            public int          StepsCompleted  = 0;
            public TickerPhase  ActivePhase     = null;
            public bool         IsRunning       = false;
        }

        public StoredData GetStoredData ()
        {
            var data = new StoredData {
                StepsCompleted = this.StepsCompleted,
                ActivePhase    = this.ActivePhase,
                IsRunning      = this.IsRunning,
            };

            return data;
        }

        public void ApplyStoredData (StoredData data)
        {
            //> stop all states, apply data, fire events

            if (data == null)
            {
                Log.Warn("Cannot apply null data");
                return;
            }

            StepsCompleted = data.StepsCompleted;
            ActivePhase    = data.ActivePhase;
            IsRunning      = data.IsRunning;

            SwitchToPhase(ActivePhase);
        }

        // ↑ Managed

        protected override void Construct ()
        {
            Debug.Assert(_StepsPerSession > 0);


            DontDestroyOnLoad(this); //~ replace with UXT.Ignition persistent object functionality

            CreateUpdateLoop();

            // ---------------------------------------
            // Ticker is now paused and in IDLE phase
            // ---------------------------------------
        }

        protected override void Destruct ()
        {
            TerminateUpdateLoop();
        }
    }
}
