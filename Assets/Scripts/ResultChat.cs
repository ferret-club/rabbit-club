using UnityEngine;
using System.Collections;

public class ResultChat : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GameObject chatObject = GameObject.Find ("DontDestroyObject/ScrollRect 2") as GameObject;
		GameObject chatFieldObject = GameObject.Find ("DontDestroyObject/ChatBackground") as GameObject;	
		GameObject canvas = GameObject.Find ("Canvas") as GameObject;	

		chatObject.transform.parent = canvas.transform;
		chatFieldObject.transform.parent = canvas.transform;
	}
}
