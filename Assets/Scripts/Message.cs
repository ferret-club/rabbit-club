using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Message : MonoBehaviour, IPointerClickHandler {

	Text me;
	bool visible = false;

	void Start() {
		me = this.transform.FindChild("Msg").GetComponent<Text>();
	}

	// このPanelがクリックされた時に呼ばれる
	public void OnPointerClick (PointerEventData eventData) {
		OffMessage();
	}

	public void OnMessage(string msg) {
		if (!visible) {
			Vector3 position = new Vector3(0, 207, 0);
			var hash = new Hashtable
			{
				{ "amount", position },
				{ "time", 0.5f },
			};
			iTween.MoveAdd(gameObject, hash);
			visible = true;
		}
		setMessage(msg);
    }

    public void OffMessage() {
		if (visible) {
			Vector3 position = new Vector3(0, -207, 0);
			var hash = new Hashtable
			{
				{ "amount", position },
				{ "time", 0.5f },
			};
			iTween.MoveAdd(gameObject, hash);
			visible = false;
		}
		setMessage("");
    }

	public void setMessage(string msg) {
		me.text = msg;
	}
}
