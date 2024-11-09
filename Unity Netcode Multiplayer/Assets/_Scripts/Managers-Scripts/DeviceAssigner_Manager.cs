using System;
using Artem_Library.Attribute_Scripts;
using Artem_Library.Library_Scripts.Systems_Scripts.Logger_System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.Managers_Scripts
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class DeviceAssigner_Manager : MonoBehaviour
    {
        [Header("Logging")]
        [ExpandableScript]
        [SerializeField] private AdvancedLogger_System logger;

        [Header("Player Input Management")] 
        [ExpandableScript]
        [SerializeField] private PlayerInputManager playerInputManager;
        
        [Header("Cinemachine Target Group")]
        [ExpandableScript]
        [SerializeField] private CinemachineTargetGroup targetGroup;
        
        [Header("Events")]
        [SerializeField] private UnityEvent onJoinEvent;

        private void OnValidate() => playerInputManager ??= GetComponent<PlayerInputManager>();
        private void Awake() => playerInputManager ??= GetComponent<PlayerInputManager>();
        private void OnEnable() => playerInputManager.onPlayerJoined += OnPlayer_Joined;
        private void OnDisable() => playerInputManager.onPlayerJoined -= OnPlayer_Joined;
        
        /// <summary>
        /// Pair the player with the gamepad that initiated the join
        /// </summary>
        /// <param name="playerInput"> Spawned Prefab </param>
        private void OnPlayer_Joined(PlayerInput playerInput)
        {
            if (playerInput.devices.Count <= 0 || playerInput.devices[0] is not Gamepad gamepad)
                logger?.LogWarning($"No gamepad found for Player {playerInput.playerIndex}.");
            else
            {
                playerInput.SwitchCurrentControlScheme(gamepad);
                logger?.LogInfo($"Assigned {gamepad.displayName} to Player {playerInput.playerIndex}");
            }

            onJoinEvent.Invoke();
            AddPlayerToTargetGroup(playerInput);
        }

        /// <summary>
        /// Adds the "Thirdperson Controller" object of the player to the Cinemachine Target Group.
        /// </summary>
        private void AddPlayerToTargetGroup(PlayerInput playerInput)
        {
            var parentTransform = GetPlayerParentTransform(playerInput);

            if (parentTransform is null) return;
            
            var playerTransform = parentTransform.Find("ThirdPerson - Controller");

            if (playerTransform is null)
            {
                logger?.LogWarning(
                    $"Could not find 'Thirdperson Controller' on the parent of Player {playerInput.playerIndex}.");
            }
            else
            {
                targetGroup.AddMember(playerTransform, 1.0f, 2.0f);
                logger?.LogInfo($"Added {playerTransform.name} to the Cinemachine Target Group.");
            }
        }

        /// <summary>
        /// Finds and returns the parent transform of the player's input.
        /// </summary>
        private Transform GetPlayerParentTransform(PlayerInput playerInput)
        {
            var parentTransform = playerInput.transform.parent;
            
            if (parentTransform is null) 
                logger?.LogWarning($"Player {playerInput.playerIndex} does not have a parent object.");

            return parentTransform;
        }
    }
}