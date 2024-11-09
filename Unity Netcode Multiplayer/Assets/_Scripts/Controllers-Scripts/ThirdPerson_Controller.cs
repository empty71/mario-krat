using _Scripts.InputSystem;
using _Scripts.Values_Scripts;
using Artem_Library.Attribute_Scripts;
using UnityEngine;

namespace _Scripts.Controllers_Scripts
{
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class ThirdPerson_Controller : MonoBehaviour
    {
        #region Public Variables
        
        [SerializeField] private PlayerInput_Handler inputHandler;
        
        [SerializeField] private ThirdPersonController_Values thirdPersonControllerValues = new()
        {
            gravity = -15.0f,
            fallTimeout = 0.15f,
            rotationSmoothTime = 0.12f,
            moveValues = new Move_Values
            {
                moveSpeed = 4,
                speedChangeRate = 10
            },
            jumpValues = new Jump_Values
            {
                jumpHeight = 1.2f,
                jumpTimeout = 0.1f
            }
        };

        [Space] [field: Line(Thickness = 10, Padding = 10, Color = colorType.Gray)]
        [Space]

        [SerializeField] private GroundCheck_Values groundCheckValues = new()
        {
            groundedOffset = -0.14f,
            groundedRadius = 0.5f
        };
        #endregion

        #region Private Variables

        private float _inputMagnitude;
        private float _speed, _verticalVelocity, _targetSpeed;
        private float _rotationVelocity, _targetRotation;
        private float _jumpTimeoutDelta, _fallTimeoutDelta;
        private const float TerminalVelocity = 53.0f, SpeedOffset = 0.01f;

        private CharacterController _controller;

        #endregion

        #region Properties
        
        public bool GroundCheck() => _controller.isGrounded;

        #endregion

        private void OnValidate() => _controller ??= GetComponent<CharacterController>();
        private void Awake() => _controller ??= GetComponent<CharacterController>();

        private void Update()
        {
            Movement_Handler();
            Jump_Handler();
            Gravity();
        }
        private void OnDrawGizmos()
        {
            var transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = GroundCheck() ? transparentGreen : transparentRed;
            var position = transform.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y - groundCheckValues.groundedOffset, position.z), groundCheckValues.groundedRadius);
        }

        #region Player Movement Methods

        private void Movement_Handler()
        {
            _targetSpeed = thirdPersonControllerValues.moveValues.moveSpeed;
            _inputMagnitude = inputHandler.Move.magnitude;

            if (inputHandler.Move == Vector2.zero)
            {
                _speed = 0f; // Stop movement when there is no input
            }
            else
            {
                var velocity = _controller.velocity;
                var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

                if (currentHorizontalSpeed < _targetSpeed - SpeedOffset || currentHorizontalSpeed > _targetSpeed + SpeedOffset)
                {
                    _speed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed * _inputMagnitude,
                        Time.deltaTime * thirdPersonControllerValues.moveValues.speedChangeRate);

                    // Round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = _targetSpeed;
                }
            }

            var inputDirection = new Vector3(inputHandler.Move.x, 0.0f, inputHandler.Move.y).normalized;
            if (inputHandler.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    thirdPersonControllerValues.rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }



        #endregion

        #region Jump Methods

        private void Jump_Handler()
        {
            if (!GroundCheck())
            {
                _jumpTimeoutDelta = thirdPersonControllerValues.jumpValues.jumpTimeout;

                if (Fall_TimeOut()) _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _fallTimeoutDelta = thirdPersonControllerValues.fallTimeout;

                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;
                if (inputHandler.Jump && _jumpTimeoutDelta <= 0.0f)
                    _verticalVelocity =
                        Mathf.Sqrt(thirdPersonControllerValues.jumpValues.jumpHeight * -2f *
                                   thirdPersonControllerValues.gravity);

                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
        }

        private bool Fall_TimeOut() => _fallTimeoutDelta >= 0.0f;
        private void Gravity()
        {
            if (_verticalVelocity < TerminalVelocity) _verticalVelocity += thirdPersonControllerValues.gravity * Time.deltaTime;
        }
        
        #endregion
    }
}
