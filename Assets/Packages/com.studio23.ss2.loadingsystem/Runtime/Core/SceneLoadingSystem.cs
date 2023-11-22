using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace Studio23.SS2.SceneLoadingSystem.Core
{
    public class SceneLoadingSystem : MonoBehaviour
    {
        public static SceneLoadingSystem Instance;
        public GameObject LoadingScreenPrefab;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string scene)
        {
            LoadScenes(new List<string> { scene});
        }

        public async void LoadScenes(List<string> scenes)
        {
            await LoadScenes(scenes, LoadSceneMode.Additive);
        }

        public async UniTask LoadSceneWithoutLoadingScreen(string scene)
        {
            SceneLoader sceneLoader = new SceneLoader(new List<string> { scene }, LoadSceneMode.Additive);
            sceneLoader.OnSceneLoadingComplete.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }

        public async Task UnloadScene(string scene)
        {
            await SceneLoader.UnloadScene(scene);
        }

        private async Task LoadScenes(List<string> scenes,LoadSceneMode sceneMode)
        {
            AbstractLoadingScreenUI loadingScreen= Instantiate(LoadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();
            loadingScreen.Initialize();
            SceneLoader sceneLoader= new SceneLoader(scenes,sceneMode);
            sceneLoader.OnSceneProgress.AddListener(loadingScreen.UpdateProgress);
            sceneLoader.OnSceneLoadingComplete.AddListener(loadingScreen.OnLoadingDone);
            loadingScreen.OnValidAnyKeyPressEvent.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }
    }
}