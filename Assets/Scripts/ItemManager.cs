using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemManager : MonoBehaviour {

    // アイテムリスト
    private Item[] items;
    // 所有しているアイテム
    public Item[] haveItems;
	// 選択しているアイテム
	[HideInInspector]
	public Item selectedItem;
    public GameObject[] itemButtons;
    public Sprite[] itemImages;
	// アイテムの説明を表示する領域
	public Message messageArea;

	void Start () {
        generateItems();
//		getItem(3);
//		getItem(8);
//		getItem(7);
//		getItem(11);
//		getItem(9);
	}

    // アイテム初期設定
    void generateItems() {
		items = new Item[12];
		items[0] = new Item(0, "ねずみの文鎮", "【ねずみの文鎮】だ\n暗がりでみるととってもリアル！");
		items[1] = new Item(1, "うしのカップ", "【うしのカップ】だ\nつぶらな瞳がとってもチャーミング！");
		items[2] = new Item(2, "とらの電池", "【とらの電池】だ\nなんでも動かせるくらいパワフル！");
		items[3] = new Item(3, "うさぎの靴下", "【うさぎの靴下】だ"); // 初めからあるのでアイテム画像設定なし
		items[4] = new Item(4, "りゅうのじょうろ", "【りゅうのじょうろだ】だ\n龍の力で植物が急成長！……なんてね");
		items[5] = new Item(5, "へびのナイフ", "【へびのナイフ】だ\n長くて使いづらいが切れ味はバツグン！");
		items[6] = new Item(6, "うまの絵の皿", "【うまの絵の皿】だ\n緻密なタッチで描かれている");
		items[7] = new Item(7, "ひつじのミトン", "【ひつじのミトン】だ"); // 初めからあるのでアイテム画像設定なし
		items[8] = new Item(8, "さるの腰かけ", "【さるの腰かけ】だ\nきのこではない");
		items[9] = new Item(9, "とりの傘", "【とりの傘】だ"); // 初めからあるのでアイテム画像設定なし
		items[10] = new Item(10, "いぬのティッシュカバー", "【いぬのティッシュカバー】だ\nプレゼントにいいかもしれない");
		items[11] = new Item(11, "いのししのミニカー", "【いのししのミニカー】だ"); // 初めからあるのでアイテム画像設定なし
        // 所持アイテムを初期化する
        haveItems = new Item[items.Length];
        for(int id = 0; id < haveItems.Length; id++) {
            haveItems[id] = null;
        }
	}

    // アイテム取得処理
    public void getItem(int id) {
        // 所有しているアイテムにアイテムオブジェクトを設定する
        haveItems[id] = items[id];
        // アイテム欄の空いているところに画像を設定する
        for(int index = 0; index < itemButtons.Length; index++) {
            // 最初に見つけた画像未設定のアイテム欄に画像を設定する
			if(itemButtons[index].transform.FindChild("ItemImg").GetComponent<Image>().enabled == false) {
				itemButtons[index].transform.FindChild("ItemImg").GetComponent<Image>().enabled = true;
				itemButtons[index].transform.FindChild("ItemImg").GetComponent<Image>().sprite = itemImages[id];
                // アイテム欄の何番目にセットしたか保存しておく
                haveItems[id].ImgIndex = index;
                break; // 画像をセットしたら処理を抜ける
            }
        }
    }

    // アイテムを失う処理
    public void removeItem(int id) {
        // アイテム欄の画像をnullにする。
		itemButtons[haveItems[id].ImgIndex].transform.FindChild("ItemImg").GetComponent<Image>().sprite = null;
		itemButtons[haveItems[id].ImgIndex].transform.FindChild("ItemImg").GetComponent<Image>().enabled = false;
        // 所有しているアイテムを無くす
        haveItems[id] = null;
    }

	// アイテム選択処理。
	public void OnSelectedItem(int id) {
		// メッセージ領域を初期化
		messageArea.OffMessage();
		// 一旦全ての選択状態を解除する
		for(int index = 0; index < itemButtons.Length; index++) {
			itemButtons[index].transform.FindChild("SelectedImg").GetComponent<Image>().enabled = false;
		}
		// 選択しているアイテムを解除
		selectedItem = null;
		for (int i = 0; i < haveItems.Length; i++) {
			if (haveItems[i] != null && haveItems[i].ImgIndex == id) {
				// 選択しているアイテムを設定
				selectedItem = haveItems[i];
				// タッチされた自分自身の選択状態だけOnにする
				itemButtons[id].transform.FindChild("SelectedImg").GetComponent<Image>().enabled = true;
				messageArea.OnMessage(selectedItem.description);
			}
		}
	}
}
