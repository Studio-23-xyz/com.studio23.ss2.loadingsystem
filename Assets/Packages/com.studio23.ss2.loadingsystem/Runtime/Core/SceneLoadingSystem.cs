using System.Collections.Generic;
using System.Threading.Tasks;
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


        public async Task LoadSceneWithoutLoadingScreen(string scene)
        {
            SceneLoader sceneLoader = new SceneLoader(new List<string> { scene }, LoadSceneMode.Additive);
            await sceneLoader.LoadSceneAsync();
        }

        public async Task UnloadScene(string scene)
        {
            await SceneLoader.UnloadScene(scene);
        }


        private async Task LoadScenes(List<string> scenes,LoadSceneMode sceneMode)
        {
            AbstractLoadingScreenUI LoadingScreen= Instantiate(LoadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();
            SceneLoader sceneLoader= new SceneLoader(scenes,sceneMode);
            sceneLoader.OnSceneProgress.AddListener(LoadingScreen.UpdateProgress);
            sceneLoader.OnSceneLoadingComplete.AddListener(LoadingScreen.OnLoadingDone);
            LoadingScreen.OnValidAnyKeyPressEvent.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }


   



    }
}