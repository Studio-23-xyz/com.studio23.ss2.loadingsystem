using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio23.SS2.SceneLoadingSystem.UI
{
    public class SceneProgressBaseUIController : MonoBehaviour
    {
        public Image BackgroundImage;
        public Image LoadingImage;
        public TextMeshProUGUI HintText;


        void OnEnable()
        {
            LoadingSceneManager.Instance.OnSceneLoadCompleted.AddListener(HideCanvas);
        }

        void OnDisable()
        {
            LoadingSceneManager.Instance.OnSceneLoadCompleted.RemoveListener(HideCanvas);
        }

        public virtual void Initialize(HintType type)
        {
            ShowHints(type);
        }

        private void HideCanvas()
        {
            gameObject.SetActive(false);
        }


        public virtual void UpdateProgress(float progress){}

        private void ShowHints(HintType type)
        {
            
        }
    }
}
