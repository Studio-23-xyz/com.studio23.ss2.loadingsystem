using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;

public class HorizontalProgressUIController : SceneProgressBaseUIController
{
    public override void UpdateProgress(float progress)
    {
        LoadingImage.fillAmount = progress;
    }

}
