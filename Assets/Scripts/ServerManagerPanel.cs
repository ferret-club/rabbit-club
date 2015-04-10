using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerManagerPanel : MonoBehaviour {

	[SerializeField]
	NetworkManager networkManager;
	[SerializeField]
	private NetworkShare networkShare;
	// UI周りへの参照
	[SerializeField]
	private InputField playerNameInputField;
	[SerializeField]
	private InputField serverNameInputField;
	[SerializeField]
	private Image avaterImg;
	public Sprite[] avaterSprite;
	[SerializeField]
	private Text statusText;
	float timer;
	int waitingTime;
	GameObject chatPanelObj;
	GameObject chatInputPanelObj;
	RoomManager roomManager;
	DontDestroy.Avater avater = DontDestroy.Avater.Bear;

	void Start() {
		networkManager.OnAfterConnect += afterConnect;
		roomManager = this.transform.FindChild("ServerList").GetComponent<RoomManager>();
		chatPanelObj = GameObject.Find("Canvas/ChatPanel");
		chatPanelObj.SetActive(false);
		chatInputPanelObj = GameObject.Find("Canvas/ChatInputPanel");
		chatInputPanelObj.SetActive(false);
	}
	
	void Update() {
		if(waitingTime > 0) {
			timer += Time.deltaTime;
			if(timer > waitingTime) {
				createHostButtons();
				timer = 0;
				waitingTime = 0;
			}
		}
	}

	// サーバー作成ボタンが押された時の処理
	public void OnPushCreateServer() {
		networkManager.LaunchServer(serverNameInputField.text);
	}

	// ローカルサーバーに接続する.
	public void OnPushConnectToLocalServer() {
		networkManager.ConnectToLocalServer();
	}

	// マスターサーバに登録されているゲームサーバのリストを更新する.
	public void OnPushUpdateHostList() {
		networkManager.UpdateHostList();
		waitingTime = 2;
	}

	void createHostButtons() {
		HostData[] hosts = networkManager.GetHostList();  // サーバ一覧を取得.
		if (hosts.Length > 0) {
			foreach(HostData host in hosts) {
//				GameObject obj = Instantiate(serverButtonPrefab) as GameObject;
// 実体化したプレハブをこのソースがアタッチされているオブジェクトの子要素にする
//				obj.transform.SetParent(this.transform);
//				obj.transform.localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y - 550);
//				ServerButton serverButton = obj.transform.GetComponent<ServerButton>();
//				serverButton.init(host.gameName, host.guid, false, this);
				GameObject node = roomManager.addNode();
				ServerButton serverButton = node.transform.GetComponent<ServerButton>();
				serverButton.init(host.gameName, host.guid, false, networkManager);
			}
			roomManager.refreshShowNode();
			MasterServer.ClearHostList();
		}
	}

	// 接続後の処理を行う
	void afterConnect() {
		// プレイヤー名、アバターなどの情報を取得しておく。
		string playerName = playerNameInputField.text;
		DontDestroy.playerName = playerName;
		DontDestroy.avater = avater;
		// チャットパネルをアクティブ状態にし、入室メッセージを送信する
		chatPanelObj.SetActive(true);
		ChatManager chatManager = chatPanelObj.GetComponent<ChatManager>();
		chatManager.addMessageStr("【"+ playerName +"】" + "さんが入室しました。", (int)DontDestroy.avater);
		chatInputPanelObj.SetActive(true);
		GameObject.Find("Canvas/Lobby/CharaFrame/CharaImage").GetComponent<Image>().sprite = this.avaterSprite[(int)avater];
		GameObject.Find("Canvas/Lobby/Message/Text").GetComponent<Text>().text = "ようこそ！【"+ playerName +"】" + "さん！";
		GameObject.Find ("Canvas/Player").GetComponent<Avater>().setSprite((int)avater);
		networkManager.OnAfterConnect -= afterConnect;
		this.gameObject.SetActive(false);
	}

	// アバター選択ボタンがクリックされた時の処理
	public void OnPushAvater() {
		avater++;
		if(avater > DontDestroy.Avater.Bird) {
			avater = DontDestroy.Avater.Bear;
		}
		avaterImg.sprite = this.avaterSprite[(int)avater];
	}

}
