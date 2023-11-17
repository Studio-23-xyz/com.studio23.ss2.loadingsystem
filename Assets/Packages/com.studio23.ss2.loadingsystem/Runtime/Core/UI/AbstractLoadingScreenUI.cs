using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio23.SS2.SceneLoadingSystem.UI
{
    public abstract class AbstractLoadingScreenUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]private List<Sprite> _backgroundImages;
    

        [Header("UI")]
        [SerializeField] private Image _backgroundImageSlot;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private TextMeshProUGUI _pressAnyKeyText;

        [Header("Config")]
        [SerializeField] private float _hintShowDuration;
        [SerializeField] private float _backgroundFadeDuration;


        private CancellationTokenSource _hintCancelTokenSource;
        private CancellationTokenSource _backgroundImageCancelTokenSource;


        private bool _canActivateScene;

        public UnityEvent OnValidAnyKeyPressEvent;


        public virtual void Initialize()
        {
            _canActivateScene = false;
            _hintCancelTokenSource = new CancellationTokenSource();
            _backgroundImageCancelTokenSource = new CancellationTokenSource();

            if (_backgroundImages.Count > 0)
            {
                _backgroundImages = Shuffle(_backgroundImages);
                _backgroundImageSlot.sprite = _backgroundImages[0];
            }

            ShowHint();

            CrossFadeBackGroundImages();
        }

        public void OnLoadingDone()
        {
            _hintCancelTokenSource.Cancel();
            _backgroundImageCancelTokenSource.Cancel();
            _pressAnyKeyText.gameObject.SetActive(true);
            _canActivateScene= true;
        }

        public void OnAnyKeyPress()
        {
            //TODO hookup with Input System
            if (!_canActivateScene) return;
            OnValidAnyKeyPressEvent?.Invoke();
            Destroy(gameObject);//Maybe wait a frame potential bug Should test

        }


        public virtual void UpdateProgress(float progress){}


        private async void ShowHint()
        {
           //TODO Implement this
           throw new NotImplementedException();
        }

        private List<T> Shuffle<T>(List<T> sourceList)
        {
            var tempHints = new List<T>(sourceList);
            if(tempHints.Count <= 0 ) return null;
            tempHints.RemoveAt(0);
            tempHints = tempHints.OrderBy(_ => Guid.NewGuid()).ToList();
            tempHints.Add(sourceList[0]);
            sourceList = new List<T>(tempHints);
            return sourceList;
        }

        private async void CrossFadeBackGroundImages()
        {
            float timer = 0.0f;
            Image currentImage = _backgroundImageSlot.GetComponent<Image>();
            int currentAlpha = (int)currentImage.color.a;
            _backgroundImages = Shuffle(_backgroundImages);
            Sprite nextImage = _backgroundImages[0];

            if (_backgroundImages.Count < 2) return;
            while (true)
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
                        _backgroundImages = Shuffle(_backgroundImages);
                        nextImage = _backgroundImages[0];
                    }
                    else currentAlpha = 1;
                }

                await UniTask.NextFrame(cancellationToken: _backgroundImageCancelTokenSource.Token,
                    cancelImmediately: true);
            }
        }
    }
}
