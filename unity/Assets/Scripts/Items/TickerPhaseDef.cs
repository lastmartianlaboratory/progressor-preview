using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [CreateAssetMenu(fileName = "TickerPhaseDef", menuName = "Progressor/TickerPhaseDef", order = 1)]
    public class TickerPhaseDef : ScriptableObject
    {
        // ▶ Prop

        public string     Id        => _Id;
        public string     Name      => _Name;
        public PhaseType  PhaseType => _PhaseType;
        public int        Duration  => _Duration;
        public Color      BaseColor => _BaseColor;

        // ≡ Sf

        [Sf] string       _Id        = "";
        [Sf] string       _Name      = "Phase";
        [Sf] PhaseType    _PhaseType = PhaseType.None;
        [Sf, Min(1)] int  _Duration  = 10;
        [Sf] Color        _BaseColor = Color.magenta;
    }
}
