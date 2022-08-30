using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _currSpeed;

    public float DeccelerationSpeed;

    public float AccelerationSpeed;
    public float MaxSpeed;

    public float BrakeSpeed;

    public float ReverseAccelerationSpeed;
    public float MaxReverseSpeed;

    public float TurningSpeed;

    public float GravitySpeed;
    public float ConstantGravitySpeed;

    private CharacterController _characterController;
    private PlayerInputController _input;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputController>();
    }

    void FixedUpdate()
    {
        if (_input.IsAccelerating)
        {
            Debug.Log($"{_currSpeed} Accelerating");
            _currSpeed = _currSpeed >= MaxSpeed ? MaxSpeed : _currSpeed + AccelerationSpeed;
        } 
        else if (_input.IsBraking)
        {
            // if moving forward, slow down, else reverse
            if (_currSpeed > 0)
            {
                Debug.Log($"{_currSpeed} Braking");
                _currSpeed = _currSpeed <= 0 ? 0 : _currSpeed - BrakeSpeed;
            }
            else
            {
                Debug.Log($"{_currSpeed} Reversing");
                _currSpeed = _currSpeed <= -MaxReverseSpeed ? -MaxReverseSpeed : _currSpeed - ReverseAccelerationSpeed;
            }
        }
        else
        {
            Debug.Log($"{_currSpeed} Deccelerating");
            var direction = _currSpeed > 0 ? -1 : 1;
            _currSpeed = _currSpeed == 0 ? 0 : _currSpeed + direction * DeccelerationSpeed;
        }
        
        var steerDirection = _input.Steering.x * TurningSpeed;
        _characterController.transform.Rotate(Vector3.up * steerDirection * Time.fixedDeltaTime);
        
        var movement = _characterController.transform.forward * _currSpeed * Time.fixedDeltaTime;
        if (!_characterController.isGrounded)
            movement.y -= GravitySpeed;
        else
            movement.y -= ConstantGravitySpeed;

        _characterController.Move(movement);
    }
}
