using System.Collections;
using _Scripts.InputSystem;
using _Scripts.System_Scripts;
using Artem_Library.Attribute_Scripts;
using Artem_Library.Library_Scripts.Systems_Scripts.Logger_System;
using UnityEngine;

namespace _Scripts.Controllers_Scripts
{
    public class CustomCharacter_Controller : MonoBehaviour
    {

        [Header("Player Model")]
        [SerializeField, Tooltip("Player model GameObject that needs to be replaced.")]
        private GameObject playerModel;

        [SerializeField, ExpandableScript] private PlayerAnimation_Controller playerAnimationController;
        [SerializeField, ExpandableScript] private PlayerInput_Handler inputHandler;
        
        private bool onSwitchCharacter;
        
        private void Start()
        {
            if (Customization_Manager.Instance.ValidateCharacterPrefabs()) 
                StartCoroutine(ReplacePlayerModel());
        }
        
        private void Update()
        {
           
            if (inputHandler.SwitchCharacter && !onSwitchCharacter && Customization_Manager.Instance.ValidateCharacterPrefabs())
                StartCoroutine(ReplacePlayerModel());
            
            onSwitchCharacter = inputHandler.SwitchCharacter;
        }

        private IEnumerator ReplacePlayerModel()
        {
            Customization_Manager.Instance.ReplacePlayerModel(playerModel);
            yield return new WaitForEndOfFrame();
            
            var newAnimator = playerModel.GetComponentInChildren<Animator>();
            if (newAnimator is not null && playerAnimationController is not null)
                playerAnimationController.SetAnimator(newAnimator);
        }
    }
}