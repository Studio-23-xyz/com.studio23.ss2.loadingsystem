using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using Studio23.SS2.SceneLoadingSystem.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace Studio23.SS2.SceneLoadingSystem.UI
{
    public abstract class AbstractLoadingScreenUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        public List<Sprite> _backgroundImages;
        [SerializeField] private TextStyleSettings _hintStyle;
        [SerializeField] private LoadingScreenTextTable _hintTable;

        [Header("UI")]
        [SerializeField]
        public Image _backgroundImageSlot;
        [SerializeField] public TextMeshProUGUI _hintHeaderText;
        [SerializeField] public TextMeshProUGUI _hintDescriptionText;
        [SerializeField] private TextMeshProUGUI _pressAnyKeyText;

        [Header("Config")]
        [SerializeField] private float _hintShowDuration;
        [SerializeField] public float _backgroundFadeDuration;
        [SerializeField] private bool _needKeyPress;

        private CancellationTokenSource _hintCancelTokenSource;
        private CancellationTokenSource _backgroundImageCancelTokenSource;
        private IDisposable _onKeyPressEvent;
        public UnityEvent OnValidAnyKeyPressEvent;

        public virtual void Initialize()
        {
            _hintCancelTokenSource = new CancellationTokenSource();
            _backgroundImageCancelTokenSource = new CancellationTokenSource();

            if (_backgroundImages.Count > 0)
            {
                _backgroundImages = ShuffleListExtension.Shuffle(_backgroundImages);
                _backgroundImageSlot.sprite = _backgroundImages[0];
            }
            ShowHint();
            CrossFadeBackGroundImages();
        }



        public void OnLoadingDone()
        {
            _hintCancelTokenSource.Cancel();
            _backgroundImageCancelTokenSource.Cancel();
            _hintHeaderText.text = string.Empty;
            _hintDescriptionText.text = string.Empty;
            _pressAnyKeyText.gameObject.SetActive(_needKeyPress);
            CheckIfKeyPressRequired();
        }

        protected void CheckIfKeyPressRequired()
        {
            if(!_needKeyPress) OnAnyKeyPress();
            else _onKeyPressEvent = InputSystem.onAnyButtonPress.CallOnce(ctrl => OnAnyKeyPress());
        }

        protected void OnAnyKeyPress()
        {
            _onKeyPressEvent?.Dispose();
            Debug.Log("Any Key Pressed");
            OnValidAnyKeyPressEvent?.Invoke();

        }

        public void RemoveLoadingScreen()
        {
            Destroy(gameObject, .1f);

        }

        public virtual void UpdateProgress(float progress){}

        protected async void ShowHint()
        {
            TextStyling(_hintStyle.TitleStyle,_hintHeaderText);
            TextStyling(_hintStyle.DescriptionStyle, _hintDescriptionText);

            while (!_hintCancelTokenSource.IsCancellationRequested)
            {
                var textData = _hintTable.GetHint();
                _hintHeaderText.text = textData.Title;
                _hintDescriptionText.text = textData.Description;
                await UniTask.Delay(TimeSpan.FromSeconds(_hintShowDuration),cancellationToken: _hintCancelTokenSource.Token,
                    cancelImmediately: true).SuppressCancellationThrow();
            }
        }

        protected void TextStyling(BaseTextStyleData styleData, TextMeshProUGUI textToStyle)
        {
            textToStyle.font = styleData.FontAsset;
            textToStyle.fontStyle = styleData.FontStyle;
        }


        protected async void CrossFadeBackGroundImages()
        {
            float timer = 0.0f;
            Image currentImage = _backgroundImageSlot.GetComponent<Image>();
            int currentAlpha = (int)currentImage.color.a;
            _backgroundImages = ShuffleListExtension.Shuffle(_backgroundImages);
            Sprite nextImage = _backgroundImages[0];

            if (_backgroundImages.Count < 2) return;
            while (!_backgroundImageCancelTokenSource.IsCancellationRequested)
            {
                timer += Time.deltaTime;
                var ratio = Mathf.Clamp01(timer / _backgroundFadeDuration);
                currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, MathF.Abs(currentAlpha - ratio));
                if (timer >= _backgroundFadeDuration)
                {
                    timer = 0.0f;
                    if (currentAlpha == 1)
                    {
                        currentAlpha = 0;
                        _backgroundImageSlot.sprite = nextImage;
                        _backgroundImages = ShuffleListExtension.Shuffle(_backgroundImages);
                        nextImage = _backgroundImages[0];
                    }
                    else currentAlpha = 1;
                }

                await UniTask.NextFrame(cancellationToken: _backgroundImageCancelTokenSource.Token,
                    cancelImmediately: true).SuppressCancellationThrow();
            }
        }
    }
}
