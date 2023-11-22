using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Studio23.SS2.SceneLoadingSystem.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneLoadingSystemTests
{
    [UnitySetUp]
    public IEnumerator TestSetup()
    {
        SceneManager.LoadScene("Scene 1");
        yield return null;
    }

    [UnityTest]
    [Order(0)]
    public IEnumerator LoadSingleScene()
    {
        SceneLoadingSystem.Instance.LoadScene("Scene 2");
        yield return new WaitForSeconds(2f);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 2").isLoaded);
    }

    [UnityTest]
    [Order(1)]
    public IEnumerator LoadSingleSceneWithInput()
    {
        SceneLoadingSystem.Instance.LoadScene("Scene 2");
        yield return WaitForInput();
        //yield return new WaitForSeconds(2f);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 2").isLoaded);
    }


    [UnityTest]
    [Order(2)]

    public IEnumerator LoadMultipleScenes()
    {
        SceneLoadingSystem.Instance.LoadScenes(new List<string> { "Scene 2", "Scene 3" });
        yield return new WaitForSeconds(2f);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 2").isLoaded);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 3").isLoaded);
    }


    [UnityTest]
    [Order(3)]
    public IEnumerator LoadMultipleScenesWithInput()
    {
        SceneLoadingSystem.Instance.LoadScenes(new List<string> { "Scene 2", "Scene 3" });
        yield return WaitForInput();
        //yield return new WaitForSeconds(2f);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 2").isLoaded);
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 3").isLoaded);
    }


    [UnityTest]
    [Order(4)]
    public IEnumerator LoadSceneWithoutLoadingScreen() => UniTask.ToCoroutine(async () =>
    {
        await SceneLoadingSystem.Instance.LoadSceneWithoutLoadingScreen("Scene 2");
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        Assert.IsTrue(SceneManager.GetSceneByName("Scene 2").isLoaded);
    });


    [UnityTest]
    [Order(5)]
    public IEnumerator UnloadScene() => UniTask.ToCoroutine(async () =>
    {
        SceneLoadingSystem.Instance.LoadScene("Scene 2");
        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        await SceneLoadingSystem.Instance.UnloadScene("Scene 2");
        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        Assert.IsFalse(SceneManager.GetSceneByName("Scene 2").isLoaded);
    });

    private IEnumerator WaitForInput()
    {
        bool isPressed = false;

        InputSystem.onAnyButtonPress.CallOnce(ctrl => isPressed = true);

        while (!isPressed)
        {
            yield return null;
        }
    }
}
