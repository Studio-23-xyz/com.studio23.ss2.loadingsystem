
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    public class AddressableSceneHandle
    {
        public AsyncOperationHandle<SceneInstance> LoadHandle { get; }

        public AddressableSceneHandle(AddressableSceneData data, bool isSingleLoad, bool activateOnLoad)
        {
            LoadHandle = Addressables.LoadSceneAsync(data.Scene, isSingleLoad? LoadSceneMode.Single : LoadSceneMode.Additive, activateOnLoad);
        }

        public async UniTask UnloadScene()
        { 
            await Addressables.UnloadSceneAsync(LoadHandle);
        }
    }
}

