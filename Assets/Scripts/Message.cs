using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Message : MonoBehaviour, IPointerClickHandler {

	Text me;
	bool visible = false;

	void Start() {
		var obj = this.transform.FindChild("Msg");
		if(obj != null) {
			me = obj.GetComponent<Text>();
		}
	}

	// このPanelがクリックされた時に呼ばれる
	public void OnPointerClick (PointerEventData eventData) {
		OffMessageTween();
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
			this.transform.position = new Vector3(0, -47.0f, 0);
			visible = false;
		}
		setMessage("");
	}

	public void OffMessageTween() {
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

	private void setMessage(string msg) {
		me.text = msg;
	}
}
