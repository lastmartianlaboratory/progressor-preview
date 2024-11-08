using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [CreateAssetMenu(fileName = "Ui", menuName = "Progressor/DataSets/Ui", order = 1)]
    public class DataSetUi : DataSet
    {
        // ▶ Prop

        public Ui.WidgetStep   StepPrefab           => _StepPrefab;
        public Ui.ButtonPhase  ButtonPhasePrefab    => _ButtonPhasePrefab;

        // ≡ Sf

        [Sf] Ui.WidgetStep      _StepPrefab;
        [Sf] Ui.ButtonPhase     _ButtonPhasePrefab;
        
        // ...

        // ↑ DataSet

        public override void Initialize ()
        {
            Debug.Assert(_StepPrefab);
        }
    }
}
