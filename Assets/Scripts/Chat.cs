using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
	[SerializeField]
	RectTransform prefab = null;

	string message = "";
    List<string> messages = new List<string> ();

	WebSocket ws;
	Queue messageQueue;

	Text ChatFieldText;

    public Sprite chatIcon0;
    public Sprite chatIcon1;
    public Sprite chatIcon2;
    public Sprite chatIcon3;
    Sprite[] spriteList;

	// Use this for initialization
	void Start ()
	{
		Connect ();
        spriteList = new Sprite[]{chatIcon0, chatIcon1, chatIcon2, chatIcon3 };
		for (int i = 0; i < 15; i++) {
            messages.Add ("ほげ");
		}
		GameObject ChatFieldGameObject = GameObject.Find ("ChatInputText");
		ChatFieldText = ChatFieldGameObject.GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update ()
	{
		lock (messageQueue.SyncRoot) {
			if (messageQueue.Count > 0) {
				foreach (Transform n in gameObject.transform) {
					GameObject.Destroy (n.gameObject);
				}
                messages.Reverse ();
				for (int i = 0; i < messages.Count; i++) {
					var item = GameObject.Instantiate (prefab) as RectTransform;
					item.SetParent (transform, false);

					var text = item.GetComponentInChildren<Text> ();
					text.text = messages [i];

                    Image image = item.FindChild("ChatIcon").GetComponent<Image> ();
                    image.sprite = spriteList[text.text.Length % 3 + 1];
				}
				Debug.Log (messageQueue.Dequeue ());
				messages.Reverse ();
			}
		}
	}

	/*
    void OnGUI()
    {
        // Input text
        message = GUI.TextArea(new Rect(0, 10, Screen.width * 0.7f, Screen.height / 10), message);
        // Send message button
        if (GUI.Button(new Rect(Screen.width * 0.75f, 10, Screen.width * 0.2f, Screen.height / 10), "Send"))
        {
            SendChatMessage();
        }
        // Show chat messages
        var l = string.Join("\n", messages.ToArray());
        var height = Screen.height * 0.1f * messages.Count;
        GUI.Label(
            new Rect(0, Screen.height * 0.1f + 10, Screen.width * 0.8f, height),
            l);
    }
    */

	void Connect ()
	{
		ws = new WebSocket ("ws://210.140.161.190:9000/chat/ws");

		messageQueue = Queue.Synchronized (new Queue ());

		// called when websocket messages come.
		ws.OnMessage += (sender, e) => {
			Debug.Log (sender);
			string s = e.Data;
			Debug.Log (string.Format ("Receive {0}", s));

			messageQueue.Enqueue (s);
			messages.Add (e.Data);
		};

		ws.Connect ();
		Debug.Log ("Connect to " + ws.Url);
	}

	/*
    void SendChatMessage()
    {
        if (message == "")
        {
            return;
        }
        Debug.Log("Send message " + message);
        ws.Send(message);
        this.message = "";
    }
    */

	public void SendChatMessage ()
	{
		string chat = ChatFieldText.text;
		if (chat != "") {
			ws.Send (chat);
		}
	}
}