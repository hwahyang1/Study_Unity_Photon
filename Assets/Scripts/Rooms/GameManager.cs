using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using Photon_PUN.Kyle;

/*
 * [Namespace] Photon_PUN.Rooms
 * Descritption
 */
namespace Photon_PUN.Rooms
{
	/*
	 * [Class] GameManager
	 * Desciption
	 */
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public GameObject playerPrefab;

		public static GameManager Instance;

		private void Start()
		{
			Instance = this;

			if (PlayerManager.LocalPlayerInstance == null) // IsMine == true인 GameObject가 없다면
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
				PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0); // Instantiate가 다른 클라이언트 간에 연동됨
			}
			else
			{
				Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
		}


		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Debug.LogFormat("OnPlayerEnteredRoom(): {0}", newPlayer.NickName);

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("IsMasterClient: {0}", PhotonNetwork.IsMasterClient);
				LoadArena();
			}
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			Debug.LogFormat("OnPlayerLeftRoom(): {0}", otherPlayer.NickName);

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("IsMasterClient: {0}", PhotonNetwork.IsMasterClient);
				LoadArena();
			}
		}

		private void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("Trying to Load a level but we are not the master client");
				return;
			}
			Debug.LogFormat("Loading Level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
		}

		public override void OnLeftRoom()
		{
			SceneManager.LoadScene("Launcher");
		}

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}
	}
}
