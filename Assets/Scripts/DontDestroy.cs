using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DontDestroy : MonoBehaviour
{
	public enum Avater {
		Bear = 0,
		Rabbit = 1,
		Dog = 2,
		Bird = 3,
	}
	public static Avater avater;
	public enum GameStatus {
		Title,
		Lobby,
		Room,
		Start,
		Play,
		Success,
		Failed,
	}
	public static GameStatus gameStatus;
	public static string playerName;
	public static int playerNum;
	// リザルト用
	public static float resultTime;
	public static bool successFlg;

	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
