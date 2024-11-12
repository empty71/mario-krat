using System;
using UnityEngine;

namespace _Scripts.Values_Scripts
{
    [Serializable]
    public struct HoldingHighlight_Values
    {
        public bool enableHighlight;
        public Color pickupColor, defaultColor;
    }

    [Serializable]
    public struct HoldDrop_Values
    {
        public float dropForce, dropUpwardAngle;
    }
}