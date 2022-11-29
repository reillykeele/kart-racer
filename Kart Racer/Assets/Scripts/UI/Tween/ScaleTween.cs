using UnityEngine;
using Util.Enums;

namespace UI.Tween
{
    public class ScaleTween : BaseTween
    {
        [Header("Scale Tween")]
        [SerializeField] private Vector3 _scaleFrom = Vector3.zero;
        [SerializeField] private Vector3 _scaleTo = Vector3.one;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;
        [SerializeField] private bool _reverseOnOut = false;

        
        public override void Tween()
        {
            _rectTransform.localScale = _scaleFrom;
            LeanTween.scale(_rectTransform, _scaleTo, _duration).setDelay(_delay).setDelay(_delay).setEase(_easeType);
        }

        public override void TweenOut()
        {
            if (_tweenDirection != TweenDirection.Out && _tweenDirection != TweenDirection.InAndOut)
                return;

            if (!_reverseOnOut)
            {
                Tween();
                return;
            }

            _rectTransform.localScale = _scaleTo;
            LeanTween.scale(_rectTransform, _scaleFrom, _duration).setDelay(_delay).setEase(_easeType);
        }
    }
}