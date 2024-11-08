using UnityEngine;
using UXT;
using UXT.Attr;
using UXT.UI;
using UXT.Extensions.Unity;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class FormSteps : Dialog
    {
        // ⌘ Auto

        [RqSc] Ticker   Ticker;

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _StepsGroup;

        // ╍ Private
        
        private WidgetStep[] Steps = new WidgetStep[0];

        // Ui

        private void DeleteSteps ()
        {
            _StepsGroup.DeleteChildrenImmediate();
            Steps = new WidgetStep[0];
        }

        private void RebuildSteps ()
        {
            DeleteSteps();

            Steps = new WidgetStep[Ticker.StepsPerSession];

            for (int n = 0; n < Ticker.StepsPerSession; n ++)
                Steps[n] = WidgetStep.Create(_StepsGroup);
        }

        // ↑ Widget

        public override void UpdateView ()
        {
            // Rebuild steps count

            if (Ticker.StepsPerSession != _StepsGroup.childCount)
                RebuildSteps();

            // Update states

            for (int n = 0; n < Steps.Length; n ++)
                Steps[n].MakeComplete((n + 1) <= Ticker.StepsCompleted);
        }

        protected override void ConstructWidget ()
        {
            // Clear any dev-only elements

            _StepsGroup.DeleteChildrenImmediate();
        }
    }
}
