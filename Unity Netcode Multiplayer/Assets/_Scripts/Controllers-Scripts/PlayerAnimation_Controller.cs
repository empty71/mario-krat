using System;
using _Scripts.Controllers_Scripts;
using _Scripts.InputSystem;
using Artem_Library.Attribute_Scripts;
using UnityEngine;

namespace _Scripts.System_Scripts
{
    public class PlayerAnimation_Controller : MonoBehaviour
    {
        [SerializeField, ExpandableScript] private Animator animator;
        [SerializeField, ExpandableScript] private ThirdPerson_Controller thirdPersonController;
        [SerializeField, ExpandableScript] private PlayerInput_Handler inputHandler;
        [SerializeField, ExpandableScript] private PickUp_Controller pickUpController;

        private static readonly int JumpAnimID = Animator.StringToHash("Jump");
        private static readonly int WalkAnimID = Animator.StringToHash("Sprint");
        private static readonly int IdleAnimID = Animator.StringToHash("Idle");
        private static readonly int IdleHoldAnimID = Animator.StringToHash("holding-both");
        private static readonly int RunHoldAnimID = Animator.StringToHash("RunHolding");

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

            if (pickUpController.IsPickedUp)
            {
                return IsMoving() ? RunHoldAnimID : IdleHoldAnimID;
            }

            return IsMoving() ? WalkAnimID : IdleAnimID;
        }

        private bool IsJumping() => !thirdPersonController.GroundCheck();
        private bool IsMoving() => inputHandler.Move.sqrMagnitude > 0;
    }
}