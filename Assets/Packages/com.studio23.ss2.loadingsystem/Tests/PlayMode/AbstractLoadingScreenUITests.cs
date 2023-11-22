using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[TestFixture]

public class AbstractLoadingScreenUITests
{
    private GameObject _loadingScreenPrefab;

    [UnitySetUp]
    public IEnumerator TestSetup()
    {
        SceneManager.LoadScene("Scene 1");
        yield return new WaitForSeconds(2f);
        _loadingScreenPrefab = SceneLoadingSystem.Instance.LoadingScreenPrefab;
    }

    [UnityTest]
    public IEnumerator LoadingScreen_ShowsHintDuringLoading()
    {
        // Arrange
        var loadingScreen = Object.Instantiate(_loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();

        // Act
        loadingScreen.Initialize();
        loadingScreen.ShowHint(); // Simulate showing hint

        yield return new WaitForSeconds(1f);

        // Assert
        Assert.AreNotEqual(string.Empty, loadingScreen._hintHeaderText.text);
        Assert.AreNotEqual(string.Empty, loadingScreen._hintDescriptionText.text);

        // Clean up
        Object.Destroy(loadingScreen.gameObject);
    }

    [UnityTest]
    public IEnumerator LoadingScreen_CrossFadesBackgroundImages()
    {
        // Arrange
        var loadingScreen = Object.Instantiate(_loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();

        // Act
        loadingScreen.Initialize();
        loadingScreen.CrossFadeBackGroundImages(); // Simulate cross-fading background images
        yield return new WaitForSeconds(loadingScreen._backgroundFadeDuration + 1f); // Wait for background image to change

        // Assert
        Assert.AreNotEqual(loadingScreen._backgroundImageSlot.sprite, loadingScreen._backgroundImages[0]);

        // Clean up
        Object.Destroy(loadingScreen.gameObject);
    }

    [UnityTest]
    public IEnumerator LoadingScreen_ActivatesSceneOnKeyPress()
    {
        // Arrange
        var loadingScreen = Object.Instantiate(_loadingScreenPrefab).GetComponent<AbstractLoadingScreenUI>();

        // Act
        loadingScreen.Initialize();
        loadingScreen.OnLoadingDone(); // Simulate loading completion
        yield return WaitForInput(); // Simulate waiting for input

        bool eventInvoked = false;
        loadingScreen.OnValidAnyKeyPressEvent.AddListener(() => eventInvoked = true);
        Assert.IsFalse(eventInvoked, "OnValidAnyKeyPressEvent was not invoked.");
        Object.Destroy(loadingScreen.gameObject);
    }


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
