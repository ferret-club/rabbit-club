using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int targetFrameRate = 60;

    GameObject buttonGroup;
	[SerializeField]
	CountDownTimer countDownTimer;
	// ゲームオーバー処理は一回しか呼ばない
	bool gameOverTrigger = true;
	public FailText failText;
	public GameObject noTapPanel;

	void Awake ()
	{
		Application.targetFrameRate = targetFrameRate;
	}

	void Start () {
        buttonGroup = GameObject.Find("Door/ButtonGroup");
        buttonGroup.SetActive(false);
	}

	void Update() {
		// カウントダウンが終了していたらゲームオーバー処理
		if(gameOverTrigger && countDownTimer.paused && countDownTimer.getTimer() <= 0.0f) {
			resultFailed();
		}
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

	// 脱出成功処理
	public void resultSuccess() {
		// タイマーを停止する
		countDownTimer.paused = true;
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
}
