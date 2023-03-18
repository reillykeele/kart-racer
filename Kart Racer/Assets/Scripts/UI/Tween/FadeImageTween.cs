using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Util.Enums;

namespace UI.Tween
{
    [RequireComponent(typeof(Image))]
    public class FadeImageTween : BaseTween
    {
        [Header("Move Tween")]
        [SerializeField] private float _fadeFrom = 0f;
        [SerializeField] private float _fadeTo = 1f;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;

        private Image _image;

        protected override void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
        }

        public override void Tween()
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _fadeFrom);
            LeanTween.value(_image.gameObject, _fadeFrom, _fadeTo, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay)
                .setEase(_easeType);
        }

        public override void TweenOut()
        {
            if (_tweenDirection != TweenDirection.Out && _tweenDirection != TweenDirection.InAndOut)
                return;

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _fadeTo);
            LeanTween.value(gameObject, _fadeTo, _fadeFrom, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay)
                .setEase(_easeType);
        }

        private void SetAlphaOnUpdate(float a) => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
    }
}