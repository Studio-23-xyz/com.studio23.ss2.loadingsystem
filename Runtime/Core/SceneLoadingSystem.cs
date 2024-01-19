using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


[assembly: InternalsVisibleTo("com.studio23.ss2.sceneloadingsystem.Tests")]
namespace Studio23.SS2.SceneLoadingSystem.Core
{
   
    public class SceneLoadingSystem : MonoBehaviour
    {
        public static SceneLoadingSystem Instance;
        [SerializeField]internal GameObject _loadingScreenPrefab;


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

        /// <summary>
        /// Load a single scene by scene name
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask LoadScene(string scene, bool makeSceneActive = false)
        {
            await LoadScenes(new List<string> { scene}, makeSceneActive);
        }

        /// <summary>
        /// Load multiple scenes by a list of scene name
        /// </summary>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public async UniTask LoadScenes(List<string> scenes, bool makeSceneActive = false)
        {
            await LoadScenes(scenes, LoadSceneMode.Additive,makeSceneActive);
        }

        /// <summary>
        /// Load a single scene by scene name without loading screen
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask LoadSceneWithoutLoadingScreen(string scene, LoadSceneMode sceneMode = LoadSceneMode.Additive, bool makeSceneActive = false)
        {
            SceneLoader sceneLoader = new SceneLoader(new List<string> { scene }, sceneMode, makeSceneActive);
            sceneLoader.OnSceneLoadingComplete.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }

        /// <summary>
        /// Unload a scene by scene name
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask UnloadScene(string scene)
        {
            await SceneLoader.UnloadScene(scene);
        }

        /// <summary>
        /// Unload all scene from build settings
        /// </summary>
        public void UnloadAll()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                SceneManager.UnloadSceneAsync(i);
            }
        }

        private async UniTask LoadScenes(List<string> scenes,LoadSceneMode sceneMode, bool makeSceneActive = false)
        {
            AbstractLoadingScreenUI loadingScreen= Instantiate(_loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();
            loadingScreen.Initialize();
            SceneLoader sceneLoader= new SceneLoader(scenes,sceneMode, makeSceneActive);
            sceneLoader.OnSceneProgress.AddListener(loadingScreen.UpdateProgress);
            sceneLoader.OnSceneLoadingComplete.AddListener(loadingScreen.OnLoadingDone);
            loadingScreen.OnValidAnyKeyPressEvent.AddListener(sceneLoader.ActivateScenes);
            sceneLoader.OnSceneActivationComplete.AddListener(loadingScreen.RemoveLoadingScreen);
            await sceneLoader.LoadSceneAsync();
        }

        public void SetActiveScene(string sceneName)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }
    }
}