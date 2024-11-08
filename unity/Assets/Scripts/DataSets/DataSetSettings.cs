using System.Collections.Generic;
using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Progressor/DataSets/Settings", order = 1)]
    public class DataSetSettings : DataSet
    {
        // ▶ Prop
        
        public TickerPhaseDef[]    Phases => _Phases;

        // ≡ Sf

        [Sf, Construct] TickerPhaseDef[]    _Phases;

        // DataSetSettings

        public TickerPhaseDef GetPhaseById (string id)
        {
            foreach (var phase in Phases)
            {
                if (phase.Id == id)
                {
                    return phase;
                }
            }
            return null;
        }

        // ↑ DataSet

        public override void Initialize ()
        {
            var phaseIds = new HashSet<string>();

            foreach (var phase in Phases)
            {
                // PhaseType.None is not for use in definitions

                if (phase.PhaseType == PhaseType.None)
                    Debug.LogError("PhaseType.None is not allowed in definitions", phase);

                // Phase Ids must be unique

                if (!phaseIds.Add(phase.Id))
                    Debug.LogError($"Duplicate Phase Id found: `{phase.Id}`", phase);
            }
        }
    }
}
