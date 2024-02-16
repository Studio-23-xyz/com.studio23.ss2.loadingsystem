using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleLoadScene : MonoBehaviour
{
    public List<SceneLoadingData> SceneToLoad;
    public AddressableSceneData SetActiveAddressableScene;
    public List<AddressableSceneData> SceneToUnload;
    public GameObject LoadingPrefab;

    [ContextMenu("Load")]
    public async void LoadUnloadScene()
    {
        SceneLoadingSystem.Instance.LoadUnloadScene(SceneToLoad, SetActiveAddressableScene, SceneToUnload);
        //if (SceneToLoad.Count > 0)
        //{
        //    await SceneLoadingSystem.Instance.LoadScenes(SceneToLoad,LoadingPrefab);
        //}
        //await SceneLoadingSystem.Instance.SetActiveScene(SetActiveAddressableScene.ToString());
        //if (SceneToUnload.Count > 0)
        //{
        //    await SceneLoadingSystem.Instance.UnloadScenes(SceneToUnload);
        //}
    }

}
