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
    public class FormChoosePhase : Dialog
    {
        // ⌘ Auto

        [RqSc] ScMain   ScMain;

        // ⚡ Events

        public event Action<TickerPhaseDef>   OnPhaseButton;
        public event Action                   OnResetProgress;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _GroupPhases;
        [Sf, Construct] Button          _ButtonResetProgress;

        [HeaderDev]
        [Sf] TMP_Text  _TextBatteryLevel;
        
        // Ui

        private void RebuildPhases ()
        {
            _GroupPhases.DeleteChildrenImmediate();

            foreach (var def in App.Data.Settings.Phases)
            {
                var button = ButtonPhase.Create(def, _GroupPhases, name:def.Name);
                
                button.ClickEvent.AddListener(_ => OnPhaseButton?.Invoke(button.PhaseDef));
            }
        }

        // ↑ Dialog

        protected override void OnOpen ()
        {
            RebuildPhases();


            #if PROGRESSOR_DEVELOPMENT

            // Show battery delta for the phase

            var batterDelta = ScMain.BatteryPhaseStart - SystemInfo.batteryLevel;
            _TextBatteryLevel.text = $"{batterDelta:F4}";

            _TextBatteryLevel.transform.Enable();

            #endif
        }

        // ↑ Widget

        protected override void ConstructWidget ()
        {
            #if PROGRESSOR_DEVELOPMENT
            Debug.Assert(_TextBatteryLevel, "Field must be assigned");
            #endif

            // Ui initialization

            _GroupPhases.DeleteChildrenImmediate();

            // Events

            _ButtonResetProgress.ClickEvent.AddListener(_ => OnResetProgress?.Invoke());
        }
    }
}
