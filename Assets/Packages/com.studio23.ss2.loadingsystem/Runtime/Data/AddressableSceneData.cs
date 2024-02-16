using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/LoadingSystem/AddressableSceneData", fileName = "AddressableSceneData")]
    public class AddressableSceneData : ScriptableObject
    {
        public AssetReference Scene;
    }
}