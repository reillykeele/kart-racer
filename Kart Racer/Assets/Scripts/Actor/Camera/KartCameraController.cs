using Cinemachine;
using KartRacer.Actor.Racer;
using KartRacer.Actor.Racer.Player;
using UnityEngine;

namespace KartRacer.Actor.Camera
{
    public class KartCameraController : MonoBehaviour
    {
        private CinemachineStateDrivenCamera _stateDrivenCamera;
        private Animator _animator;
        private PlayerInputController _input;

        void Awake()
        {
            _stateDrivenCamera = GetComponent<CinemachineStateDrivenCamera>();
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            var player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.FinishRaceEvent.AddListener(SwitchCinematic);

                _input = player.GetComponent<PlayerInputController>();
                _input?.OnLookBehindEvent.AddListener(SwitchFollowCamera);

                foreach (var camera in _stateDrivenCamera.ChildCameras)
                {
                    camera.Follow = player.transform;
                    camera.LookAt = player.transform;
                }
            }
            else
            {
                var racer = FindObjectOfType<RacerController>();
                if (racer != null)
                {
                    racer.FinishRaceEvent.AddListener(SwitchCinematic);

                    foreach (var camera in _stateDrivenCamera.ChildCameras)
                    {
                        camera.Follow = racer.transform;
                        camera.LookAt = racer.transform;
                    }
                }
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
