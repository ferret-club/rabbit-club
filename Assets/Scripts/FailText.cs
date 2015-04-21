using UnityEngine;
using System.Collections;

public class FailText : MonoBehaviour {

	public void OnCall() {
        Vector3 position = new Vector3(0, -780, 0);
        var hash = new Hashtable
            {
                { "amount", position },
                { "time", 1.5f },
                { "easetype", iTween.EaseType.easeOutBounce }
            };
        iTween.MoveAdd(gameObject, hash);
    }
}
