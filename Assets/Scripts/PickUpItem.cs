using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUpItem : MonoBehaviour {

	// InspectorでアイテムL画像を設定して下さい
	public Sprite[] itemImages;
	private Image me;
	private Image itemImg;

	void Start() {
		me = this.GetComponent<Image>();
		itemImg = this.transform.FindChild("ItemImg").GetComponent<Image>();
	}
	
	// _idに表示したいアイテムL画像のIDを私て下さい
	public void OnPickUp(int _id) {
		me.enabled = true;
		itemImg.enabled = true;
		itemImg.sprite = itemImages[_id];
	}

	// クリックされたら自分自身を非表示にする
	public void OnClick() {
		itemImg.sprite = null;
		itemImg.enabled = false;
		me.enabled = false;
	}

}
