using UnityEngine;
using System.Collections;

// タップされた時に何かしらの反応を返すオブジェクトにアタッチするスクリプト
public class EventAnswer : MonoBehaviour {

	public int answerId = 0;
	public int rewardId = 0;
	public string rewardMsg = "";
	public string nonMsg = "";
	ItemManager itemManager;
	PickUpItem pickUpItem;
	Message messageArea;
	[SerializeField]
	NetworkShare networkShare;

	void Start() {
		GameObject obj = GameObject.Find("InventoryPanel");
		itemManager = obj.GetComponent<ItemManager>();
		pickUpItem = GameObject.Find("PickUpItemButton").GetComponent<PickUpItem>();
		messageArea = GameObject.Find("RowerObject/Message").GetComponent<Message>();
		networkShare = GameObject.Find("NetworkShare").GetComponent<NetworkShare>();
	}
	
	public void OnClick() {
		// アイテムが選択されていて答えと合っている。もしくは答えが-1は無条件で手に入る。
		if ((itemManager.selectedItem != null && itemManager.selectedItem.id == answerId) || answerId == -1) {
			// 音を鳴らす
			messageArea.PlayClickSe();
			// 自分以外のプレイヤーにアイテム取得情報を表示する（Ticker表示）
			networkShare.callTicker();
			// アイテム取得処理
			itemManager.getItem(rewardId);
			// 取得したアイテムを大きく表示する
			pickUpItem.OnPickUp(rewardId);
			// メッセージエリアにアイテム取得情報を表示する
			messageArea.OnMessage(rewardMsg);
			Destroy(this.gameObject);
		} else {
			// メッセージがある場合は表示する
			if(nonMsg != null || nonMsg != "") {
				messageArea.OnMessage (nonMsg);
			}
		}
	}
}
