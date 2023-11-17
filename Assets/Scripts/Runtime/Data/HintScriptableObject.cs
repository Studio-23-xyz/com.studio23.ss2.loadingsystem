using Studio23.SS2.SceneLoadingSystem.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Studio23.SS2.SceneLoadingSystem.Data
{
    public class HintScriptableObject : ScriptableObject
    {
        public List<HintData> HintsInfo;

        public List<string> ReturnHints(HintType type)
        {
            var hintData = HintsInfo.FirstOrDefault(x => x.HintType == type);
            if (hintData != null)
            {
                return hintData.Hints;
            }

            return null;
        }
    }

}