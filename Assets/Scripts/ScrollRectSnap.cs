using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SocketIO;
using LitJson;

public class ScrollRectSnap : MonoBehaviour, IEndDragHandler
{

	float[] points;
	[Tooltip ("how many screens or pages are there within the content (steps)")]
	public int screens = 1;
	[Tooltip ("How quickly the GUI snaps to each panel")]
	public float snapSpeed;
	public float inertiaCutoffMagnitude;
	float stepSize;
	ScrollRect scroll;
	private GameObject scrollContent;
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
	int target = 2;

	private SocketIOComponent socket;
	string clientId;
	string connectId;

	bool isLobby = true;

	[SerializeField]
	GameObject Lobby = null;
	[SerializeField]
	GameObject CountDownTimer = null;

	[SerializeField]
	NetworkShare networkShare;

	void Start ()
	{
		scroll = gameObject.GetComponent<ScrollRect>();
		scrollContent = GameObject.Find("Canvas/ScrollRect/Content").gameObject;
		if (screens > 0) {
			points = new float[screens];
			stepSize = 1 / (float)(screens - 1);
			for (int i = 0; i < screens; i++) {
				points [i] = i * stepSize;
			}
		} else {
			points [0] = 0;
		}
	}

	void Update() {
		if(DontDestroy.gameStatus == DontDestroy.GameStatus.Start) {
			DontDestroy.gameStatus = DontDestroy.GameStatus.Play;
			gameMainStart();
		}

		if(LerpH) {
			scroll.horizontalNormalizedPosition = Mathf.Lerp (scroll.horizontalNormalizedPosition, targetH, snapSpeed * Time.deltaTime);
			float targetPositionX = 0;
			switch(target) {
			case 0:
				targetPositionX = 2160;
				break;
			case 1:
				targetPositionX = 1080;
				break;
			case 3:
				targetPositionX = -1080;
				break;
			case 4:
				targetPositionX = -2160;
				break;
			case 2:
			default:
				targetPositionX = 0;
				break;
			}
			if(Mathf.Abs(scrollContent.transform.localPosition.x - targetPositionX) < 1) {
				LerpH = false;
				// 他のユーザーにもアバターの位置情報を送る
				networkShare.updatePosition (DontDestroy.playerNum, (int)DontDestroy.avater, target);
			}
		}
		if(LerpV) {
			scroll.verticalNormalizedPosition = Mathf.Lerp (scroll.verticalNormalizedPosition, targetV, snapSpeed * Time.deltaTime);
			if (Mathf.Approximately (scroll.verticalNormalizedPosition, targetV))
				LerpV = false;
		}
	}

	public void DragEnd()
	{
		target = FindNearest(scroll.horizontalNormalizedPosition, points);
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

	public void OnDrag() {
		if (dragInit) {
			dragStartNearest = FindNearest(scroll.horizontalNormalizedPosition, points);
			dragInit = false;
		}
		LerpH = false;
		LerpV = false;
	}

	int FindNearest(float f, float[] array)
	{
		float distance = Mathf.Infinity;
		roomNumber = 0;
		for (int index = 0; index < array.Length; index++) {
			if (Mathf.Abs (array [index] - f) < distance) {
				distance = Mathf.Abs (array [index] - f);
				roomNumber = index;
			}
		}
		return roomNumber;
	}

	// ゲームを開始ボタンが押された時の処理
	public void OnLoadMain() {
		DontDestroy.gameStatus = DontDestroy.GameStatus.Start;
		networkShare.setGameStatus(DontDestroy.GameStatus.Start);
	}

	// ゲームを開始する
	private void gameMainStart()
	{
		GameObject MusicManagerObj = GameObject.Find("MusicManager") as GameObject;
		MusicManagerObj.GetComponent<MusicManager>().PlayBgm();
		isLobby = false;
		Lobby.SetActive(false);
		// 受け取っている最新のアバター情報で自分から見えている他プレイヤーのアバターを表示する
		networkShare.setAvaterActive();
		// カウントダウンタイマーを有効にして開始する
		CountDownTimer.SetActive(true);
		CountDownTimer.transform.GetComponent<CountDownTimer>().initialize();
		CountDownTimer.transform.GetComponent<CountDownTimer>().paused = false;
	}

	public void OnEndDrag(PointerEventData eventData) {
		DragEnd();
	}

}
