using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    public class SceneOperationAction : ScriptableObject
    {
        public virtual async UniTask DoSceneOperation()
        {
            await UniTask.Yield();
        }
    }
}
