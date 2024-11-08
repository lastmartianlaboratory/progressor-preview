using UnityEngine;
using UXT;
using DG.Tweening;
using UXT.Attr;
using UXT.UI;
using UXT.UI.Canvas;
using Progressor.Ui;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    public class WmMain : WidgetManager
    {
        // ⌘ Auto

        [RqCc] public FormStatus            FormStatus          { get; private set; }
        [RqCc] public FormSteps             FormSteps           { get; private set; }
        [RqCc] public FormChoosePhase       FormChoosePhase     { get; private set; }
        [RqCc] public FormProgress          FormProgress        { get; private set; }

        // ≡ Sf

        [ConstructHeader]
        [Sf, Construct] RectTransform   _Root;

        // WmMain

        public void FlipUi ()
        {
            // Smooth flip

            _Root.DOKill();

            if (_Root.rotation.eulerAngles.z == 0)
                _Root.DOLocalRotate(new Vector3(0, 0, 180), 0.3f)
                    .SetEase(Ease.InOutCubic);
            else
                _Root.DOLocalRotate(new Vector3(0, 0, 0), 0.3f)
                    .SetEase(Ease.InOutCubic);

            // Immediate flip

            // if (_Root.rotation.eulerAngles.z == 0)
            //     _Root.rotation = Quaternion.Euler(0, 0, 180);
            // else
            //     _Root.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
