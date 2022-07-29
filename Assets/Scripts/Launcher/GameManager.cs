using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/*
 * [Namespace] Photon_PUN.Launcher
 * Descritption
 */
namespace Photon_PUN.Launcher
{
	/*
	 * [Class] GameManager
	 * Description
	 */
	public class GameManager : MonoBehaviourPunCallbacks
	{
		[SerializeField]
		private GameObject controlPanel;
		[SerializeField]
		private GameObject progressLabel;

		[SerializeField]
		private byte maxPlayersPerRoom = 4;

		private const string gameVersion = "1";

		private bool isConnecting = false;

		private void Awake()
		{
			// MasterClient에서 PhotonNetwork.LoadLevel()을 호출하여 모든 클라이언트가 동일한 Scene을 로드 할 수 있음
			PhotonNetwork.AutomaticallySyncScene = true;
		}

		private void Start()
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}

		public void Connect()
		{
			isConnecting = true;

			progressLabel.SetActive(true);
			controlPanel.SetActive(false);

			if (PhotonNetwork.IsConnected)
			{
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				PhotonNetwork.GameVersion = gameVersion;
				PhotonNetwork.ConnectUsingSettings();
			}
		}

		public override void OnConnectedToMaster()
		{
			Debug.Log("OnConnectedToMaster() was called");

			// Room을 나오면 Master로 연결하기 때문에 동일하게 호출되어버림
			if (isConnecting)
			{
				PhotonNetwork.JoinRandomRoom();
			}
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogWarning("OnDisconnected() was called: " + cause.ToString());

			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.LogFormat("OnJoinRandomFailed() was called: [{0}] {1}", returnCode, message);

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("OnJoinedRoom() was called");

			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("We load the 'Room for 1'");

				PhotonNetwork.LoadLevel("Room for 1");
			}
		}
	}
}
