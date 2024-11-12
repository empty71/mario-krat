using System;
using _Scripts.InputSystem;
using _Scripts.Values_Scripts;
using Artem_Library.Attribute_Scripts;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PickUp_Controller : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Transform followObject, holdPosition;
    [SerializeField] private LayerMask detectableLayer;
    [SerializeField, Tag] private string detectableTag;

    [Header("Detection Radius and Range")]
    [SerializeField, Range(0, 5)] private float sphereRadius;

    [Header("Hold and Drop Settings")]
    [SerializeField] private HoldDrop_Values holdDropValues;
    [SerializeField] private HoldingHighlight_Values holdingHighlightValues;

    [Header("Input & Events")]
    [SerializeField, ExpandableScript] private PlayerInput_Handler inputHandler;
    public UnityEvent onPickupEvent, onDropEvent;

    private GameObject _heldObject;
    private Rigidbody _heldObjectRb;
    private Collider _heldObjectCollider;

    public bool IsPickedUp { get; set; }
    private void Update() => HandleInput();

    private void HandleInput()
    {
        if (inputHandler?.IsHolding == true)
        {
            AttemptPickup();
        }
        else if (IsPickedUp)
        {
            DropItem();
        }
    }

    private void AttemptPickup()
    {
        if (_heldObject is not null) return;

        var detectedObject = PerformSphereRaycast();
        
        if (detectedObject is not null) PickUpItem(detectedObject);
    }

    private GameObject PerformSphereRaycast()
    {
        var originPosition = followObject?.position ?? transform.position;
        var targetPosition = holdPosition?.position ?? originPosition + followObject!.forward * sphereRadius;
        var direction = (targetPosition - originPosition).normalized;
        var detectionDistance = Vector3.Distance(originPosition, targetPosition);

        if (!Physics.SphereCast(originPosition, sphereRadius, direction, out var hit, detectionDistance, detectableLayer))
            return null;

        if (!string.IsNullOrEmpty(detectableTag) && !hit.collider.CompareTag(detectableTag)) 
            return null;

        if (holdingHighlightValues.enableHighlight == true) 
            HighlightObject(hit.collider.gameObject, holdingHighlightValues.pickupColor);

        return hit.collider.gameObject;
    }


    private void PickUpItem(GameObject item)
    {
        _heldObject = item;
        _heldObject.transform.SetParent(holdPosition);
        _heldObject.transform.localPosition = Vector3.zero;
        _heldObject.transform.localRotation = Quaternion.identity;
        IsPickedUp = true;

        CacheObjectComponents();

        if (_heldObjectRb is not null) _heldObjectRb.isKinematic = true;
        if (_heldObjectCollider is not null) _heldObjectCollider.enabled = false;

        onPickupEvent?.Invoke();
    }

    private void DropItem()
    {
        if (_heldObject is null) return;

        if (_heldObjectRb is not null)
        {
            _heldObjectRb.isKinematic = false;
            var dropDirection = (followObject?.forward + Vector3.up * Mathf.Tan(holdDropValues.dropUpwardAngle * Mathf.Deg2Rad))?.normalized ?? Vector3.up;
            _heldObjectRb.AddForce(dropDirection * holdDropValues.dropForce, ForceMode.Impulse);
        }

        if (_heldObjectCollider is not null) _heldObjectCollider.enabled = true;

        if (holdingHighlightValues.enableHighlight) 
            HighlightObject(_heldObject, holdingHighlightValues.defaultColor);

        ClearHeldObject();
        onDropEvent?.Invoke();
    }

    private void CacheObjectComponents()
    {
        _heldObjectRb = _heldObject?.GetComponent<Rigidbody>();
        _heldObjectCollider = _heldObject?.GetComponent<Collider>();
    }

    private void ClearHeldObject()
    {
        _heldObject?.transform.SetParent(null);
        _heldObject = null;
        _heldObjectRb = null;
        _heldObjectCollider = null;
        IsPickedUp = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var originPosition = followObject?.position ?? transform.position;
        var targetPosition = holdPosition?.position ?? originPosition + (followObject?.forward ?? transform.forward) * sphereRadius;

        Gizmos.DrawWireSphere(targetPosition, sphereRadius);
    }


    private static void HighlightObject(GameObject obj, Color color)
    {
        var renderer = obj?.GetComponent<Renderer>();
        if (renderer is not null) renderer.material.color = color;
    }
}
