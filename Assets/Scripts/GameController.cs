using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public int targetFrameRate = 60;

    void Awake () {
        Application.targetFrameRate = targetFrameRate;
    }

    void Start () {
            
    }
    
    void Update () {
    
    }

    public void goResultScene() {
//		GameObject chatObject = GameObject.Find("Canvas/ScrollRect 2") as GameObject;
//		Debug.Log ("name:" + chatObject.name);
//		DontDestroyOnLoad(chatObject);
		Application.LoadLevel("ResultScene");
    }
}
