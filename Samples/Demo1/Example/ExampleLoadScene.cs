using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;

public class ExampleLoadScene : MonoBehaviour
{
    public SceneOperationAction SceneToLoad;
    public SceneOperationAction SetActiveAddressableScene;
    public SceneOperationAction SceneToUnload;

    public async void LoadUnloadScene()
    {
        if(SceneToLoad!=null)
            await SceneToLoad.DoSceneOperation();
        if(SetActiveAddressableScene!=null)
            await SetActiveAddressableScene.DoSceneOperation();
        if(SceneToUnload!=null)
            await SceneToUnload.DoSceneOperation();
    }


}
