using UnityEngine;
using System.Collections;

public class Ticker : MonoBehaviour {

	public void OnTicker () {
        Vector3 position = new Vector3(0, -96, 0);
        var hash = new Hashtable
        {
            { "amount", position },
            { "time", 0.5f },
        };
        iTween.MoveAdd(gameObject, hash);
	}
	
    public void OffTicker () {
        Vector3 position = new Vector3(0, 96, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
            };
        iTween.MoveAdd(gameObject, hash);
	}
}
