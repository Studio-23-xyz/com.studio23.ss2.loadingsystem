using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [System.Serializable]
    public class TextStyleSettings : ScriptableObject
    {
        [Header("Title")] 
        public BaseTextStyleData TitleStyle;

        [Header("Description")]
        public BaseTextStyleData DescriptionStyle;
    }

    [System.Serializable]
    public class BaseTextStyleData
    {
        public TMP_FontAsset FontAsset;
        public FontStyles FontStyle;
    }
}