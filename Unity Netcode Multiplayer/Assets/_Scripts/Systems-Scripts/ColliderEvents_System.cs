using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ColliderEvents_System : MonoBehaviour
{
    [Tag, Tooltip("List of tags that can trigger the events. Leave empty to allow any tag.")]
    [SerializeField] private string[] allowedTags = Array.Empty<string>();

    [Tooltip("Layer mask specifying which layers can trigger the events. Leave empty to allow any layer.")]
    [SerializeField] private LayerMask allowedLayers;

    [Header("Events without Collider Data")]
    [SerializeField] private UnityEvent onEnterTrigger, onStayTrigger , onExitTrigger;

    [Header("Events with Collider Data")]
    [SerializeField] public UnityEvent<Collider> onEnterTriggerWithCollider, onStayTriggerWithCollider, onExitTriggerWithCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!Valid_Target(other.gameObject)) return;
        onEnterTrigger.Invoke();
        onEnterTriggerWithCollider.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Valid_Target(other.gameObject)) return;
        onStayTrigger.Invoke();
        onStayTriggerWithCollider.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Valid_Target(other.gameObject)) return;
        onExitTrigger.Invoke();
        onExitTriggerWithCollider.Invoke(other);
    }

    private bool Valid_Target(GameObject obj)
    {
        var isLayerAllowed = allowedLayers == (allowedLayers | (1 << obj.layer)) || allowedLayers == 0;
        var isTagAllowed = allowedTags.Length == 0 || Array.Exists(allowedTags, obj.CompareTag);
        return isLayerAllowed && isTagAllowed;
    }
}
