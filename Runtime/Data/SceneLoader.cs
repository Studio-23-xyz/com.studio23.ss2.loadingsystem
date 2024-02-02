using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    internal class ProgressEvent:UnityEvent<float>{}
    internal class SceneLoader
    {
        private List<string> _scenesToLoad;
        private LoadSceneMode _sceneLoadingMode;
        private List<AsyncOperation> _asyncOperation;

        internal ProgressEvent OnSceneProgress;
        internal UnityEvent OnSceneLoadingComplete;
        internal UnityEvent OnSceneActivationComplete;


        internal SceneLoader(List<string> scenesToLoad,LoadSceneMode sceneLoadingMode)
        {
            OnSceneProgress = new ProgressEvent();
            OnSceneLoadingComplete = new UnityEvent();
            OnSceneActivationComplete = new UnityEvent();
            _scenesToLoad = scenesToLoad;
            _sceneLoadingMode = sceneLoadingMode;
            _asyncOperation = new List<AsyncOperation>();
        }

        internal static async UniTask UnloadScene(string scene)
        {
            await SceneManager.UnloadSceneAsync(scene,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        internal async UniTask LoadSceneAsync()
        {
            foreach (var scene in _scenesToLoad)
            {
                AsyncOperation tempOperation = SceneManager.LoadSceneAsync(scene, _sceneLoadingMode);
                tempOperation.allowSceneActivation = false;
                _asyncOperation.Add(tempOperation);
            }

            while (!_asyncOperation.TrueForAll(r => r.progress >= 0.9f))
            {
                float averageProgress = _asyncOperation.Average(r => r.progress);
                OnSceneProgress?.Invoke(averageProgress);
                await UniTask.Yield();
            }

            OnSceneProgress?.Invoke(1.0f);

            await UniTask.NextFrame();

            OnSceneLoadingComplete?.Invoke();
        }

        internal async void ActivateScenes()
        {
            foreach (var operation in _asyncOperation)
            {
                operation.allowSceneActivation = true;
            }

            foreach (var operation in _asyncOperation)
            {
                while (!operation.isDone)
                {
                    await UniTask.Yield();

                }
            }
            OnSceneActivationComplete?.Invoke();
        }
    }
}