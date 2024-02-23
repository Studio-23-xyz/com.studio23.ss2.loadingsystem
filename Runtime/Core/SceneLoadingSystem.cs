using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

[assembly: InternalsVisibleTo("com.studio23.ss2.sceneloadingsystem.Tests")]
namespace Studio23.SS2.SceneLoadingSystem.Core
{
   
    public class SceneLoadingSystem : MonoBehaviour
    {
        public static SceneLoadingSystem Instance;
        private Dictionary<AddressableSceneData, AddressableSceneHandle> _scenesLoaded;


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

        private void Start()
        {
            _scenesLoaded = new Dictionary<AddressableSceneData, AddressableSceneHandle>();
        }

        public void PopulateSceneLoadData(AddressableSceneData addressableSceneData, AddressableSceneHandle addressableSceneHandle)
        {
            _scenesLoaded.Add(addressableSceneData, addressableSceneHandle);
        }




        /// <summary>
        /// Make scenes without loading screen
        /// </summary>
        public async UniTask LoadScenesWithoutLoadingScreen(List<SceneLoadingData> scenes, LoadSceneMode sceneMode = LoadSceneMode.Additive, bool activateOnload = false)
        {
            SceneLoader sceneLoader = new SceneLoader(scenes, sceneMode, activateOnload);
            sceneLoader.OnSceneLoadingComplete.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }


        /// <summary>
        /// Make scenes with loading screen
        /// </summary>
        public async UniTask LoadScenes(List<SceneLoadingData> scenes, GameObject loadingScreenPrefab = null)
        {
            AbstractLoadingScreenUI loadingScreen= Instantiate(loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();
            loadingScreen.Initialize();
            SceneLoader sceneLoader= new SceneLoader(scenes);
            sceneLoader.OnSceneProgress.AddListener(loadingScreen.UpdateProgress);
            sceneLoader.OnSceneLoadingComplete.AddListener(loadingScreen.OnLoadingDone);
            loadingScreen.OnValidAnyKeyPressEvent.AddListener(sceneLoader.ActivateScenes);
            sceneLoader.OnSceneActivationComplete.AddListener(loadingScreen.RemoveLoadingScreen);
            await sceneLoader.LoadSceneAsync();
        }

        /// <summary>
        /// Make a single scene active
        /// </summary>
        public async UniTask SetActiveScene(AsyncOperationHandle<SceneInstance> activeScene)
        {
            var scene = activeScene.Result.Scene;
            await UniTask.WaitUntil(() => scene.isLoaded);
            SceneManager.SetActiveScene(scene);
        }

        /// <summary>
        /// Unload multiple by sceneToUnload name
        /// </summary>
        /// <param name="sceneToUnload"></param>
        /// <returns></returns>
        public async UniTask UnloadScenes(List<AddressableSceneData> scenesToUnload)
        {
            foreach (var scene in scenesToUnload)
            {
                await _scenesLoaded[scene].UnloadScene();
            }
        }

        /// <summary>
        /// Return addressable handle data
        /// </summary>
        /// <param name="addressableScene"></param>
        /// <returns></returns>

        public AsyncOperationHandle<SceneInstance> GetHandleData(AddressableSceneData addressableScene)
        {
            AsyncOperationHandle<SceneInstance> sceneHandleData = new AsyncOperationHandle<SceneInstance>();
            if (_scenesLoaded.TryGetValue(addressableScene, out var foundLoadedScene))
                sceneHandleData =  foundLoadedScene.LoadHandle;
            return sceneHandleData;
        }
    }
}