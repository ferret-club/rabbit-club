using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour {

    public void OnMessage () {
        Vector3 position = new Vector3(0, 200, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
            };
        iTween.MoveAdd(gameObject, hash);
    }

    public void OffMessage () {
        Vector3 position = new Vector3(0, -200, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
            };
        iTween.MoveAdd(gameObject, hash);
    }
}
