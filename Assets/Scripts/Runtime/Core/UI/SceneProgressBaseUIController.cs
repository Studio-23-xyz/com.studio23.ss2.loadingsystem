using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Core;
using Studio23.SS2.SceneLoadingSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Studio23.SS2.SceneLoadingSystem.UI
{
    public class SceneProgressBaseUIController : MonoBehaviour
    {
        [SerializeField]private List<Sprite> _backgroundImageSprites;
        [SerializeField]private Sprite _loadingImageSprite;
        [SerializeField] private Image _backgroundImageSlot;
        [SerializeField] protected Image _loadingImageSlot;
        [SerializeField]private TextMeshProUGUI _hintText;
        [SerializeField] private float _hintShowDuration;
        [SerializeField] private float _backgroundFadeDuration;
        private CancellationTokenSource _hintCancelTokenSource;
        private CancellationTokenSource _backgroundImageCancelTokenSource;
        private List<string> _allHints;




        void OnEnable()
        {
            LoadingSceneManager.Instance.OnSceneLoadCompleted.AddListener(HideCanvas);
        }

        void OnDisable()
        {
            LoadingSceneManager.Instance.OnSceneLoadCompleted.RemoveListener(HideCanvas);
        }

        public virtual void Initialize(HintType type)
        {
            _hintCancelTokenSource = new CancellationTokenSource();
            _backgroundImageCancelTokenSource = new CancellationTokenSource();
            //if (_hintCancelTokenSource.IsCancellationRequested)
            //    throw new OperationCanceledException();
            //if(_backgroundImageCancelTokenSource.IsCancellationRequested)
            //    throw new OperationCanceledException();


            _allHints = new List<string>();

            if(_loadingImageSprite!=null)
                _loadingImageSlot.sprite = _loadingImageSprite;
            if (_backgroundImageSprites.Count > 0)
            {
                _backgroundImageSprites = Shuffle(_backgroundImageSprites);
                _backgroundImageSlot.sprite = _backgroundImageSprites[0];
            }
            GenerateHints(type);
            _allHints = Shuffle(_allHints);
            ShowHint();

            CrossFadeBackGroundImages();
        }

        private void HideCanvas()
        {
            _hintCancelTokenSource.Cancel();
            _backgroundImageCancelTokenSource.Cancel();
            Destroy(gameObject);
        }


        public virtual void UpdateProgress(float progress){}


        private async void ShowHint()
        {
            if(_allHints.Count <= 0) return;
            while (true)
            {
                _hintText.text = _allHints[0];
                _allHints = Shuffle(_allHints);
                await UniTask.Delay(TimeSpan.FromSeconds(_hintShowDuration), ignoreTimeScale: true, cancellationToken:_hintCancelTokenSource.Token, cancelImmediately: true);
            }

        }

        private void GenerateHints(HintType type)
        {
            var resourceData = Resources.Load<HintScriptableObject>("HintsData");
            if (resourceData != null)
            {
                _allHints = resourceData.ReturnHints(type);
            }
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
            _backgroundImageSprites = Shuffle(_backgroundImageSprites);
            Sprite nextImage = _backgroundImageSprites[0];

            if (_backgroundImageSprites.Count < 2) return;
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
                        _backgroundImageSprites = Shuffle(_backgroundImageSprites);
                        nextImage = _backgroundImageSprites[0];
                    }
                    else currentAlpha = 1;
                }

                await UniTask.NextFrame(cancellationToken: _backgroundImageCancelTokenSource.Token,
                    cancelImmediately: true);
            }
        }
    }
}
