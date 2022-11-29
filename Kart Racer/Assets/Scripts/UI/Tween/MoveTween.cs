using UnityEngine;
using Util.Enums;

namespace UI.Tween
{
    public class MoveTween : BaseTween
    {
        [Header("Move Tween")]
        [SerializeField] private Vector2 _moveFrom = Vector2.zero;
        [SerializeField] private Vector2 _moveTo = Vector2.one;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;
        [SerializeField] private bool _reverseOnOut = false;

        public override void Tween()
        {
            _rectTransform.anchoredPosition = _moveFrom;
            LeanTween.move(_rectTransform, _moveTo, _duration).setDelay(_delay).setEase(_easeType);
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

            _rectTransform.anchoredPosition = _moveTo;
            LeanTween.move(_rectTransform, _moveFrom, _duration).setDelay(_delay).setEase(_easeType);
        }
    }
}