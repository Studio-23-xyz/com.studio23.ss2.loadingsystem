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

     
        internal ProgressEvent OnSceneProgress;
        internal UnityEvent OnSceneLoadingComplete;
        List<AsyncOperation> asyncOperation;

        internal SceneLoader(List<string> scenesToLoad,LoadSceneMode sceneLoadingMode)
        {
            OnSceneProgress = new ProgressEvent();
            OnSceneLoadingComplete = new UnityEvent();
            _scenesToLoad = scenesToLoad;
            _sceneLoadingMode = sceneLoadingMode;
            asyncOperation = new List<AsyncOperation>();
        }


        internal static async UniTask UnloadScene(string scene)
        {
            await SceneManager.UnloadSceneAsync(scene,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        internal async UniTask LoadSceneAsync()
        {
            
            foreach (var scene in _scenesToLoad)
            {
                asyncOperation.Add(SceneManager.LoadSceneAsync(scene, _sceneLoadingMode));
            }

            foreach (var operation in asyncOperation)
            {
                operation.allowSceneActivation = false;
            }


            while (asyncOperation.TrueForAll(r => r.progress < 0.9f))
            {
                float averageProgress = asyncOperation.Average(r => r.progress);
                OnSceneProgress?.Invoke(averageProgress);
                await UniTask.Yield();
            }
            await UniTask.NextFrame();
            OnSceneLoadingComplete?.Invoke();
        }

        internal void ActivateScenes()
        {
            foreach (var operation in asyncOperation)
            {
                operation.allowSceneActivation = true;
            }
        }

    }


}