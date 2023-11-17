using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Studio23.SS2.SceneLoadingSystem.Core
{
    public class LoadingSceneManager : MonoBehaviour
    {
        private static LoadingSceneManager _instance;
        public UnityEvent OnSceneLoadCompleted;
        public static LoadingSceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LoadingSceneManager>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("LoadingSceneManager");
                        _instance = singletonObject.AddComponent<LoadingSceneManager>();
                    }
                }

                return _instance;
            }
        }


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(bool isAsync = true, List<string> scenesToLoad = null, HintType type  = HintType.None,
            ProgressBarType progressBarType = ProgressBarType.None, LoadSceneMode sceneLoadingMode = LoadSceneMode.Additive)
        {
            
            var loadingScreenGameObject = Instantiate(ReturnLoadingScreenData(progressBarType));
            loadingScreenGameObject.TryGetComponent(out SceneProgressBaseUIController sceneProgressUi);
            var sceneData = new SceneData(isAsync, scenesToLoad, type, sceneLoadingMode, sceneProgressUi);
        }

        public List<string> GetCurrentlyLoadedScenes()
        {
            var loadedScene = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                    loadedScene.Add(scene.name);
            }

            return loadedScene;
        }

        private GameObject ReturnLoadingScreenData(ProgressBarType progressBarType)
        {
            var resourceData = Resources.Load<LoadingScreenData>("ProgressBarData");
            if (resourceData != null)
            {
                return resourceData.ReturnProgressGameObject(progressBarType);
            }

            return null;
        }
    }
}