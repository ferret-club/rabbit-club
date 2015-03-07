using UnityEngine;
using System.Collections;

public class ChatBackground : MonoBehaviour {

    GameObject chatBackground;

    void Start () {
        chatBackground = GameObject.Find("ScrollRect 2/Content");
    }

    public void OnButtonMaximize () {
        Vector3 position = new Vector3(0, 1130, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveAdd(chatBackground, hash);
    }

    public void OnButtonMinimize () {
        Vector3 position = new Vector3(0, -1130, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveAdd(chatBackground, hash);
    }
}
