using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// ゲームタイプ名.
	const string GameTypeName = "rabbit-club"; // あなたのすきなタイトル名に書き換えてください.

	// ローカルIPアドレスとポート.
	const string LocalServerIP = "127.0.0.1"; // 開発用.
	const int ServerPort = 25000;

	bool useNat = false; // natパンチスルーを使用するか.

	[SerializeField]
	private Text statusText;

	// 状態.
	public enum Status {
		NoError,				// エラーなし.

		LaunchingServer,		// サーバー起動中.
		ServerLaunched,			// サーバーが起動に成功.
		LaunchServerFailed,		// サーバーの起動に失敗.

		ConnectingToServer,		// サーバーに接続中.
		ConnectedToServer,		// サーバーに接続に成功.
		ConnectToServerFailed,	// サーバーへの接続に失敗.

		DisconnectedFromServer, // サーバーから切断された.

		ServerNameIsEmpty,      // サーバー名が入力されていない
		SearchingHosts,         // ホスト検索中
	};
	public Status _status = Status.NoError;
	public Status status {
		get{
			return _status;
		}
		private set {
			_status = value;
			statusText.text = _status.ToString();
		}
	}
	[SerializeField]
	private NetworkShare networkShare;
	// 接続後の処理を行う
	public delegate void AfterConnect();
	public event AfterConnect OnAfterConnect;

	// サーバーを起動する.
	public void LaunchServer(string gameServerName) {
		if(gameServerName == null || gameServerName == "") {
			// サーバー名が取得できない場合はステータスにエラーを渡して終了
			status = Status.ServerNameIsEmpty;
		} else {
			status = Status.LaunchingServer;
			StartCoroutine(LaunchServerCoroutine(gameServerName));
		}
	}

	// サーバーを起動するコルーチン.
	IEnumerator LaunchServerCoroutine(string roomName) {
		yield return StartCoroutine(CheckNat());

		// サーバーを起動する.
		NetworkConnectionError error = Network.InitializeServer(4, ServerPort, useNat);
		if(error != NetworkConnectionError.NoError) {
			Debug.Log("Can't Launch Server");
			status = Status.LaunchServerFailed;
		} else if(roomName == null || roomName == "") {
			Debug.Log("ServerName Empty.");
			status = Status.LaunchServerFailed;
		} else {
			// マスターサーバーにゲームサーバーを登録する.
			MasterServer.RegisterHost(GameTypeName, roomName);
		}
	}

	IEnumerator CheckNat() {
		bool doneTesting = false; // 接続テストが終わったか.
		bool probingPublicIP = false;
		float timer = 0;
		useNat = false;

		// 接続テストをしてNATパンチスルーが必要かしらべる.
		while (!doneTesting) {
			ConnectionTesterStatus connectionTestResult = Network.TestConnection();
			switch (connectionTestResult) {
			case ConnectionTesterStatus.Error:
				// 問題が発生した.
				doneTesting = true;
				break;

			case ConnectionTesterStatus.Undetermined: 
				// 調査中.
				doneTesting = false;
				break;

			case ConnectionTesterStatus.PublicIPIsConnectable:
				// パブリックIPアドレスを持っているのでNATパンチスルーは使わなくていい.
				useNat = false;
				doneTesting = true;
				break;


			case ConnectionTesterStatus.PublicIPPortBlocked:
				// パブリックIPアドレスを持っているようだがポートがブロックされていて接続できない.
				useNat = false;
				if (!probingPublicIP) {
					connectionTestResult = Network.TestConnectionNAT();
					probingPublicIP = true;
					timer = Time.time + 10;
				}

				else if (Time.time > timer) {
					probingPublicIP = false;        // reset
					useNat = true;
					doneTesting = true;
				}
				break;
			case ConnectionTesterStatus.PublicIPNoServerStarted:
				// パブリックIPアドレスを持っているがサーバーが起動していない.
				break;

			case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
				// NATパンチスルーに制限がある.
				// サーバーに接続できないクライアントがあるかもしれない.
				useNat = true;
				doneTesting = true;
				break;
			case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
			case ConnectionTesterStatus.NATpunchthroughFullCone:
				// NATパンチスルーによりサーバーとクライアンは問題なく接続できる.
				useNat = true;
				doneTesting = true;
				break;

			default: 
				Debug.Log("Error in test routine, got " + connectionTestResult);
				break;
			}
			yield return null;
		}
	}

	// サーバーに接続する.
	public void ConnectToServer(string serverGuid, bool connectLocalServer) {
		status = Status.ConnectingToServer;
		if (connectLocalServer)
			Network.Connect(LocalServerIP, ServerPort);
		else 
			Network.Connect(serverGuid);
	}

	// ローカルサーバーに接続する.
	public void ConnectToLocalServer() {
		ConnectToServer("", true);
	}

	// サーバーが起動した.
	void OnServerInitialized() {
		status = Status.ServerLaunched;
		// ホスト自身のユーザー数を増やす
		networkShare.userCnt++;
		// ホスト自身はplayerNum=1
		DontDestroy.playerNum = 1;
		// 接続後の処理を行う
		if(OnAfterConnect != null) {
			OnAfterConnect();
		}
		// サーバー自身をユーザーとして登録
		networkShare.updatePosition(DontDestroy.playerNum, (int)DontDestroy.avater, 2);
		networkShare.setAvaterActive();
	}

	// サーバーに接続した.
	void OnConnectedToServer() {
		status = Status.ConnectedToServer;
		// 接続後の処理を行う
		if(OnAfterConnect != null) {
			OnAfterConnect();
		}
	}

	// サーバーへの接続に失敗した.
	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("FailedToConnect: " + error.ToString());
		status = Status.ConnectToServerFailed;
	}

	// プレイヤーが接続した
	void OnPlayerConnected(NetworkPlayer player) {
		// ユーザー数を増やす
		networkShare.userCnt++;
		networkShare.setUserCntForYou(player, networkShare.userCnt);
		// ユーザーが増えたらサーバー側のプレイヤー情報を送りアバターアクティブを見直す
		networkShare.updatePosition(DontDestroy.playerNum, (int)DontDestroy.avater, 2);
		networkShare.setAvaterActive();
	}

	// プレイヤーが切断した.（サーバーが動作しているコンピュータで呼び出される）.
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		networkShare.userCnt--;
	}

	// サーバーから切断された.
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("DisconnectedFromServer: " + info.ToString());
		status = Status.DisconnectedFromServer;
		Application.LoadLevel(0);
	}

	// ステータスを得る.
	public Status GetStatus() {
		return status;
	}

	void OnDestroy() {
		if (Network.isServer) {
			MasterServer.UnregisterHost();
			Network.Disconnect();
		}
	}

	//-------------------- ロビー関連. --------------------
	// マスターサーバに登録されているゲームサーバのリストを更新する.
	public void UpdateHostList() {
		status = Status.SearchingHosts;
		MasterServer.RequestHostList(GameTypeName);
	}

	// マスターサーバに登録されているゲームサーバのリストを取得する.
	public HostData[] GetHostList() {
		return MasterServer.PollHostList();
	}

	// マスターサーバとNATファシリテータのIPアドレスを設定する.
	void SetMasterServerAndNatFacilitatorIP(string masterServerAddress, string facilitatorAddress) {
		MasterServer.ipAddress = masterServerAddress;
		Network.natFacilitatorIP = facilitatorAddress;
	}

	// マスターサーバーへの登録を削除する.
	public void UnregisterHost() {
		MasterServer.UnregisterHost();
	}
}
