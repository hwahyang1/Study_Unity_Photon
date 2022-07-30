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
		public static GameObject LocalPlayerInstance;

		public float health = 1f;

		[SerializeField]
		private GameObject beams;

		[SerializeField]
		private GameObject playerUiPrefab;

		private bool isFiring;

		private void Awake()
		{
			beams.SetActive(false);

			// 인원수에 따라 Scene이 변하지만, 플레이어는 재생성되지 않고 기존의 GameObject를 계속 쓰도록
			if (photonView.IsMine)
			{
				LocalPlayerInstance = gameObject;
			}

			DontDestroyOnLoad(gameObject);
		}

		// 인원수가 변동 될 때 경기장의 크기가 변해서 맵탈이 일어나는 현상 방지
		private void Start()
		{
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
			{
				CalledOnLevelWasLoaded(scene.buildIndex);
			};

			// UI 생성 및 타게팅
			if (playerUiPrefab != null)
			{
				GameObject _uiGo = Instantiate(playerUiPrefab);
				_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
			}
		}

		private void CalledOnLevelWasLoaded(int level)
		{
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
			{
				transform.position = new Vector3(0f, 5f, 0f);
			}

			// 얘는 인원수가 변동 될 때 날아가기 때문에 다시 생성함
			GameObject _uiGo = Instantiate(playerUiPrefab);
			_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
		}
		//

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
