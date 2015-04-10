using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public int targetFrameRate = 60;

    GameObject buttonGroup;

	void Awake ()
	{
		Application.targetFrameRate = targetFrameRate;
	}

	void Start () {
        buttonGroup = GameObject.Find ("Door/ButtonGroup");
        buttonGroup.SetActive(false);
	}

	// リザルト画面に遷移する
	public void goResultScene() {
		// リザルトに残しておきたいオブジェクトをDontDestroyの子要素に設定しておいて引き継ぐ
//		GameObject chatObject = GameObject.Find ("Canvas/RowerObject/ScrollRect 2") as GameObject;
//		GameObject chatFieldObject = GameObject.Find ("Canvas/ChatBackground") as GameObject;
//		GameObject DontDestroyObject = GameObject.Find ("DontDestroyObject") as GameObject;
//		Debug.Log ("name:" + chatObject.name);
//		chatObject.transform.parent = DontDestroyObject.transform;
//		chatFieldObject.transform.parent = DontDestroyObject.transform;
        Invoke("LoadResult", 3);
		
	}

    void LoadResult ()
    {
        Application.LoadLevel("ResultScene");
    }
}
