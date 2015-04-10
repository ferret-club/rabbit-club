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
	public static string playerName;
	public static int playerNum;

	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
