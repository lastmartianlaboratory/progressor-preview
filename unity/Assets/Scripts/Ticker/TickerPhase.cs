using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using UXT;

namespace Progressor
{
    public class TickerPhase
    {
        [JsonProperty]  public readonly string    Id = "";
        [JsonProperty]  public readonly PhaseType Type = PhaseType.None;
        [JsonProperty]  public readonly int       Duration = 0; // s
        [JsonProperty]  public float              Progress { get; private set; } = 0; // s
        [JsonIgnore]    public float              ProgressNormalized  => Mathf.Clamp01(Progress / Duration);

        // TickerPhase

        public TickerPhase ()
        {
            /// While not be used explicitely,
            /// it is implicitely used 
            /// deserialization among other things

            Id       = "";
            Type     = PhaseType.None;
            Duration = 0;
            Progress = 0;
        }

        public TickerPhase (TickerPhaseDef def)
        {
            Id       = def.Id;
            Type     = def.PhaseType;
            Duration = def.Duration;
            Progress = 0;
        }

        public bool Advance (float timeDelta)
        {
            Progress += timeDelta;

            if (Progress >= Duration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset ()
        {
            Progress = 0;
        }

        // Util

        public string ProgressAsTimeMS ()
        {
            // Return the progress in form of M.SS

            int minutes = (int)Progress / 60;
            int seconds = Mathf.FloorToInt(Progress) % 60;

            return $"{minutes:D2}.{seconds:D2}";
        }
    }
}
