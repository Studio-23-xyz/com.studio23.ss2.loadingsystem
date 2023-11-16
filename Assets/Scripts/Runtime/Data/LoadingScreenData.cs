using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/Scene Loading System/Progress Bar Data")]
    [System.Serializable]
    public class LoadingScreenData : ScriptableObject
    {
        public List<ProgressBarData> ProgressBarData;

    }

    [System.Serializable]
    public class ProgressBarData
    {
        public ProgressBarType ProgressBarType;
        public GameObject ProgressBarPrefab;
    }




    public enum ProgressBarType
    {
        None,
        Horizontal,
        Circular,
        Looping,
    }
}
