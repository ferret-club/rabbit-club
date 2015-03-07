using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// 

    /// The message which store current input text.
    /// 

    string message = "";
    /// 

    /// The list of chat message.
    /// 

    List<string> messages = new List<string>();

    /// 

    /// Raises the GU event.
    ///
    /// 

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

    WebSocket ws;

    void Connect()
    {
        ws = new WebSocket("ws://210.140.161.190:9000/chat/ws");

        // called when websocket messages come.
        ws.OnMessage += (sender, e) =>
        {
            string s = e.Data;
            Debug.Log(string.Format("Receive {0}", s));
            messages.Add("> " + e.Data);
            if (messages.Count > 10)
            {
                messages.RemoveAt(0);
            }
        };

        ws.Connect();
        Debug.Log("Connect to " + ws.Url);
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

    public void SendChatMessage()
    {
        GameObject ChatFieldGameObject = GameObject.Find("ChatText") as GameObject;
        Text ChatFieldText = ChatFieldGameObject.GetComponent<Text>();
        string chat = ChatFieldText.text;
        ws.Send(chat);
    }
}