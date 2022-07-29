using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;

/*
 * [Namespace] Photon_PUN.Kyle
 * Descritption
 */
namespace Photon_PUN.Kyle
{
	/*
	 * [Class] PlayerAnimatorManager
	 * Description
	 */
	public class PlayerAnimatorManager : MonoBehaviourPun
	{
		[SerializeField]
		private float directionDampTime = 0.25f;

		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			CameraWork _cameraWork = GetComponent<CameraWork>();
			if (_cameraWork != null)
			{
				if (photonView.IsMine)
				{
					_cameraWork.OnStartFollowing();
				}
			}
		}

		private void Update()
		{
			if (!photonView.IsMine && PhotonNetwork.IsConnected/*개발 단계에서 해당 Prefab을 네트워크 연결 없이 테스트하기 위함*/) return;

			if (animator == null) return;

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButtonDown("Fire2")) // LAlt
				{
					animator.SetTrigger("Jump");
				}
			}

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			if (v < 0) v = 0;

			animator.SetFloat("Speed", h * h + v * v);
			animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
		}
	}
}
