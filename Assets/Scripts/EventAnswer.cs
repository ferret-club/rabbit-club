using UnityEngine;
using System.Collections;

public class EventAnswer : MonoBehaviour {

	public int answerId = 0;
	public int rewardId = 0;
	public string rewardMsg = "";
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
		if (itemManager.selectedItem != null && itemManager.selectedItem.id == answerId) {
			itemManager.getItem(rewardId);
			pickUpItem.OnPickUp(rewardId);
			messageArea.OnMessage(rewardMsg);
//			scrollRectSnap.SendTicker();
		}
	}
}
