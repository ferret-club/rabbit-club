using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerButton : MonoBehaviour {

	Text me;
	NetworkManager networkManager;
	string serverGuid;
	bool connectLocalServer;

	void Start () {
		GameObject obj = this.transform.FindChild("Text").gameObject;
		me = obj.GetComponent<Text>();
	}

	public void init(string _hostName, string _serverGuid, bool _connectLocalServer, NetworkManager _networkManager) {
		if(me == null) {
			// ノード（ボタン）の文字を変更する
			GameObject obj = this.transform.FindChild("Text").gameObject;
			me = obj.GetComponent<Text>();
		}
		me.text = _hostName;
		networkManager = _networkManager;
		serverGuid = _serverGuid;
		connectLocalServer = _connectLocalServer;
	}

	public void OnPush() {
		networkManager.ConnectToServer(serverGuid, connectLocalServer);
	}
}
