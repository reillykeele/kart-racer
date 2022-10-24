using Actor.Racer;
using Actor.Racer.Player;
using UnityEngine;

namespace Actor.Camera
{
    public class KartCameraController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerInputController _input;

        void Awake()
        {
            _animator = GetComponent<Animator>();

            _input = FindObjectOfType<PlayerInputController>();
            if (_input != null)
            {
                _input.OnLookBehindEvent.AddListener(SwitchFollowCamera);
            }

            var bruh = FindObjectOfType<PlayerController>() ?? FindObjectOfType<RacerController>();
            if (bruh != null)
            {
                bruh.FinishRaceEvent.AddListener(SwitchCinematic);
            }
        }

        public void SwitchFollowCamera(bool lookingBehind)
        {
            if (lookingBehind)
                _animator.Play("CameraFollowRear");
            else
                _animator.Play("CameraFollowFront");
        }

        public void SwitchCinematic(float _)
        {
            _animator.Play("CameraCinematic");
        }

    }
}
