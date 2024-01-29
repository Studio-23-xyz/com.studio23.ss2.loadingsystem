using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;

public class ExampleLoadScene : MonoBehaviour
{
    public async void LoadScene()
    {
        List<string> sceneToLoad = new List<string>();
        sceneToLoad.Add(FakeSceneTable.FakeScene2);
        sceneToLoad.Add(FakeSceneTable.FakeScene3);
        await SceneLoadingSystem.Instance.LoadScenes(sceneToLoad);
        await SceneLoadingSystem.Instance.SetActiveScene(FakeSceneTable.FakeScene2);
    }
}
