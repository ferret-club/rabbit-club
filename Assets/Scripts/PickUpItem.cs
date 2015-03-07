using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUpItem : MonoBehaviour {

	// InspectorでアイテムL画像を設定して下さい
	public Sprite[] itemImages;
	private Image me;

	void Start() {
		me = this.transform.FindChild("ItemImg").GetComponent<Image>();
	}
	
	void Update() {
	
	}

	// _idに表示したいアイテムL画像のIDを私て下さい
	public void OnPickUp(int _id) {
		this.GetComponent<Image>().enabled = true;
		me.enabled = true;
		this.transform.FindChild("ItemImg").GetComponent<Image>().sprite = itemImages[_id];
	}
}
