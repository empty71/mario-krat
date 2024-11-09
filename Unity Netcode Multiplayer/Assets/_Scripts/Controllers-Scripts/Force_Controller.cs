using Artem_Library.Attribute_Scripts;
using UnityEngine;

namespace _Scripts.Controllers_Scripts
{
	public class Force_Controller : MonoBehaviour
	{
		[SerializeField, BoolConverter] private bool canPush;
		[SerializeField, Range(0.5f, 5f),] private float strength = 1.1f;
		[SerializeField] private LayerMask pushLayers;

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (canPush)
				Push_Handler(hit);
		}

		private void Push_Handler(ControllerColliderHit hit)
		{
			var body = hit.collider.attachedRigidbody;
			if (body is null || body.isKinematic) return;
		
			var bodyLayerMask = 1 << body.gameObject.layer;
			if ((bodyLayerMask & pushLayers.value) == 0) return;
			if (hit.moveDirection.y < -0.3f) return;
		
			var pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
			body.AddForce(pushDir * strength, ForceMode.Impulse);
		}
	}
}
