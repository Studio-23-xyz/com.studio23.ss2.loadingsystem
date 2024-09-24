using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEngine;

[System.Serializable]
public class SceneLoadingData
{
    public AddressableSceneData SceneData;
    public bool IsLoadAsSingle;
    public bool ActivateOnLoad;
}
