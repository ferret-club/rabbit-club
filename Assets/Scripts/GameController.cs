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
}
