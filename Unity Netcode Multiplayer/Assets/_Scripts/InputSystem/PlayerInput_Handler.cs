using Artem_Library.Attribute_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.InputSystem
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerInput_Handler : MonoBehaviour
	{ 
		#region Customization Input

		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Gray)]
		[field: Header("Input Customization")]
		[field: SerializeField, BoolConverter] public bool CanCustomize{ get; set; }
		[field: SerializeField, BoolConverter] public bool SwitchCharacter{ get; set; }

		#endregion
		
		#region Movement Input
		
		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Gray)]
		[field: Header("Movement Values")]
		[field: SerializeField] public Vector2 Move { get; set; }
		[field: SerializeField, BoolConverter] public bool AnalogMovement{ get; set; }
		[field: SerializeField, BoolConverter] public bool CanMove{ get; set; }
		
		#endregion
		
		#region Jump Input

		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Gray)]
		[field: Header("Input Dodge")]
		[field: SerializeField, BoolConverter] public bool Jump{ get; set; }
		
		#endregion
		
		#region Interact Input

		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Gray)]
		[field: Header("Input Interact")]
		
		[field: SerializeField, BoolConverter] public bool IsHolding{ get; set; }

		#endregion

		#region Camera Input

		[field: Space]
		[field: Line(Thickness = 10, Padding = 10, Color = colorType.Black)]
		
		[field: Header("Camera Values")]
		[field: SerializeField] public Vector2 Look{ get; set; }
		[field: SerializeField, BoolConverter] public bool CursorLocked{ get; set; }
		

		#endregion
		
		#region ReadInput - New Input
		public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>(), CanMove);
		//public void OnLook(InputValue value) => LookInput(value.Get<Vector2>());
		public void OnSwitchCharacter(InputValue value) => SwitchCharacterInput(value.isPressed, CanCustomize);
		public void OnJump(InputValue value) => JumpInput(value.isPressed);
		public void OnPickUp(InputValue value) => HoldingInput(value.isPressed);
		
		#endregion
		
		#region Convert - New Input

		#region Movement Input

		private void MoveInput(Vector2 newMoveDirection, bool canMove) => Move = canMove ? newMoveDirection : Move;
		private void JumpInput(bool newJumpState) => Jump = newJumpState;
		private void HoldingInput(bool newHoldingState) => IsHolding = newHoldingState;
		private void SwitchCharacterInput(bool newSwitchCharacters,bool canCustomize) => SwitchCharacter = canCustomize ? newSwitchCharacters : SwitchCharacter;

		#endregion

		#region Camera Input
		//private void LookInput(Vector2 newLookDirection) => Look = newLookDirection;
		private void OnApplicationFocus(bool hasFocus) => SetCursorState(CursorLocked);
		#endregion
		
		#endregion
		private static void SetCursorState(bool newState) => Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		
	}
}
	

