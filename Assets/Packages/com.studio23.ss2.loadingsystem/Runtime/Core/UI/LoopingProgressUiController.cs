using Studio23.SS2.SceneLoadingSystem.UI;
using UnityEngine;

//TODO Implement this
public class LoopingProgressUiController : AbstractLoadingScreenUI
{
    private RectTransform _rectTransform;
    [SerializeField] private float _rotationSpeed;


    public override void Initialize()
    {
        //_rectTransform = _loadingImageSlot.GetComponent<RectTransform>();
        base.Initialize();
        RotateLoadingImage();
    }

    public override void UpdateProgress(float progress)
    {
        RotateLoadingImage();
    }


    private void RotateLoadingImage()
    {
        Vector3 currentRotation = _rectTransform.localEulerAngles;
        float newRotation = currentRotation.z - (_rotationSpeed * Time.deltaTime);
        _rectTransform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, newRotation);
    }


}
