using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ticker : MonoBehaviour
{

	Text TickerText;

	void Start ()
	{
		GameObject ChatFieldGameObject = GameObject.Find ("TickerText");
		TickerText = ChatFieldGameObject.GetComponent<Text> ();
	}

	public void OnTicker (string characterName)
	{
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
