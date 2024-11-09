using System;
using _Scripts.Controllers_Scripts;
using _Scripts.InputSystem;
using UnityEngine;

namespace _Scripts.System_Scripts
{
    public class Animation_Controller : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ThirdPerson_Controller thirdPersonController;
        [SerializeField] private PlayerInput_Handler inputHandler;
        
        private static readonly int JumpAnimID = Animator.StringToHash("Jump");
        private static readonly int WalkAnimID = Animator.StringToHash("Sprint");
        private static readonly int IdleAnimID = Animator.StringToHash("Idle");
        
        private void Update()
        {
            if (animator is null) return;

            var animationState = DetermineAnimationState();
            animator.Play(animationState);
        }

        public void SetAnimator(Animator newAnimator) => animator = newAnimator;

        private int DetermineAnimationState()
        {
            if (IsJumping())
                return JumpAnimID;

            return IsMoving() ? WalkAnimID : IdleAnimID;
        }

        private bool IsJumping() => !thirdPersonController.GroundCheck();
        private bool IsMoving() => inputHandler.Move.sqrMagnitude > 0;
    }
}
