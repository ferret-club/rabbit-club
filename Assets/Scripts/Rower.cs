using UnityEngine;
using System.Collections;

public class Rower : MonoBehaviour {

    public void OnButtonUp () {
        Vector3 position = new Vector3(0, 400, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveAdd(gameObject, hash);
    }

    public void OnButtonDown () {
        Vector3 position = new Vector3(0, -400, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 0.5f },
                { "easetype", iTween.EaseType.easeOutCubic }
            };
        iTween.MoveAdd(gameObject, hash);
    }
}
