using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [System.Serializable]
    public class LoadingScreenData : ScriptableObject
    {
        public List<ProgressBarData> ProgressBarData;

        public GameObject ReturnProgressGameObject(ProgressBarType type )
        {
            var progressBarData = ProgressBarData.FirstOrDefault(x => x.ProgressBarType == type);
            return progressBarData?.ProgressBarPrefab;
        }
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
