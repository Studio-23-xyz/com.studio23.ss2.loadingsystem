
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/LoadingSystem/SceneSetAction", fileName = "SceneSetAction")]

    public class SceneSetOperation : SceneOperationAction
    {
        public AddressableSceneData SetActiveAddressableScene;

        public override async UniTask DoSceneOperation()
        {
            var sceneHandleData = Core.SceneLoadingSystem.Instance.GetHandleData(SetActiveAddressableScene);
            if (sceneHandleData.IsValid())
                await Core.SceneLoadingSystem.Instance.SetActiveScene(sceneHandleData);

        }
    }

}