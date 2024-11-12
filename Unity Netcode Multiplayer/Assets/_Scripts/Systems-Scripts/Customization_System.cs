using System;
using _Scripts.InputSystem;
using Artem_Library.Attribute_Scripts;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;


namespace _Scripts.Systems_Scripts
{
    public class Customization_System : MonoBehaviour
    {
        [SerializeField, ExpandableScript] private CinemachineTargetGroup cinemachineTargetGroup;
        [SerializeField] private GameObject defaultTarget;

        private int _targetAmount;

        /// <summary>
        /// Toggles the CanCustomize property on the PlayerInput_Handler component that is in parent object
        /// </summary>
        public void Activate_Customization(Collider other)
        {
            var parentObject = other.gameObject.transform.parent?.gameObject;
            var parentComponent = parentObject?.GetComponentInChildren<PlayerInput_Handler>();

            if (parentComponent is not null) parentComponent.CanCustomize = !parentComponent.CanCustomize;
        }
        
        public void Add_TargetGroup(Collider other)
        {
            if (_targetAmount > 0 && defaultTarget is not null)
            {
                cinemachineTargetGroup.RemoveMember(defaultTarget.transform);
                _targetAmount--;
            }
            
            cinemachineTargetGroup?.AddMember(other.transform, 1f, 1f);
            _targetAmount++;
        }
        
        public void Remove_TargetGroup(Collider other)
        {
            cinemachineTargetGroup?.RemoveMember(other.transform);
            _targetAmount--;
            
            Add_Default();
        }
        private void Add_Default()
        {
            if (_targetAmount > 0 || defaultTarget is null || cinemachineTargetGroup is null) return;
            cinemachineTargetGroup.AddMember(defaultTarget.transform, 1f, 1f);
            _targetAmount = 1;
        }
    }
}