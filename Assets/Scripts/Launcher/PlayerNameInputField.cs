using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

/*
 * [Namespace] Photon_PUN.Launcher
 * Descritption
 */
namespace Photon_PUN.Launcher
{
	/*
	 * [Class] PlayerNameInputField
	 * Description
	 */
	public class PlayerNameInputField : MonoBehaviour
	{
		private const string playerNameKey = "PlayerName";

		private void Start()
		{
			string defaultName = string.Empty;
			InputField _inputField = GetComponent<InputField>();
			if (_inputField != null)
			{
				if (PlayerPrefs.HasKey(playerNameKey))
				{
					defaultName = PlayerPrefs.GetString(playerNameKey);
					_inputField.text = defaultName;
				}
			}

			PhotonNetwork.NickName = defaultName;
		}

		public void SetPlayerName(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Debug.LogError("Player Name is null or empty");
				return;
			}
			PhotonNetwork.NickName = value;

			PlayerPrefs.SetString(playerNameKey, value);
		}
	}
}
