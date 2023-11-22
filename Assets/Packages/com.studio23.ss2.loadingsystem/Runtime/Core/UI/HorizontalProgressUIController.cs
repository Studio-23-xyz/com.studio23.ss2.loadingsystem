using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.UI;


public class HorizontalProgressUIController : AbstractLoadingScreenUI
{
    [Header("UI")]
    [SerializeField] private Image _loadingImageSlot;
    public override void UpdateProgress(float progress)
    {
        _loadingImageSlot.fillAmount = progress;
    }

}
