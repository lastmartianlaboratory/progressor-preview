using UnityEngine;
using UXT;
using UXT.Attr;
using UXT.UI;
using Sf = UnityEngine.SerializeField;

namespace Progressor.Ui
{
    public class WidgetProgressBar : Button
    {
        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _Progress;

        // WidgetProgressBar

        public void SetProgress (float progress)
        {
            progress = Mathf.Clamp01(progress);

            _Progress.localScale = new (progress, _Progress.localScale.y, _Progress.localScale.z);
        }

        // ↑ Widget

        protected override void ConstructWidget ()
        {
            SetProgress(0);
        }
    }
}
