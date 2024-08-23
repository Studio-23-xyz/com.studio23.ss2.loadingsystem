
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    public class AddressableSceneHandle
    {
        internal AsyncOperationHandle<SceneInstance> LoadHandle { get; }

        internal AddressableSceneHandle(AddressableSceneData data, bool isSingleLoad, bool activateOnLoad)
        {
            LoadHandle = Addressables.LoadSceneAsync(data.Scene, isSingleLoad? LoadSceneMode.Single : LoadSceneMode.Additive, activateOnLoad);
        }

        internal async UniTask UnloadScene()
        { 
            var operation =  Addressables.UnloadSceneAsync(LoadHandle);
            await UniTask.WaitUntil(() => operation.IsDone == false);
        }
    }
}

