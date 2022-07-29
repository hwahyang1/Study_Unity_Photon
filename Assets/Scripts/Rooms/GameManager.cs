using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

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
		public static GameManager Instance;

		private void Start()
		{
			Instance = this;
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
