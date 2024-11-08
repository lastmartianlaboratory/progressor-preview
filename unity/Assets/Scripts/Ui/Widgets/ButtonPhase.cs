using System;
using UnityEngine;
using TMPro;
using UXT;
using UXT.Attr;
using UXT.UI;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class ButtonPhase : Button
    {
        // ▶ Prop

        public TickerPhaseDef   PhaseDef { get; private set; } = null;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] UnityEngine.UI.Image    _Background;
        [Sf, Construct] TMP_Text                _Caption;

        // Create

        public static ButtonPhase Create (TickerPhaseDef def, RectTransform parent, string name = "PhaseButton")
        {
            var button = Instantiate(App.Data.Ui.ButtonPhasePrefab, parent);

            button.name = name;
            button.Initialize();
            button.ApplyPhaseDefinition(def);

            return button;
        }

        private void ApplyPhaseDefinition (TickerPhaseDef def)
        {
            if (!def)
            {
                Log.Warn("Phase definition cannot be null", this);
                return;
            }

            PhaseDef = def;

            _Background.color = def.BaseColor;
            _Caption.text     = def.Name;
        }
    }
}
