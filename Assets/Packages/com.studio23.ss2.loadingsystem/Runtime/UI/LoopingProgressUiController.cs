using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoopingProgressUiController : AbstractLoadingScreenUI
{
    private RectTransform _rectTransform;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Image _loadingImageSlot;

    public override void Initialize()
    {
        _rectTransform = _loadingImageSlot.GetComponent<RectTransform>();
        base.Initialize();
    }

    public override void UpdateProgress(float progress)
    {
        return;
    }

    void Update()
    {
        _rectTransform.localEulerAngles += new Vector3(0,0, _rotationSpeed * Time.deltaTime);
    }


}
