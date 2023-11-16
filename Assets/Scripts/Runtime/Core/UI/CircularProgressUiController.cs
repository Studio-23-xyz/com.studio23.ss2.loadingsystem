using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;

public class CircularProgressUiController : SceneProgressBaseUIController
{
    public override void UpdateProgress(float progress)
    {
        LoadingImage.fillAmount = progress;
    }
}
