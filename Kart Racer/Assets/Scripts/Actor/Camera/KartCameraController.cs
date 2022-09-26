using System.Collections;
using System.Collections.Generic;
using Actor.Racer.Player;
using UnityEngine;

public class KartCameraController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInputController _input;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = FindObjectOfType<PlayerInputController>();
        if (_input != null)
            _input.OnLookBehindEvent.AddListener(SwitchFollowCamera);
        else
            enabled = false;
    }

    public void SwitchFollowCamera(bool lookingBehind)
    {
        if (lookingBehind)
            _animator.Play("CameraFollowRear");
        else
            _animator.Play("CameraFollowFront");
    }


}
