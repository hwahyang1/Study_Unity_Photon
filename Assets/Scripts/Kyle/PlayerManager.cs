using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using Photon_PUN.Rooms;

/*
 * [Namespace] Photon_PUN.Kyle
 * Descritption
 */
namespace Photon_PUN.Kyle
{
	/*
	 * [Class] PlayerManager
	 * Description
	 */
	public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
	{
		public float health = 1f;

		[SerializeField]
		private GameObject beams;

		private bool isFiring;

		private void Awake()
		{
			beams.SetActive(false);
		}

		private void Update()
		{
			if (photonView.IsMine) ProcessInputs();
			if (health <= 0f) GameManager.Instance.LeaveRoom();
			if (isFiring != beams.activeInHierarchy) beams.SetActive(isFiring);
		}

		private void ProcessInputs()
		{
			if (Input.GetButtonDown("Fire1")) //LCtrl || LMouse
			{
				if (!isFiring) isFiring = true;
			}
			if (Input.GetButtonUp("Fire1"))
			{
				if (isFiring) isFiring = false;
			}
		}

		/* 둘중 하나 선택해서 사용 */

		/*private void OnTriggerEnter(Collider other)
		{
			if (!photonView.IsMine) return;
			if (!other.name.Contains("Beam")) return;
			health -= 0.1f;
		}*/

		private void OnTriggerStay(Collider other)
		{
			if (!photonView.IsMine) return;
			if (!other.name.Contains("Beam")) return;
			health -= 0.1f * Time.deltaTime;
		}

		/* 테스트 단계에서 네트워크를 경유하여 발사를 동기화 해줌 */
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(isFiring);
				stream.SendNext(health);
			}
			else
			{
				isFiring = (bool)stream.ReceiveNext();
				health = (float)stream.ReceiveNext();
			}
		}
	}
}
