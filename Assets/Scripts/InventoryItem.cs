using UnityEngine;
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

	void Start() {
		// ホールドアイテムはタップされた時に代入されるので初期値はnullにしておく
		holdObj = null;
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
		this.gameObject.transform.position = new Vector3(holdPosition.x, holdPosition.y, holdPosition.z);
	}

}
