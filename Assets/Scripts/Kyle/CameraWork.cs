using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] Photon_PUN.Kyle
 * Descritption
 */
namespace Photon_PUN.Kyle
{
	/*
	 * [Class] CameraWork
	 * Description
	 */
	public class CameraWork : MonoBehaviour
	{
		[SerializeField]
		private float distance = 7.0f;

		[SerializeField]
		private float height = 3.0f;

		[SerializeField]
		private Vector3 centerOffset = Vector3.zero;

		[SerializeField]
		private bool followOnStart = false;

		[SerializeField]
		private float smoothSpeed = 0.125f;

		private Transform cameraTransform;

		private bool isFollowing;

		private Vector3 cameraOffset = Vector3.zero;

		private void Start()
		{
			if (followOnStart) OnStartFollowing();
		}

		private void LateUpdate()
		{
			if (cameraTransform == null && isFollowing) OnStartFollowing();
			if (isFollowing) Follow();
		}

		public void OnStartFollowing()
		{
			cameraTransform = Camera.main.transform;
			isFollowing = true;

			Cut();
		}

		private void Follow()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;

			cameraTransform.position = Vector3.Lerp(cameraTransform.position, transform.position + transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

			cameraTransform.LookAt(transform.position + centerOffset);
		}

		private void Cut()
		{
			cameraOffset.z = -distance;
			cameraOffset.y = height;

			cameraTransform.position = transform.position + transform.TransformVector(cameraOffset);

			cameraTransform.LookAt(transform.position + cameraOffset);
		}
	}
}
