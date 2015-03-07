using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemManager : MonoBehaviour {

    // アイテムリスト
    private Item[] items;
    // 所有しているアイテム
    public Item[] haveItems;
    public GameObject[] itemButtons;
    public Sprite[] itemImages;

	void Start () {
        generateItems();
//        getItem(1);
//        getItem(0);
//        removeItem(1);
//		getItem(10);
//		getItem(8);
//		getItem(6);
//		getItem(2);
//		getItem(1);
//		getItem(5);
//		getItem(4);
	}

    // アイテム初期設定
    void generateItems() {
		items = new Item[12];
		items[0] = new Item(0, "ねずみの文鎮", "説明1");
		items[1] = new Item(1, "うしのカップ", "説明2");
		items[2] = new Item(2, "とらの電池", "説明3");
		items[3] = new Item(3, "うさぎの靴下", "説明3"); // 初めからあるのでアイテム画像設定なし
		items[4] = new Item(4, "りゅうのじょうろ", "説明3");
		items[5] = new Item(5, "へびのナイフ", "説明3");
		items[6] = new Item(6, "うまの絵の皿", "説明3");
		items[7] = new Item(7, "ひつじのミトン", "説明3"); // 初めからあるのでアイテム画像設定なし
		items[8] = new Item(8, "さるの腰かけ", "説明3");
		items[9] = new Item(9, "とりの傘", "説明3"); // 初めからあるのでアイテム画像設定なし
		items[10] = new Item(10, "いぬのティッシュカバー", "説明3");
		items[11] = new Item(11, "いのししのミニカー", "説明3"); // 初めからあるのでアイテム画像設定なし
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
}
