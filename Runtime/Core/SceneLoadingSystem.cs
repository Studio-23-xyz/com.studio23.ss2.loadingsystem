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
        /// Load a single sceneToUnload by sceneToUnload name
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask LoadScene(string scene)
        {
            await LoadScenes(new List<string> { scene});
        }

        /// <summary>
        /// Load multiple scenes by a list of sceneToUnload name
        /// </summary>
        /// <param name="scenes"></param>
        /// <returns></returns>
        public async UniTask LoadScenes(List<string> scenes)
        {
            await LoadScenes(scenes, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Load a single sceneToUnload by sceneToUnload name without loading screen
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask LoadSceneWithoutLoadingScreen(string scene, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            await LoadWithoutLoadingScreen(new List<string> { scene }, sceneMode);
        }


        /// <summary>
        /// Load multiple scenes by sceneToUnload name without loading screen
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask LoadScenesWithoutLoadingScreen(List<string> scene, LoadSceneMode sceneMode = LoadSceneMode.Additive)
        {
            await LoadWithoutLoadingScreen(scene, sceneMode);
        }

        /// <summary>
        /// Unload a sceneToUnload by sceneToUnload name
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public async UniTask UnloadScene(string scene)
        {
            await SceneLoader.UnloadScene(scene);
        }

        /// <summary>
        /// Unload multiple by sceneToUnload name
        /// </summary>
        /// <param name="sceneToUnload"></param>
        /// <returns></returns>
        public async UniTask UnloadScenes(List<string> sceneToUnload)
        {
            List<UniTask> sceneToUnloadsTask = new List<UniTask>();
            foreach (var scene in sceneToUnload)
            {
                sceneToUnloadsTask.Add(SceneLoader.UnloadScene(scene));
            }
            await UniTask.WhenAll(sceneToUnloadsTask);
        }

        /// <summary>
        /// Unload all sceneToUnload from build settings
        /// </summary>
        public void UnloadAll()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                SceneManager.UnloadSceneAsync(i);
            }
        }

        private async UniTask LoadWithoutLoadingScreen(List<string> scenes, LoadSceneMode sceneMode)
        {
            SceneLoader sceneLoader = new SceneLoader(scenes, sceneMode);
            sceneLoader.OnSceneLoadingComplete.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }

        private async UniTask LoadScenes(List<string> scenes,LoadSceneMode sceneMode)
        {
            AbstractLoadingScreenUI loadingScreen= Instantiate(_loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();
            loadingScreen.Initialize();
            SceneLoader sceneLoader= new SceneLoader(scenes,sceneMode);
            sceneLoader.OnSceneProgress.AddListener(loadingScreen.UpdateProgress);
            sceneLoader.OnSceneLoadingComplete.AddListener(loadingScreen.OnLoadingDone);
            loadingScreen.OnValidAnyKeyPressEvent.AddListener(sceneLoader.ActivateScenes);
            sceneLoader.OnSceneActivationComplete.AddListener(loadingScreen.RemoveLoadingScreen);
            await sceneLoader.LoadSceneAsync();
        }

        /// <summary>
        /// Make a single scene active
        /// </summary>
        public async UniTask SetActiveScene(string activeScene)
        {
            Scene sceneToActivate = SceneManager.GetSceneByName(activeScene);
            if(!sceneToActivate.IsValid()) return;
            await UniTask.WaitUntil(() => sceneToActivate.isLoaded);
            SceneManager.SetActiveScene(sceneToActivate);
        }
    }
}