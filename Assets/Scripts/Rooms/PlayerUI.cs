using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon_PUN.Kyle;

/*
 * [Namespace] Photon_PUN.Rooms
 * Descritption
 */
namespace Photon_PUN.Rooms
{
	/*
	 * [Class] PlayerUI
	 * Description
	 */
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField]
		private Text playerNameText;

		[SerializeField]
		private Slider playerHealthSlider;

		private PlayerManager target;

		[SerializeField]
		private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

		private float characterControllerHeight = 0f;
		private Transform targetTransform;
		private Vector3 targetPosition;

		private void Awake()
		{
			transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
		}

		private void Update()
		{
			// 이유에 관계없이 대상 GameObject가 사라지면 같이 날아감
			if (target == null)
			{
				Destroy(gameObject);
				return;
			}

			if (playerHealthSlider != null)
			{
				playerHealthSlider.value = target.health;
			}
		}

		private void LateUpdate()
		{
			if (targetTransform != null)
			{
				targetPosition = targetTransform.position;
				targetPosition.y += characterControllerHeight;
				transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
			}
		}

		public void SetTarget(PlayerManager _target)
		{
			target = _target;
			if (playerNameText != null)
			{
				playerNameText.text = target.photonView.Owner.NickName;
			}

			CharacterController _characterController = _target.GetComponent<CharacterController>();
			if (_characterController != null)
			{
				characterControllerHeight = _characterController.height;
			}
		}
	}
}
