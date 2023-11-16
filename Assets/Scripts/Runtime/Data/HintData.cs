using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{

    public class GeneratedHints :ScriptableObject
    {
        public List<HintData> HintsInfo;
    }

    [System.Serializable]
    public class HintData
    {
        public HintType HintType;
        public HintStyle HintStyle;
        public List<string> Hints;

        public string GetHint()
        {
            return Hints.FirstOrDefault();
        }

        private void ShuffleHints()
        {
            var tempHints = Hints;
            if(tempHints.FirstOrDefault()!=null) tempHints.RemoveAt(0);

            var shuffledList = tempHints.OrderBy(_ => Guid.NewGuid()).ToList();
            shuffledList.Add(Hints[0]);

            Hints = shuffledList;
        }
    }


    public enum HintType
    {
        None,
        Ui,
        GamePlay,
    }

    public enum HintStyle
    {
        None,
        Normal,
        Bold,
        Italic,
    }
}
