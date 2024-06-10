using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    internal class SceneLoader
    {

        private readonly List<SceneLoadingData> _scenesToLoad;
        private List<AsyncOperationHandle<SceneInstance>> _loadHandles;

        internal UnityEvent<float> OnSceneProgress;
        internal UnityEvent OnSceneLoadingComplete;
        internal UnityEvent OnSceneActivationComplete;


        internal SceneLoader(List<SceneLoadingData> scenesToLoad,LoadSceneMode sceneLoadingMode = LoadSceneMode.Additive, bool activateOnLoad = false)
        {
            OnSceneProgress = new UnityEvent<float>();
            OnSceneLoadingComplete = new UnityEvent();
            OnSceneActivationComplete = new UnityEvent();
            _scenesToLoad = scenesToLoad;
            _loadHandles = new List<AsyncOperationHandle<SceneInstance>>();
        }

        internal async UniTask LoadSceneAsync()
        {
            foreach (var scene in _scenesToLoad)
            {
                var sceneLoadHandle = new AddressableSceneHandle(scene.SceneData, scene.IsLoadAsSingle, scene.ActivateOnLoad);
                _loadHandles.Add(sceneLoadHandle.LoadHandle);
                Core.SceneLoadingSystem.Instance.PopulateSceneLoadData(scene.SceneData,sceneLoadHandle);
            }
            while (!_loadHandles.TrueForAll(r => r.Status == AsyncOperationStatus.Succeeded))
            {
                float averageProgress = _loadHandles.Average(r => r.PercentComplete);
                OnSceneProgress?.Invoke(averageProgress);
                await UniTask.Yield();
            }
            OnSceneProgress?.Invoke(1.0f);
            OnSceneLoadingComplete?.Invoke();
            await UniTask.WaitUntil(() => _loadHandles.TrueForAll(r => r.Result.Scene.isLoaded));
        }

        internal async void ActivateScenes()
        {
            foreach (var operation in _loadHandles)
            {
                operation.Result.ActivateAsync();
            }

            await UniTask.WaitForFixedUpdate();

            foreach (var operation in _loadHandles)
            {
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                }
            }
            OnSceneActivationComplete?.Invoke();
        }
    }
}