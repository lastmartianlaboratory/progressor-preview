using System;
using UnityEngine;
using TMPro;
using UXT;
using UXT.Attr;
using UXT.UI;
using UXT.Extensions.Unity;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class FormStatus : Dialog
    {
        // ⌘ Auto

        [RqSc] Ticker   Ticker;

        // ⚡ Events

        public event Action     OnIdle;
        public event Action     OnFlipScreen;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _GroupProgress;
        [Sf, Construct] RectTransform   _PhaseWorking;
        [Sf, Construct] RectTransform   _PhaseResting;
        [Sf, Construct] RectTransform   _StatePaused; //? used as a Phase but not really a Phase
        [Sf, Construct] TMP_Text        _ProgressTime;
        [Sf, Construct] TMP_Text        _SystemDay;
        [Sf, Construct] TMP_Text        _SystemTime;
        [Sf, Construct] Button          _ButtonIdle;
        [Sf, Construct] Button          _ButtonFlipScreen;
        [Sf, Construct] RectTransform   _BuildTagDev;

        // ↑ Widget

        public override void UpdateView ()
        {
            /// Using a single `UpdateView()`
            /// as a more simple solution than
            /// having multiple Ticker events handlers

            if (Ticker.ActivePhase == null)
            {
                _GroupProgress.Disable();
            }
            else
            {
                _GroupProgress.Enable();

                if (!Ticker.IsRunning)
                {
                    _StatePaused.ActivateExclusively();
                }
                else
                {
                    if (Ticker.ActivePhase.Type == PhaseType.Work)
                        _PhaseWorking.ActivateExclusively();
                    else if (Ticker.ActivePhase.Type == PhaseType.Rest)
                        _PhaseResting.ActivateExclusively();
                }
            }
        }

        protected override void ConstructWidget ()
        {
            _ButtonIdle      .ClickEvent.AddListener(_ => OnIdle?.Invoke());
            _ButtonFlipScreen.ClickEvent.AddListener(_ => OnFlipScreen?.Invoke());

            #if PROGRESSOR_DEVELOPMENT
            _BuildTagDev.Enable();
            #else
            _BuildTagDev.Disable();
            #endif
        }

        // ↑ MonoBehaviour

        private void Update ()
        {
            _SystemDay.text  = $"{System.DateTime.Now.DayOfWeek}";
            _SystemTime.text = $"{System.DateTime.Now.ToString("HH:mm")}";

            if (Ticker.ActivePhase != null)
                _ProgressTime.text = Ticker.ActivePhase.ProgressAsTimeMS();
            else
                _ProgressTime.text = "---";
        }
    }
}
