using System;
using System.Collections.Generic;
using Artem_Library.Attribute_Scripts;
using Artem_Library.Library_Scripts.ScriptsTemplate_Scripts;
using Artem_Library.Library_Scripts.Systems_Scripts.Logger_System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Controllers_Scripts
{
    public class Customization_Manager : Singleton_MonoBehaviour<Customization_Manager>
    {
        [Header("Logging")]
        [ExpandableScript]
        [SerializeField] private AdvancedLogger_System logger;
        
        [Header("Customization")]
        [field: SerializeField, BoolConverter] public bool CanCustomize{ get; set; }
        [SerializeField] private GameObject[] characterPrefabs;
        
        private readonly HashSet<int> _usedIndices = new();

        private void OnValidate() => characterPrefabs = Resources.LoadAll<GameObject>("Characters-Models");

        private void Awake() => characterPrefabs = Resources.LoadAll<GameObject>("Characters-Models");

        public bool ValidateCharacterPrefabs() => characterPrefabs is { Length: > 0 };

        public void ReplacePlayerModel(GameObject playerModel)
        {
            if (!CanCustomize || characterPrefabs is not { Length: > 0 } || playerModel is null)
            {
                logger?.LogWarning("Cannot replace model. Either customization is disabled, models are unavailable, or player model is null.");
                return;
            }

            var newModelPrefab = GetRandomCharacterModel();
            if (newModelPrefab is null)
            {
                logger?.LogError("Failed to retrieve a new character model.");
                return;
            }

            // Remove existing child models
            foreach (Transform child in playerModel.transform) Destroy(child.gameObject);
            
            var newModel = Instantiate(newModelPrefab, playerModel.transform);
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localRotation = Quaternion.identity;
        }

        private GameObject GetRandomCharacterModel()
        {
            if (_usedIndices.Count >= characterPrefabs.Length)
                ResetUsedIndices();

            int randomIndex;
    
            do
            {
                randomIndex = Random.Range(0, characterPrefabs.Length);
            } while (_usedIndices.Contains(randomIndex));

            _usedIndices.Add(randomIndex);
            return characterPrefabs[randomIndex];
        }

        private void ResetUsedIndices() => _usedIndices.Clear();
    }
}
