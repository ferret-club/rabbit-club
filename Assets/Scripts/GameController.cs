using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int targetFrameRate = 60;

    GameObject buttonGroup;

	void Awake ()
	{
		Application.targetFrameRate = targetFrameRate;
	}

	void Start ()
	{
        buttonGroup = GameObject.Find ("Door/ButtonGroup");
        buttonGroup.SetActive(false);
	}

	void Update ()
	{
    
	}

	public void goResultScene ()
	{
		GameObject chatObject = GameObject.Find ("Canvas/RowerObject/ScrollRect 2") as GameObject;
		GameObject chatFieldObject = GameObject.Find ("Canvas/ChatBackground") as GameObject;
		GameObject DontDestroyObject = GameObject.Find ("DontDestroyObject") as GameObject;
		Debug.Log ("name:" + chatObject.name);
		chatObject.transform.parent = DontDestroyObject.transform;
		chatFieldObject.transform.parent = DontDestroyObject.transform;
        Invoke("LoadResult", 3);
		
	}

    void LoadResult ()
    {
        Application.LoadLevel ("ResultScene");
    }
}
