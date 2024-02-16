using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEditor;
using UnityEngine;
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


        public async UniTask LoadScenesWithoutLoadingScreen(List<SceneLoadingData> scenes, LoadSceneMode sceneMode = LoadSceneMode.Additive, bool activateOnload = false)
        {
            SceneLoader sceneLoader = new SceneLoader(scenes, sceneMode, activateOnload);
            sceneLoader.OnSceneLoadingComplete.AddListener(sceneLoader.ActivateScenes);
            await sceneLoader.LoadSceneAsync();
        }

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
        public async UniTask SetActiveScene(Scene activeScene)
        {
            await UniTask.WaitUntil(() => activeScene.isLoaded);
            SceneManager.SetActiveScene(activeScene);
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


        public async void LoadUnloadScene(List<SceneLoadingData> SceneToLoad, AddressableSceneData SetActiveAddressableScene, List<AddressableSceneData> SceneToUnload)
        {
            if (SceneToLoad.Count > 0)
            {
                await LoadScenesWithoutLoadingScreen(SceneToLoad);
            }

            await SetActiveScene(_scenesLoaded[SetActiveAddressableScene].LoadHandle.Result.Scene);

            if (SceneToUnload.Count > 0)
            {
                await UnloadScenes(SceneToUnload);
            }
        }
    }
}