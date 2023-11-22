using System.Collections.Generic;
using System.Linq;
using Studio23.SS2.SceneLoadingSystem.Extension;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Scene Loading System/Loading Screen TextTable")]
    [System.Serializable]
    public class LoadingScreenTextTable : ScriptableObject
    {
        public List<TextData> HintsInfo;

        public TextData GetHint(TextType type)
        {
            HintsInfo = ShuffleListExtension.Shuffle(HintsInfo);
            var hintData = HintsInfo.FindAll(x => x.Type == type);
            return hintData[Random.Range(0,hintData.Count)];
        }

        public TextData GetHint()
        {
            HintsInfo = ShuffleListExtension.Shuffle(HintsInfo);
            var hintData = HintsInfo.FirstOrDefault();
            return hintData != null ? HintsInfo[0] : null;
        }
    }
}