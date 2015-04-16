using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Message : MonoBehaviour, IPointerClickHandler {

	Text me;
	bool visible = false;
	public AudioClip audioClip;
	private AudioSource audioSource;

	void Start() {
		var obj = this.transform.FindChild("Msg");
		if(obj != null) {
			me = obj.GetComponent<Text>();
		}
		audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClip;
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
			Invoke("OffMessageTween", 5);
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
        if(me == null) {
            var obj = this.transform.FindChild("Msg");
            me = obj.GetComponent<Text>();
        }
		me.text = msg;
	}

	public void PlayClickSe() {
		audioSource.Play();
	}
}
