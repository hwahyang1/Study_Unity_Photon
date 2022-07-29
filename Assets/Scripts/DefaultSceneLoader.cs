#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/*
 * [Class] DefaultSceneLoader
 * Description
 */
[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void InitScene()
	{
		if (SceneManager.GetActiveScene().name.CompareTo("Launcher") != 0)
		{
			SceneManager.LoadScene("Launcher");
		}
	}
}
#endif