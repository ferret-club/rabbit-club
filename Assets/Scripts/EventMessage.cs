using UnityEngine;
using System.Collections;

public class EventMessage : MonoBehaviour {

	public string msg = "";
	Message messageArea;

	void Start() {
        messageArea = GameObject.Find("RowerObject/Message").GetComponent<Message>();
	}
	
	public void OnClick() {
		messageArea.OnMessage(msg);
	}
}
