using UnityEngine;
using System.Collections;

public class Item {

    public int id;
    public string name;
    public string description;
    // アイテム欄の何番目に設定したか
    private int imgIndex;
    public int ImgIndex {
        set { imgIndex = value; }
        get { return imgIndex; }
    }

    public Item(int _id, string _name, string _description) {
        init(_id, _name, _description);
    }

    private void init(int _id, string _name, string _description) {
        this.id = _id;
        this.name = _name;
        this.description = _description;
    }

}
