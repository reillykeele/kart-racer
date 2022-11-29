using UnityEngine;
using Util.Enums;

namespace UI.Tween
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseTween : MonoBehaviour
    {
        [Header("Tween")]
        [SerializeField] public float _duration = 1f;
        [SerializeField] public float _delay = 0f;
        [SerializeField] public bool _cancelOnTween = true;
        [SerializeField] public bool _tweenInOnEnable = false;
        [SerializeField] public TweenDirection _tweenDirection = TweenDirection.In;

        // public UnityEvent OnCompletedEvent;

        protected RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable()
        {
            if (_tweenInOnEnable)
                TweenIn();
        }

        public abstract void Tween();
        public virtual void TweenIn()
        {
            if (_cancelOnTween && LeanTween.isTweening(_rectTransform))
                LeanTween.cancel(_rectTransform);

            if (ShouldTweenIn())
                Tween();
        }

        public virtual void TweenOut()
        {
            if (_cancelOnTween && LeanTween.isTweening(_rectTransform))
                LeanTween.cancel(_rectTransform);

            if (ShouldTweenOut())
                Tween();
        }

        public bool ShouldTweenIn() => _tweenDirection == TweenDirection.In || _tweenDirection == TweenDirection.InAndOut;
        public bool ShouldTweenOut() => _tweenDirection == TweenDirection.Out || _tweenDirection == TweenDirection.InAndOut;
    }
}
