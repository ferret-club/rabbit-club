using UnityEngine;
using System.Collections;

public class InventorySlide : MonoBehaviour {

    int arrow = 0; // 0:どちらでもない,1:右,2:左
    public float speed = 2000.0f;

    void Start() {
	}

	void Update() {
        move();
	}

    public void OnArrowRightButton() {
        arrow = 1;
    }
    public void OnArrowLeftButton() {
        arrow = 2;
    }

    void move() {
        // 現在の位置を取得
        RectTransform rTran = (RectTransform)this.GetComponent<RectTransform>();
        float x = rTran.anchoredPosition.x;
        switch(arrow) {
          case 0:
            // 何もしない
            break;
        case 1:
            if (x <= -722.0f) {
                x = -722.0f;
                arrow = 0; // 移動しきったら移動フラグをニュートラルにしておく。
            } else {
                x -= speed * Time.deltaTime;
            }
            break;
          case 2:
            if (x >= 500.0f) {
                x = 500.0f;
                arrow = 0; // 移動しきったら移動フラグをニュートラルにしておく。
            } else {
                x += speed * Time.deltaTime;
            }
            break;
          default:
            break;
        }
        rTran.anchoredPosition = new Vector2(x, rTran.anchoredPosition.y);
    }
}
