using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;
using LitJson;

public class ScrollRectSnap : MonoBehaviour
{

	float[] points;
	[Tooltip ("how many screens or pages are there within the content (steps)")]
	public int screens = 1;
	[Tooltip ("How quickly the GUI snaps to each panel")]
	public float snapSpeed;
	public float inertiaCutoffMagnitude;
	float stepSize;
	ScrollRect scroll;
	bool LerpH;
	float targetH;
	[Tooltip ("Snap horizontally")]
	public bool snapInH = true;
	bool LerpV;
	float targetV;
	[Tooltip ("Snap vertically")]
	public bool snapInV = true;
	bool dragInit = true;
	int dragStartNearest;
	int roomNumber = 2;

	private SocketIOComponent socket;
	string clientId;
	string connectId;
	private Message message = new Message ();
	private Hashtable connections = new Hashtable ();
	private Hashtable endingState = new Hashtable ();
	private Dictionary<string, GameObject> playerObjects = new Dictionary<string, GameObject> ();

	bool isLobby = true;

	[SerializeField]
	GameObject Lobby = null;

	[SerializeField]
	GameObject RabbitLogin = null;

	[SerializeField]
	GameObject DogLogin = null;

	[SerializeField]
	GameObject BirdLogin = null;

	[SerializeField]
	GameObject CountDownTimer = null;

	[SerializeField]
	GameObject[] PlayerPrefabArray = null;

	[SerializeField]
	GameObject PlayerParent = null;

	[SerializeField]
	GameObject TickerObject = null;

	private int[] CharacterPositionArray = {
		-2630,
		-1550,
		-470,
		610,
		1690
	};

	Dictionary<string, string> CharacterName
	= new Dictionary<string, string> () {
		{ "PlayerRabbit", "うさぎさん" },
		{ "PlayerRabbit(Clone)", "うさぎさん" },
		{ "PlayerDog", "いぬさん" },
		{ "PlayerDog(Clone)", "いぬさん" },
		{ "PlayerBird", "とりさん" },
		{ "PlayerBird(Clone)", "とりさん" }
	};

	// Use this for initialization
	void Start ()
	{
		scroll = gameObject.GetComponent<ScrollRect> ();
		//scroll.inertia = true;
		if (screens > 0) {
			points = new float[screens];
			stepSize = 1 / (float)(screens - 1);
			for (int i = 0; i < screens; i++) {
				points [i] = i * stepSize;
			}
		} else {
			points [0] = 0;
		}

		int rdm = UnityEngine.Random.Range (1, 100);
		clientId = SystemInfo.deviceUniqueIdentifier + rdm;
		connections.Add (clientId, "");
		endingState.Add (clientId, "OFF");
		GameObject go = GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent> ();
		socket.On ("open", OnSocketOpen);
		socket.On ("sendMsgFromServer", OnSocketSendMsgFromServer);
		socket.On ("connectIdFromServer", OnSocketConnectIdFromServer);
		socket.On ("disConnectIdFromServer", OnSocketDisConnectIdFromServer);
		socket.On ("error", OnSocketError);
		socket.On ("close", OnSocketClose);	
	}

	void Update ()
	{
		if (LerpH) {
			scroll.horizontalNormalizedPosition = Mathf.Lerp (scroll.horizontalNormalizedPosition, targetH, snapSpeed * Time.deltaTime);
			if (Mathf.Approximately (scroll.horizontalNormalizedPosition, targetH))
				LerpH = false;
		}
		if (LerpV) {
			scroll.verticalNormalizedPosition = Mathf.Lerp (scroll.verticalNormalizedPosition, targetV, snapSpeed * Time.deltaTime);
			if (Mathf.Approximately (scroll.verticalNormalizedPosition, targetV))
				LerpV = false;
		}

		if (isLobby) {
			Dictionary<string, string> data = new Dictionary<string, string> ();
			data ["clientId"] = clientId;
			data ["roomNum"] = "2";
			data ["ticker"] = "NO";
			data ["endButton"] = "OFF";
			socket.Emit ("sendMsgFromClient", new JSONObject (data));
		}
	}

	public void DragEnd ()
	{
		int target = FindNearest (scroll.horizontalNormalizedPosition, points);
		if (target == dragStartNearest && scroll.velocity.sqrMagnitude > inertiaCutoffMagnitude * inertiaCutoffMagnitude) {
			if (scroll.velocity.x < 0) {
				target = dragStartNearest + 1;
			} else if (scroll.velocity.x > 1) {
				target = dragStartNearest - 1;
			}
			target = Mathf.Clamp (target, 0, points.Length - 1);
		}
		if (scroll.horizontal && snapInH && scroll.horizontalNormalizedPosition > 0f && scroll.horizontalNormalizedPosition < 1f) {
			targetH = points [target];
			LerpH = true;
		}
		if (scroll.vertical && snapInV && scroll.verticalNormalizedPosition > 0f && scroll.verticalNormalizedPosition < 1f) {
			targetH = points [target];
			LerpH = true;
		}
		dragInit = true;
	}

	public void OnDrag ()
	{
		if (dragInit) {
			dragStartNearest = FindNearest (scroll.horizontalNormalizedPosition, points);
			dragInit = false;
		}
		LerpH = false;
		LerpV = false;
	}

	int FindNearest (float f, float[] array)
	{
		float distance = Mathf.Infinity;
		roomNumber = 0;
		for (int index = 0; index < array.Length; index++) {
			if (Mathf.Abs (array [index] - f) < distance) {
				distance = Mathf.Abs (array [index] - f);
				roomNumber = index;
			}
		}
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["clientId"] = clientId;
		data ["roomNum"] = roomNumber.ToString ();
		data ["ticker"] = "NO";
		data ["endButton"] = "OFF";
		socket.Emit ("sendMsgFromClient", new JSONObject (data));
		return roomNumber;
	}

	public void SendTicker ()
	{
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["clientId"] = clientId;
		data ["roomNum"] = roomNumber.ToString ();
		data ["ticker"] = "YES";
		data ["endButton"] = "OFF";
		socket.Emit ("sendMsgFromClient", new JSONObject (data));
	}

	public void SendEnding ()
	{
		endingState [clientId] = "ON";

		int onButtonCount = 0;
		foreach (DictionaryEntry button in endingState) {
			if (button.Value == "ON") {
				onButtonCount++;
			}
		}

		if (onButtonCount >= 4) {
			Application.LoadLevel ("ResultScene");
		}

		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["clientId"] = clientId;
		data ["roomNum"] = roomNumber.ToString ();
		data ["ticker"] = "NO";
		data ["endButton"] = "ON";
		socket.Emit ("sendMsgFromClient", new JSONObject (data));
	}

	public void OnSocketOpen (SocketIOEvent e)
	{
		Debug.Log ("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void OnSocketConnectIdFromServer (SocketIOEvent e)
	{
		connectId = e.data.GetField ("connectId").str;
	}

	public void OnSocketDisConnectIdFromServer (SocketIOEvent e)
	{
		string disConnectId = e.data.GetField ("disConnectId").str;
		foreach (string key in connections.Keys) {
			string connectionId = (string)connections [key];
			if (connectionId == disConnectId) {
				GameObject disConnectPlayer = GameObject.Find (key);
				if (disConnectPlayer != null) {
					Debug.Log ("destroy");
					Destroy (disConnectPlayer.gameObject);
				}
			}
		}
	}

	public void OnSocketSendMsgFromServer (SocketIOEvent e)
	{

		//Debug.Log("[SocketIO] msg from server: " + e.name + " " + e.data);
		string msgCid = e.data.GetField ("clientId").str;
		int msgRoomNum = int.Parse (e.data.GetField ("roomNum").str);
		string msgTicker = e.data.GetField ("ticker").str;
		string msgEndButton = e.data.GetField ("endButton").str;

		if (msgCid != clientId) {
			Debug.Log (msgRoomNum);
			if (!connections.ContainsKey (msgCid)) {
				connections.Add (msgCid, connectId);
				endingState.Add (msgCid, "OFF");
				Debug.Log ("crate");
				Debug.Log (connections.Count);
				GameObject player = (GameObject)Instantiate (PlayerPrefabArray [connections.Count - 2]);
				player.transform.parent = PlayerParent.transform;
				int additional = 0;
				switch (player.name) {
				case "PlayerRabbit(Clone)":
					additional = 0;
					break;
				case "PlayerDog(Clone)":
					additional = 160;
					break;
				case "PlayerBird(Clone)":
					additional = 320;
					break;
				}
				player.transform.position = new Vector2 (CharacterPositionArray [2] + additional, 90);
				playerObjects [msgCid] = player;

				if (connections.Count == 2) {
					RabbitLogin.SetActive (true);
				}

				if (connections.Count == 3) {
					DogLogin.SetActive (true);
				}

				if (connections.Count == 4) {
					BirdLogin.SetActive (true);
				}
			} else {
				GameObject player = playerObjects [msgCid];
				int additional = 0;
				switch (player.name) {
				case "PlayerRabbit(Clone)":
					additional = 0;
					break;
				case "PlayerDog(Clone)":
					additional = 160;
					break;
				case "PlayerBird(Clone)":
					additional = 320;
					break;
				}

				Vector3 position = new Vector3 (CharacterPositionArray [msgRoomNum] + additional, 90, 0);
				var hash = new Hashtable {
					{ "position", position },
					{ "time", 0.5f },
					{ "easetype", iTween.EaseType.easeOutCubic }
				};
				iTween.MoveTo (player, hash);

				if (msgTicker == "YES") {
					Ticker ticker = TickerObject.GetComponent<Ticker> ();
					ticker.OnTicker (CharacterName [player.name]);
				}

				if (msgEndButton == "ON") {
					endingState [msgCid] = "ON";
				}
			}

			int onButtonCount = 0;
			foreach (DictionaryEntry button in endingState) {
				if (button.Value == "ON") {
					onButtonCount++;
				}
			}

			if (onButtonCount >= 4) {
				Application.LoadLevel ("ResultScene");
			}

			if (isLobby && connections.Count >= 4) {
				StartCoroutine ("OnLoadMainFromLobby");
			}
		}
	}

	private IEnumerator OnLoadMainFromLobby ()
	{
		yield return new WaitForSeconds (3.0f);
		OnLoadMain ();
	}

	public void OnLoadMain ()
	{
		isLobby = false;
		Lobby.SetActive (false);
		CountDownTimer.SetActive (true);
	}

	public void OnSocketError (SocketIOEvent e)
	{
		Debug.Log ("[SocketIO] Error received: " + e.name + " " + e.data);
	}

	public void OnSocketClose (SocketIOEvent e)
	{
		Debug.Log ("[SocketIO] Close received: " + e.name + " " + e.data);
	}

	public class Message
	{
		public string clientId  { get; set; }

		public int roomNum   { get; set; }

		public string ticker { get; set; }

		public string endButton { get; set; }
	}
}
