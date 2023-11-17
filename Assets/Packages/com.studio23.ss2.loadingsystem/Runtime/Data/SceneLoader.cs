using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    internal class ProgressEvent:UnityEvent<float>{}
    internal class SceneLoader
    {
        internal List<string> ScenesToLoad;
        internal LoadSceneMode SceneLoadingMode;

     
        internal ProgressEvent OnSceneProgress;
        internal UnityEvent OnSceneLoadingComplete;
        List<AsyncOperation> asyncOperation;

        internal SceneLoader(List<string> scenesToLoad,LoadSceneMode sceneLoadingMode)
        {
            ScenesToLoad = scenesToLoad;
            SceneLoadingMode = sceneLoadingMode;
            asyncOperation = new List<AsyncOperation>();
        }


        internal static async UniTask UnloadScene(string scene)
        {
            await SceneManager.UnloadSceneAsync(scene,UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        internal async UniTask LoadSceneAsync()
        {
            
            foreach (var scene in ScenesToLoad)
            {
                asyncOperation.Add(SceneManager.LoadSceneAsync(scene, SceneLoadingMode));
            }

            foreach (var operation in asyncOperation)
            {
                operation.allowSceneActivation = false;
            }


            while (asyncOperation.TrueForAll(r => r.progress < 0.9f))
            {
                float totalProgress = 0f;
                for (int i = 0; i < asyncOperation.Count; i++)
                {
                    totalProgress += asyncOperation[i].progress;
                }

                float averageProgress = totalProgress / asyncOperation.Count;
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