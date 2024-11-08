using UnityEngine;
using UXT;
using UXT.Attr;
using UXT.Extensions.Unity;
using UXT.UI;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class WidgetStep : Widget
    {
        // ▶ Prop

        public bool     IsComplete { get; private set; } = false;
        
        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _ShapeComplete;
        [Sf, Construct] RectTransform   _ShapeIncomplete;

        // Create

        public static WidgetStep Create (RectTransform parent, string name = "Step")
        {
            var item = Instantiate<WidgetStep>(App.Data.Ui.StepPrefab, parent);

            item.name = name;

            item.Initialize();

            return item;
        }

        // Ui

        public void MakeComplete (bool complete = true)
        {
            IsComplete = complete;
            Refresh();
        }

        // ↑ Widget

        public override void UpdateView ()
        {
            if (IsComplete)
                _ShapeComplete.ActivateExclusively();
            else
                _ShapeIncomplete.ActivateExclusively();
        }
    }
}
