using Artem_Library.Attribute_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Values_Scripts
{
    [System.Serializable]
    public struct ThirdPersonController_Values
    {
        [Header("Character Environment Values")]
        public float gravity;
        public float fallTimeout;
        
        [field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
        [Space]
        
        [Header("Character Values")]
        [Range(0.0f, 0.3f)] public float rotationSmoothTime;
        
        public Move_Values moveValues;
        
        [field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
        public Jump_Values jumpValues;
        
    }
    
    [System.Serializable]
    public struct Move_Values
    {
        public float moveSpeed, speedChangeRate;
    }
    
    [System.Serializable]
    public struct Jump_Values
    {
        public float jumpHeight, jumpTimeout;
    }
    
    [System.Serializable]
    public struct GroundCheck_Values
    {
        [Range(0, 2)] public float groundedRadius;
        [Range(-2, 2)] public float groundedOffset;
    }
    

    [System.Serializable]
    public struct Camera_Values
    {
        [Header("Camera Rotation Value")]
        [Range(-90, 90)] public float topClamp, bottomClamp;
        public float cameraAngleOverride;
        
        [Space]
        [field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
        [Space]
        
        [Header("CineMachine Components")]
        public GameObject followObj;
        
        [Space]
        [field: Line(Thickness = 10, Padding = 10, Color = colorType.Blue)]
        [Space]
        
        [Header("New Input Components")]
        public PlayerInput playerInput;
        
    }

}