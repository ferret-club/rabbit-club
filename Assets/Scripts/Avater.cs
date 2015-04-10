using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Avater : MonoBehaviour {

	public Sprite[] avaterIconSprite;
	private float movePositionX = 1080;
	// 元々居た部屋番号0〜4で2が中央
	private int prevRoomNumber = 2;

	public void setSprite(int avater) {
		this.GetComponent<Image>().sprite = this.avaterIconSprite[avater];
	}

	public void moveAvater(int roomNumber) {
		if(prevRoomNumber == roomNumber) {
			// 部屋番号に変更がない場合は何もしない
			return;
		}
		float localMoveX = movePositionX;
		if (prevRoomNumber > roomNumber) {
			localMoveX = -localMoveX;
		}
		// 新しいポジションを保存しておく
		prevRoomNumber = roomNumber;
		Vector3 position = new Vector3(localMoveX, 0, 0);
		var hash = new Hashtable {
			{ "amount", position },
			{ "time", 0.5f },
		};
		iTween.MoveAdd (this.gameObject, hash);
	}

}
