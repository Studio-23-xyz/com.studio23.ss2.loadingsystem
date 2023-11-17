using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Studio23.SS2.SceneLoadingSystem.Data
{
    public class LoadingScreenTextTable : ScriptableObject
    {
        public List<TextData> HintsInfo;

        public TextData GetHint(TextType type)
        {
            var hintData = HintsInfo.FirstOrDefault(x => x.Type == type);
            if (hintData != null)
            {
                return hintData;
            }

            return null;
        }
    }

}