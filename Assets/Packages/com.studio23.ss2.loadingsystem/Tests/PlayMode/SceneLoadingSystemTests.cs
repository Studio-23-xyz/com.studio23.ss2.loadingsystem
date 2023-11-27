using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class SceneLoadingSystemTests
{
    private readonly InputTestFixture input = new();
    private SceneLoadingSystem _sceneLoadingSystem;

    [UnityTest]
    [Order(0)]
    public IEnumerator LoadSingleScene()
    {
        return UniTask.ToCoroutine(async () =>
        {
            _sceneLoadingSystem = new GameObject().AddComponent<SceneLoadingSystem>();



            SceneLoadingSystem.Instance._loadingScreenPrefab =
                Resources.Load<GameObject>("Prefabs/LoadingScreenWithLoopingImage"); //TODO
            await SceneLoadingSystem.Instance.LoadSceneWithoutLoadingScreen(SceneTable.DummyScene1);
            await UniTask.Delay(TimeSpan.FromSeconds(2f)); //RISK
            Assert.IsTrue(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
        });
    }


    [UnityTest]
    [Order(1)]
    public IEnumerator UnloadScene()
    {
        return UniTask.ToCoroutine(async () =>
        {
            
            await SceneLoadingSystem.Instance.UnloadScene(SceneTable.DummyScene1);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            Assert.IsFalse(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
        });
    }


    [UnityTest]
    [Order(2)]
    public IEnumerator LoadSingleSceneWithInput()
    {
        return UniTask.ToCoroutine(async () =>
        {
            input.Setup();
            await SceneLoadingSystem.Instance.LoadScene(SceneTable.DummyScene1);
            await UniTask.Delay(TimeSpan.FromSeconds(2f)); //RISK
            Assert.IsFalse(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
            PressKey();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Assert.IsTrue(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);

        });
    }


    [UnityTest]
    [Order(3)]
    public IEnumerator UnloadScene_2()
    {
        return UniTask.ToCoroutine(async () =>
        {
            Assert.IsTrue(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
            await SceneLoadingSystem.Instance.UnloadScene(SceneTable.DummyScene1);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            Assert.IsFalse(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
        });
    }



    [UnityTest]
    [Order(4)]
    public IEnumerator LoadMultipleScenes()
    {
        return UniTask.ToCoroutine(async () =>
        {
            await SceneLoadingSystem.Instance.LoadScenes(new List<string> { SceneTable.DummyScene1, SceneTable.DummyScene2 });
            Assert.IsFalse(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
            await UniTask.Delay(TimeSpan.FromSeconds(2f)); //RISK
            PressKey();
            await UniTask.Delay(TimeSpan.FromSeconds(1f)); //RISK
            Assert.IsTrue(SceneManager.GetSceneByName(SceneTable.DummyScene1).isLoaded);
            Assert.IsTrue(SceneManager.GetSceneByName(SceneTable.DummyScene2).isLoaded);
        });
    }

    [TearDown]
    public void ClearTestData()
    {
        input.TearDown();
        Object.Destroy(_sceneLoadingSystem);
    }



    private void PressKey()
    {
        var key = InputSystem.AddDevice<Keyboard>();
        input.Press(key.aKey);
    }
}