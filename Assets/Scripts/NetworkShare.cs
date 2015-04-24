using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkShare : MonoBehaviour {

	[SerializeField]
	NetworkManager networkManager;
	// ユーザー数合計
	[HideInInspector]
	public int userCnt;
	private GameObject scrollContent;
	public struct UserAvater {
		public int playerNum;
		public int avater;
		public float roomNumber;
	}
	// 位置同期用
	public Dictionary<int, object> userSync = new Dictionary<int, object>();
	private Dictionary<int, GameObject> avaters = new Dictionary<int, GameObject>();
	public Ticker ticker;
	public ItemManager itemManager;

	void Start() {
		scrollContent = GameObject.Find("Canvas/ScrollRect/Content").gameObject;
		// 他プレイヤーのアバター表示用画像を一旦読み込んで非表示にしておく（初めから消しておくとfindできないため）
		avaters[2] = GameObject.Find("Canvas/ScrollRect/Content/Background/Avater2").gameObject;
		avaters[3] = GameObject.Find("Canvas/ScrollRect/Content/Background/Avater3").gameObject;
		avaters[4] = GameObject.Find("Canvas/ScrollRect/Content/Background/Avater4").gameObject;
		foreach (var ava in avaters.Values) {
			ava.SetActive(false);
		}
	}

	// 特定のユーザーだけにユーザー数を設定する
	public void setUserCntForYou(NetworkPlayer player, int cnt) {
		if (networkManager != null && (
			networkManager.GetStatus () == NetworkManager.Status.ConnectedToServer
			|| networkManager.GetStatus () == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("setUserCnt", player, new object[]{cnt});
		}
	}

	[RPC]
	public void setUserCnt(int cnt) {
		// 渡されたプレイヤー番号のアバターをアクティブにする
		userCnt = cnt;
		DontDestroy.playerNum = cnt;
		updatePosition(DontDestroy.playerNum, (int)DontDestroy.avater, 2);
	}

	// 有効になっているアバターをアクティブにする
	public void setAvaterActive() {
		foreach(UserAvater user in userSync.Values) {
			// 1は自分自身なので同期表示用アバターに関係ない
			if (user.playerNum == 1)
				continue;
			if (avaters.ContainsKey(user.playerNum)) {
				GameObject ava = avaters [user.playerNum];
				// 無効なら有効にして画像を設定する
				if (!ava.activeInHierarchy) {
					ava.SetActive (true);
					ava.GetComponent<Avater>().setSprite(user.avater);
				}
			}
		}
	}

	// 現在（見ている）位置を自分を含めた全員に送信する
	public void updatePosition(int playerNum, int avater, int roomNumber) {
		if (networkManager != null && (
			networkManager.GetStatus () == NetworkManager.Status.ConnectedToServer
			|| networkManager.GetStatus () == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("setAvaterPosition", RPCMode.All, new object[]{playerNum, avater, roomNumber});
		}
	}

	// 現在（見ている）位置を自分以外の全員に送信する
	public void updatePositionOthers(int playerNum, int avater, int roomNumber) {
		if (networkManager != null && (
			networkManager.GetStatus () == NetworkManager.Status.ConnectedToServer
			|| networkManager.GetStatus () == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("setAvaterPosition", RPCMode.Others, new object[]{playerNum, avater, roomNumber});
		}
	}

	[RPC]
	public void setAvaterPosition(int playerNum, int avater, int roomNumber) {
		// プレイヤー番号、アバター、位置
		// 基準位置x460,y90
		// -520,-400,-280
		UserAvater userAvater;
		userAvater.playerNum = playerNum;
		userAvater.avater = avater;
		userAvater.roomNumber = roomNumber;
		if (DontDestroy.playerNum == 1 && playerNum == 1) {
//			Debug.Log("setAvaterPosition() sever me playerNum:" + playerNum + " DontDestroy.playerNum:" + DontDestroy.playerNum + " avater:" + avater);
			// 自分自身がサーバーの場合はアバターをいじらない
			userSync[1] = userAvater;
		} else if(DontDestroy.playerNum != 1 && DontDestroy.playerNum == playerNum) {
//			Debug.Log("setAvaterPosition()1 playerNum:" + playerNum + " DontDestroy.playerNum:" + DontDestroy.playerNum + " avater:" + avater);
			// サーバー以外かつ自分自身のナンバーなら1に自分自身のアバターを入れる
			userAvater.playerNum = 1;
			userSync[1] = userAvater;
		} else if(DontDestroy.playerNum != 1 && playerNum == 1) {
//			Debug.Log("setAvaterPosition()2 playerNum:" + playerNum + " DontDestroy.playerNum:" + DontDestroy.playerNum + " avater:" + avater);
			// サーバー以外かつ1なら自分自身のナンバーに1(サーバー)のアバターを入れる
			userAvater.playerNum = DontDestroy.playerNum;
			userSync[DontDestroy.playerNum] = userAvater;
			if (avaters.ContainsKey (userAvater.playerNum)) {
				GameObject ava = avaters[userAvater.playerNum];
				if(ava.activeInHierarchy) {
					ava.GetComponent<Avater>().moveAvater(roomNumber);
				}
			}
		} else {
//			Debug.Log("setAvaterPosition()else playerNum:" + playerNum + " DontDestroy.playerNum:" + DontDestroy.playerNum + " avater:" + avater);
			userSync[playerNum] = userAvater;
			if (avaters.ContainsKey (userAvater.playerNum)) {
				GameObject ava = avaters[userAvater.playerNum];
				if(ava.activeInHierarchy) {
					ava.GetComponent<Avater>().moveAvater(roomNumber);
				}
			}
		}
	}

	public void callTicker() {
		// ネットワーク接続が確立されていれば接続先にもメッセージを送る
		if (networkManager != null && 
			(networkManager.GetStatus() == NetworkManager.Status.ConnectedToServer
				|| networkManager.GetStatus() == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("OnTicker", RPCMode.Others, new object[] {DontDestroy.playerName});
		}
	}

	[RPC]
	public void OnTicker(string characterName) {
		// TickerにNetworkViewを付けるとTickerオブジェクト自体が同期され自分のTickerも動いてしまうため、NetworkShare経由で呼び出す
		ticker.OnTicker(characterName);
	}

	public void getItem(int rewardId, string itemName) {
		// ネットワーク接続が確立されていれば接続先にもメッセージを送る
		if (networkManager != null && 
			(networkManager.GetStatus() == NetworkManager.Status.ConnectedToServer
				|| networkManager.GetStatus() == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("OnGetItem", RPCMode.Others, new object[] {rewardId, itemName});
		}
	}

	[RPC]
	public void OnGetItem(int rewardId, string itemName) {
		itemManager.getItem(rewardId);
		GameObject itemObj = GameObject.Find("Canvas/ScrollRect/Content/Background/" + itemName) as GameObject;
		if(itemObj != null) {
			// アイテムを画面から削除する
			Destroy(itemObj);
		}
	}

	// ゲーム状態を送信する
	public void setGameStatus(DontDestroy.GameStatus gameStatus) {
		if (networkManager != null && 
			(networkManager.GetStatus() == NetworkManager.Status.ConnectedToServer
				|| networkManager.GetStatus() == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("OnSetGameStatus", RPCMode.Others, new object[] {(int)gameStatus});
		}
	}

	[RPC]
	public void OnSetGameStatus(int gameStatus) {
		DontDestroy.gameStatus = (DontDestroy.GameStatus) gameStatus;
	}

}
