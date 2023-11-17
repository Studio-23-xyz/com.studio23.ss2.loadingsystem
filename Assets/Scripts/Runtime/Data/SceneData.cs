using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    
    [System.Serializable]
    public class SceneData
    {
        public bool IsAsync;
        public List<string> ScenesToLoad;
        public HintType HintType;
        public LoadSceneMode SceneLoadingMode;
        public SceneProgressBaseUIController GeneratedLoadingScreenBaseUi;

        public SceneData(bool isAsync, List<string> scenesToLoad, HintType type,
            LoadSceneMode sceneLoadingMode, SceneProgressBaseUIController generatedLoadingScreenBaseUi)
        {
            IsAsync = isAsync;
            ScenesToLoad = scenesToLoad;
            HintType = type;
            SceneLoadingMode = sceneLoadingMode;
            GeneratedLoadingScreenBaseUi = generatedLoadingScreenBaseUi;


            SceneLoadDecision();
        }

        private async UniTask LoadSceneAsync()
        {
            List<AsyncOperation> asyncOperation = new List<AsyncOperation>();

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
                GeneratedLoadingScreenBaseUi.UpdateProgress(averageProgress);

                await UniTask.Yield();
            }

            foreach (var operation in asyncOperation)
            {
                operation.allowSceneActivation = true;
            }

            LoadingSceneManager.Instance.OnSceneLoadCompleted?.Invoke();
            await UniTask.NextFrame();

        }

        private void LoadSceneNormally()
        {
            foreach (var scene in ScenesToLoad)
            {
                SceneManager.LoadScene(scene, SceneLoadingMode);
            }

            LoadingSceneManager.Instance.OnSceneLoadCompleted?.Invoke();
        }

        private async void SceneLoadDecision()
        {
            GeneratedLoadingScreenBaseUi.Initialize(HintType);

            if (IsAsync)
            {
                await LoadSceneAsync();
            }
            else
            {
                LoadSceneNormally();
            }
        }
    }


}