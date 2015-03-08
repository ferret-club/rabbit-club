using UnityEngine;
using System.Collections;

public class EventAnswer : MonoBehaviour {

	public int answerId = 0;
	public int rewardId = 0;
	public string rewardMsg = "";
	public string nonMsg = "";
	ItemManager itemManager;
	PickUpItem pickUpItem;
	Message messageArea;
	ScrollRectSnap scrollRectSnap;

	void Start () {
		GameObject obj = GameObject.Find("InventoryPanel");
		itemManager = obj.GetComponent<ItemManager>();
		pickUpItem = GameObject.Find("PickUpItemButton").GetComponent<PickUpItem>();
		messageArea = GameObject.Find("Message").GetComponent<Message>();
		scrollRectSnap = GameObject.Find("Canvas/ScrollRect").GetComponent<ScrollRectSnap>();
	}
	
	public void OnClick() {
		// アイテムが選択されていて答えと合っている。もしくは答えが-1は無条件で手に入る。
		if ((itemManager.selectedItem != null && itemManager.selectedItem.id == answerId) || answerId == -1) {
			itemManager.getItem (rewardId);
			pickUpItem.OnPickUp (rewardId);
			messageArea.OnMessage (rewardMsg);
//			scrollRectSnap.SendTicker();
			Destroy (this.gameObject);
		} else {
			// メッセージがある場合は表示する
			if(nonMsg != null || nonMsg != "") {
				messageArea.OnMessage (nonMsg);
			}
		}
	}
}
