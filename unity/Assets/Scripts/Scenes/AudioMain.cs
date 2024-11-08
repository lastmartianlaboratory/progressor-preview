using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    public class AudioMain : Managed
    {
        // ≡ Sf
        
        [ConstructHeader]
        [Sf, Construct] AudioSource _AudioSource;

        // Sounds

        public void PlayPhaseCompleted ()
        {
            _AudioSource.PlayOneShot(App.Data.Audio.PhaseCompleted);
        }
    }
}
