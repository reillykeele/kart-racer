using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerInputController : MonoBehaviour
{
    public bool IsAccelerating { get; set; }
    public bool IsBraking { get; set; }
    public bool IsDrifting { get; set; }
    public bool IsUsingItem { get; set; }
    public Vector2 Steering { get; set; }

    public void OnAccelerate(InputValue val) => IsAccelerating = val.isPressed;

    public void OnBrakeReverse(InputValue val) => IsBraking = val.isPressed;

    public void OnDrift(InputValue val) => IsDrifting = val.isPressed;

    public void OnItem(InputValue val) => IsUsingItem = val.isPressed;

    public void OnSteer(InputValue val) => Steering = val.Get<Vector2>();

}
