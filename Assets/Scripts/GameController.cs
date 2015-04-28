using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int targetFrameRate = 60;

    GameObject buttonGroup;
	[SerializeField]
	CountDownTimer countDownTimer;
	// ゲームオーバー/クリア処理は一回しか呼ばない
	bool gameOverTrigger = true;
	bool gameClearTrigger = true;
	public ClearText clearText;
	public FailText failText;
	public GameObject noTapPanel;
	[SerializeField]
	NetworkShare networkShare;
	[SerializeField]
	GameObject rowerObject;

	void Awake ()
	{
		Application.targetFrameRate = targetFrameRate;
	}

	void Start () {
        buttonGroup = GameObject.Find("Door/ButtonGroup");
        buttonGroup.SetActive(false);
		DontDestroy.gameStatus = DontDestroy.GameStatus.Lobby;
	}

	void Update() {
		// カウントダウンが終了していたらゲームオーバー処理
		if(gameOverTrigger && countDownTimer.paused && countDownTimer.getTimer() <= 0.0f) {
			OnResultFailed();
			resultFailed();
		}
		if(gameClearTrigger && DontDestroy.gameStatus == DontDestroy.GameStatus.Success) {
			resultSuccess();
		} else if(gameOverTrigger && DontDestroy.gameStatus == DontDestroy.GameStatus.Failed) {
			resultFailed();
		}
	}

	private void OnResultFailed() {
		DontDestroy.gameStatus = DontDestroy.GameStatus.Failed;
		networkShare.setGameStatus(DontDestroy.GameStatus.Failed);
	}

	// 脱出失敗処理
	private void resultFailed() {
		gameOverTrigger = false;
		// 前面にパネルを置いて操作を受け付けなくする
		noTapPanel.transform.GetComponent<Image>().enabled = true;
		// 文字を表示する
		failText.OnCall();
		// リザルト画面に遷移する
		goResultScene(false);
	}

	public void OnResultSuccess() {
		DontDestroy.gameStatus = DontDestroy.GameStatus.Success;
		networkShare.setGameStatus(DontDestroy.GameStatus.Success);
	}

	// 脱出成功処理
	private void resultSuccess() {
		gameClearTrigger = false;
		// タイマーを停止する
		countDownTimer.paused = true;
		// 前面にパネルを置いて操作を受け付けなくする
		noTapPanel.transform.GetComponent<Image>().enabled = true;
		// 文字を表示する
		clearText.OnCall();
		// リザルト画面に遷移する
		goResultScene(true);
	}

	// リザルト画面に遷移する
	private void goResultScene(bool successFlg) {
		// リザルト画面へ受け渡す値を設定する
		DontDestroy.successFlg = successFlg;
		DontDestroy.resultTime = countDownTimer.getResultTime();
		// リザルトに残しておきたいオブジェクトをDontDestroyの子要素に設定しておいて引き継ぐ
//		GameObject chatObject = GameObject.Find ("Canvas/RowerObject/ScrollRect 2") as GameObject;
//		GameObject chatFieldObject = GameObject.Find ("Canvas/ChatBackground") as GameObject;
//		GameObject DontDestroyObject = GameObject.Find ("DontDestroyObject") as GameObject;
//		Debug.Log ("name:" + chatObject.name);
//		chatObject.transform.parent = DontDestroyObject.transform;
//		chatFieldObject.transform.parent = DontDestroyObject.transform;
		Invoke("LoadResult", 4);
	}

	void LoadResult() {
        Application.LoadLevel("ResultScene");
    }

	public void setGameStatusDoor() {
		DontDestroy.gameStatus = DontDestroy.GameStatus.Door;
	}

	public void setGameStatusPlay() {
		DontDestroy.gameStatus = DontDestroy.GameStatus.Play;
	}
}
