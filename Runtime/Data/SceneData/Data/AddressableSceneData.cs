using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Studio23.SS2.SceneLoadingSystem.Data
{
    [CreateAssetMenu(menuName = "Studio-23/LoadingSystem/AddressableSceneData", fileName = "AddressableSceneData")]
    public class AddressableSceneData : ScriptableObject, IEquatable<AddressableSceneData>
    {
        public AssetReference Scene;

        /// <summary>
        /// This is required because this SO can be duplicated across addressable and non-addressable scenes.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AddressableSceneData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Scene.AssetGUID == other.Scene.AssetGUID;
        }
        
        /// <summary>
        /// This is required because this SO can be duplicated across addressable and non-addressable scenes.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AddressableSceneData)obj);
        }
        /// <summary>
        /// This is required because this SO can be duplicated across addressable and non-addressable scenes.
        /// Hashcode override is used for dictionaries
        /// </summary>
        /// <returns>hashcpde</returns>
        public override int GetHashCode()
        {
            return Scene.AssetGUID.GetHashCode();
        }
    }
}