using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class LoadingSceneManager : MonoBehaviour
{

    private static LoadingSceneManager _instance;
    public List<GeneratedSceneData> SceneDatas;
    public UnityEvent OnSceneLoadCompleted;

    public static LoadingSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingSceneManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("LoadingSceneManager");
                    _instance = singletonObject.AddComponent<LoadingSceneManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Initialize()
    {
        SceneDatas = new List<GeneratedSceneData>();
    }


    public void LoadScene(bool isLoadingSceneEnabled, bool isAsync, List<string> scenesToLoad, List<string> hintsToShow, GameObject loadingScreen, LoadSceneMode sceneLoadingMode)
    {
        SceneProgressUIController sceneProgressUi = new SceneProgressUIController();
        if (isLoadingSceneEnabled)
        {
            var loadingScreenGameObject = Instantiate(loadingScreen);
            sceneProgressUi = loadingScreenGameObject.GetComponent<SceneProgressUIController>();
        }
        
        SceneDatas.Add(new GeneratedSceneData(isLoadingSceneEnabled, isAsync,scenesToLoad, hintsToShow,sceneLoadingMode, sceneProgressUi));
    }


    public List<string> GetCurrentlyLoadedScenes()
    {
        var loadedScene = new List<string>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
                loadedScene.Add(scene.name);
        }

        return loadedScene;
    }

}


[System.Serializable]
public class GeneratedSceneData
{
    public bool IsLoadingSceneEnabled;
    public bool IsAsync;
    public List<string> ScenesToLoad;
    public List<string> HintsToShow;
    public LoadSceneMode SceneLoadingMode;
    public SceneProgressUIController GeneratedLoadingScreenUi;


    

    public GeneratedSceneData(bool isLoadingSceneEnabled, bool isAsync, List<string> scenesToLoad, List<string> hintsToShow, LoadSceneMode sceneLoadingMode, SceneProgressUIController generatedLoadingScreenUi)
    {
        IsLoadingSceneEnabled = isLoadingSceneEnabled;
        IsAsync = isAsync;
        ScenesToLoad = scenesToLoad;
        HintsToShow = hintsToShow;
        SceneLoadingMode = sceneLoadingMode;
        GeneratedLoadingScreenUi = generatedLoadingScreenUi;

        SceneLoadDecision();
    }

    public async UniTask LoadSceneAsync(List<string> sceneNames, LoadSceneMode mode)
    {
        List<AsyncOperation> asyncOperation = new List<AsyncOperation>();

        foreach (var scene in sceneNames)
        {
            asyncOperation.Add(SceneManager.LoadSceneAsync(scene, mode));
        }

        foreach (var operation in asyncOperation)
        {
            operation.allowSceneActivation = false;
        }


        bool allScenesLoaded = false;
        while (!allScenesLoaded)
        {
            float totalProgress = 0f;
            int completedScenes = 0;

            for (int i = 0; i < asyncOperation.Count; i++)
            {
                totalProgress += asyncOperation[i].progress;

                if (asyncOperation[i].isDone)
                {
                    completedScenes++;
                }
            }

            float averageProgress = totalProgress / asyncOperation.Count;
            if(IsLoadingSceneEnabled)
                GeneratedLoadingScreenUi.UpdateProgress(averageProgress);
            allScenesLoaded = completedScenes == asyncOperation.Count;
            await UniTask.Yield();
        }

        foreach (var operation in asyncOperation)
        {
            operation.allowSceneActivation = true;
        }

        LoadingSceneManager.Instance.OnSceneLoadCompleted?.Invoke();
        await UniTask.NextFrame();

    }

    private void LoadSceneNormally(List<string> sceneNames, LoadSceneMode mode)
    {
        foreach (var scene in sceneNames)
        {
            SceneManager.LoadScene(scene, mode);
        }


        LoadingSceneManager.Instance.OnSceneLoadCompleted?.Invoke();
    }

    private async void SceneLoadDecision()
    {
        if(IsLoadingSceneEnabled)
            GeneratedLoadingScreenUi.ShowHints(HintsToShow);

        if (IsAsync)
        {
            await LoadSceneAsync(ScenesToLoad, SceneLoadingMode);
        }
        else
        {
            LoadSceneNormally(ScenesToLoad, SceneLoadingMode);
        }
    }
}
