using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	// アイテムの元々の位置を覚えておく
	private Vector3 holdPosition;
	// タップを開始した位置
	private Vector3 startPosition;
	// アイテムをホールドしている最中か
	private bool holdFlg = false;
	// ホールドアイテム
	private GameObject holdObj;
	// アイテム画像への参照
	private Image image;
	// アイテム画像の元の色
	private Color fullAlphaColor;
	// アイテム画像のアルファ値を半分にした色
	private Color halfAlphaColor;

	void Start() {
		// ホールドアイテムはタップされた時に代入されるので初期値はnullにしておく
		holdObj = null;
		// アイテム画像への参照を得ておく。
		image = this.transform.GetComponent<Image>();
		// アイテム画像の元の色を覚えておく。
		fullAlphaColor = image.color;
		// アイテム画像のアルファ値を半分にした色を持っておく
		halfAlphaColor = image.color;
		halfAlphaColor.a = 0.5f;
	}
	// このオブジェクトがドラッグ開始された時に呼ばれる
	public void OnBeginDrag(PointerEventData eventData) {
		// 扉画面の時以外はアイテムドラッグを許さない
		if(DontDestroy.gameStatus != DontDestroy.GameStatus.Door) {
			return;
		}
		holdFlg = true;
		// タップされた時の位置を覚えておく
		holdPosition = this.gameObject.transform.position;
		startPosition = eventData.position;
		// ドラッグが開始されたらアイテム画像の色を半透明にする。
		image.color = halfAlphaColor;
	}
	// このオブジェクトがドラッグ中に呼ばれる
	public void OnDrag(PointerEventData eventData) {
		if(DontDestroy.gameStatus != DontDestroy.GameStatus.Door || !holdFlg) {
			return;
		}
		float moveX = startPosition.x - eventData.position.x;
		float moveY = startPosition.y - eventData.position.y;
		startPosition = new Vector3(eventData.position.x, eventData.position.y, startPosition.z);
		this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x - (moveX * 5), this.gameObject.transform.position.y - (moveY * 5));
	}
	// このオブジェクトがドラッグ後、離された時に呼ばれる
	public void OnEndDrag(PointerEventData eventData) {
		if(DontDestroy.gameStatus != DontDestroy.GameStatus.Door || !holdFlg) {
			return;
		}
		holdFlg = false;
		// ドラッグが終了したらアイテム画像の色を元に戻す。
		image.color = fullAlphaColor;
		this.gameObject.transform.position = new Vector3(holdPosition.x, holdPosition.y, holdPosition.z);
	}

}
