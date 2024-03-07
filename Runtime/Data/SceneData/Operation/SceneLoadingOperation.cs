


using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/LoadingSystem/SceneLoadingAction", fileName = "SceneLoadingAction")]

    public class SceneLoadingOperation : SceneOperationAction
    {
        public List<SceneLoadingData> SceneData;
        public GameObject PrefabGameObject;
        public override async UniTask DoSceneOperation()
        {
            if(PrefabGameObject == null)
                await Core.SceneLoadingSystem.Instance.LoadScenesWithoutLoadingScreen(SceneData);
            else
                await Core.SceneLoadingSystem.Instance.LoadScenes(SceneData);
        }
    }
}
