using System.Linq;
using System.Reflection;
using UnityEngine;
using UXT;
using UXT.Attr;
using Sf = UnityEngine.SerializeField;

namespace Progressor
{
    [CreateAssetMenu(fileName = "Data", menuName = "Progressor/DataSets/DataSets", order = 1)]
    public class DataSets : ScriptableObject //! UXT template
    {
        ///  Public fields of `DataSet` type will be initialized automatically

        //? Don't like the public/writeable on these
        //? Construct used unconventionally

        [Construct] public DataSetSettings   Settings   = null;
        [Construct] public DataSetUi         Ui         = null;
        [Construct] public DataSetAudio      Audio      = null;

        // DataSets

        public void Initialize () //~ should this be editor only? Dev only?
        {
            /// Run initialization for every `DataSet` field

            foreach (var field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.GetCustomAttribute<Construct>() != null) // Filter specific attribute //~ define which attributes are valid for data sets
                {
                    Debug.Assert(field.GetValue(this) != null, $"Mandatory field <b>{ field.Name }</b> not set");

                    if (field.GetValue(this) is DataSet data)
                        data.Initialize();
                }
            }
        }
    }
}
