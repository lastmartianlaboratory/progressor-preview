using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [CreateAssetMenu(fileName = "Audio", menuName = "Progressor/DataSets/Audio", order = 1)]
    public class DataSetAudio : DataSet
    {
        // ▶ Prop

        public AudioClip   PhaseCompleted => _PhaseCompleted;

        // ≡ Sf

        [Sf] AudioClip  _PhaseCompleted; //> check if Construct works. Assert if doesnt
        
        // ...

        // ↑ DataSet

        public override void Initialize ()
        {
            Debug.Assert(_PhaseCompleted);
        }
    }
}
