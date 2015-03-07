using UnityEngine;
using System.Collections;

public class ChatBackground : MonoBehaviour {

    GameObject chatBackground;

    void Start () {
        chatBackground = GameObject.Find("ScrollRect 2/Content");
    }

    public void OnButtonMaximize () {
        Vector3 position = new Vector3(0, 200, 0);
        var hash = new Hashtable
            {
                { "position", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveTo(chatBackground, hash);
    }

    public void OnButtonMinimize () {
        Vector3 position = new Vector3(0, -920, 0);
        var hash = new Hashtable
            {
                { "position", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveTo(chatBackground, hash);
    }
}
