using System;
using UnityEngine;

namespace Data.Racer.Player
{
    [Serializable]
    public struct PlayerInputData
    {
        public bool IsAccelerating { get; set; }
        public bool IsBraking { get; set; }
        public bool IsDrifting { get; set; }
        public bool IsUsingItem { get; set; }
        public Vector2 Steering { get; set; }
    }
}
