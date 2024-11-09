using System.Collections.Generic;
using _Scripts.Managers_Scripts;
using Artem_Library.Attribute_Scripts;
using Artem_Library.Library_Scripts.Systems_Scripts.Logger_System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Systems_Scripts
{
    public class Spawn_System : MonoBehaviour
    {
        [Header("Logging")]
        [ExpandableScript]
        [SerializeField] private AdvancedLogger_System logger;
        
        [SerializeField] private List<Transform> spawnPositions;
        
        public void AssignRandomSpawnPosition(PlayerInput playerInput)
        {
            if (spawnPositions.Count == 0)
            {
                logger?.LogWarning("No spawn positions available.");
                return;
            }

            // Select a random spawn position
            var randomSpawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            playerInput.transform.position = randomSpawnPosition.position;
            playerInput.transform.rotation = randomSpawnPosition.rotation;

            logger?.LogInfo($"Player spawned at random position: {randomSpawnPosition.name}");
        }
    }
}