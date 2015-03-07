using UnityEngine;
using System.Collections;

public class ResultExit : MonoBehaviour
{

	public void goLobbyScene ()
	{
		GameObject DontDestroyObject = GameObject.Find ("DontDestroyObject") as GameObject;
		Object.Destroy (DontDestroyObject);
		Application.LoadLevel ("main");
	}
}
