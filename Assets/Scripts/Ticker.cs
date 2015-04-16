using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ticker : MonoBehaviour
{

	Text TickerText;
	[SerializeField]
	NetworkManager networkManager;

	void Start ()
	{
		GameObject ChatFieldGameObject = GameObject.Find ("TickerText");
		TickerText = ChatFieldGameObject.GetComponent<Text> ();
	}

	public void callTicker() {
//		Debug.Log("callTicker()");
		// ネットワーク接続が確立されていれば接続先にもメッセージを送る
		if (networkManager != null && 
			(networkManager.GetStatus() == NetworkManager.Status.ConnectedToServer
			|| networkManager.GetStatus() == NetworkManager.Status.ServerLaunched)) {
			GetComponent<NetworkView>().RPC("OnTicker", RPCMode.Others, new object[] {DontDestroy.playerName});
		}
	}

	[RPC]
	public void OnTicker(string characterName) {
//		Debug.Log("OnTicker() characterName" + characterName);
		TickerText.text = characterName + "がアイテムを見つけました！";
		Vector3 position = new Vector3 (0, -96, 0);
		var hash = new Hashtable {
			{ "amount", position },
			{ "time", 0.5f },
		};
		iTween.MoveAdd (gameObject, hash);

        Invoke("OffTicker", 5);
	}

	public void OffTicker ()
	{
		Vector3 position = new Vector3 (0, 96, 0);
		var hash = new Hashtable {
			{ "amount", position },
			{ "time", 0.5f },
		};
		iTween.MoveAdd (gameObject, hash);
	}
}
