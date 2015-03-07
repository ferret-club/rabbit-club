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
        getItem(1);
        getItem(0);
        removeItem(1);
	}

    // アイテム初期設定
    void generateItems() {
        items = new Item[3];
        items[0] = new Item(0, "アイテム1", "説明1");
        items[1] = new Item(1, "アイテム2", "説明2");
        items[2] = new Item(2, "アイテム3", "説明3");
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
            if(itemButtons[index].GetComponent<Image>().sprite == null) {
                itemButtons[index].GetComponent<Image>().sprite = itemImages[id];
                // アイテム欄の何番目にセットしたか保存しておく
                haveItems[id].ImgIndex = index;
                break; // 画像をセットしたら処理を抜ける
            }
        }
    }

    // アイテムを失う処理
    public void removeItem(int id) {
        // アイテム欄の画像をnullにする。
        itemButtons[haveItems[id].ImgIndex].GetComponent<Image>().sprite = null;
        // 所有しているアイテムを無くす
        haveItems[id] = null;
    }
}
