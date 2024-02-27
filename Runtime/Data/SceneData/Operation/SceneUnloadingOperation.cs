
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/LoadingSystem/SceneUnLoadingAction", fileName = "SceneUnLoadingAction")]

    public class SceneUnloadingOperation : SceneOperationAction
    {
        public List<AddressableSceneData> ScenesToUnload;

        public override async UniTask DoSceneOperation()
        {
            await Core.SceneLoadingSystem.Instance.UnloadScenes(ScenesToUnload);
        }
    }
}
