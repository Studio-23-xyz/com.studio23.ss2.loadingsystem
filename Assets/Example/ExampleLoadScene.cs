using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;

public class ExampleLoadScene : MonoBehaviour
{
    public void LoadScene()
    {
        List<string> sceneToLoad = new List<string>();
        sceneToLoad.Add(SceneTable.Scene2);
        sceneToLoad.Add(SceneTable.Scene3);
        LoadingSceneManager.Instance.LoadScene(scenesToLoad: sceneToLoad, type:HintType.Ui , progressBarType:ProgressBarType.Looping);
    }
}
