using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour {

	[SerializeField]
	private GameObject nodePrefab;
	private GameObject scrollContent;
	// 縦位置
	private float potisionY = 360.0f;
	// 縦位置増分
	private float potisionIncY = 60.0f;
	// 横位置
	private float potisionX = 0.0f;
	private int scrollResizeStartCnt = 6;
	private bool useScrollMove = false;
	private RectTransform scrollRect;
	// ノードを管理するリスト
	private List<GameObject> nodeList = new List<GameObject>();

	void Start () {
		scrollContent = this.transform.FindChild("Content").gameObject;
		scrollRect = scrollContent.GetComponent<RectTransform>();
	}

	public GameObject addNode() {
		GameObject node = null;
		node = Instantiate (nodePrefab) as GameObject;
		nodeList.Add(node);

		return node;
	}

	public void refreshShowNode() {
		int cnt = 0;
		foreach (var node in nodeList) {
			cnt++;
			showNode(node, cnt);
		}
	}

	private void showNode(GameObject _node, int _nodeCnt) {
		if(scrollContent == null) {
			scrollContent = this.transform.FindChild("Content").gameObject;
		}
		_node.transform.SetParent(scrollContent.transform);

		// ノードを個数に応じた形にずらして配置する
		if (_nodeCnt > scrollResizeStartCnt) {
			_node.transform.localPosition = new Vector2 (potisionX, potisionY - ((potisionIncY * 2) * _nodeCnt) + (potisionIncY * (_nodeCnt - scrollResizeStartCnt)));
		} else {
			_node.transform.localPosition = new Vector2 (potisionX, potisionY - ((potisionIncY * 2) * _nodeCnt));
		}

		if(_nodeCnt >= scrollResizeStartCnt) {
			// ノード数が増えたらScrollRectの大きさを拡げる
			if(scrollRect == null) {
				scrollRect = scrollContent.GetComponent<RectTransform>();
			}
			scrollRect.sizeDelta = new Vector2(scrollRect.sizeDelta.x, scrollRect.sizeDelta.y + (potisionIncY * 2));
			if(useScrollMove) {
				// ScrollRectの大きさを拡げた分位置を下にずらす
				scrollContent.transform.localPosition = new Vector2 (scrollContent.transform.localPosition.x, scrollContent.transform.localPosition.y + (potisionIncY * 2));
			} else {
				// 動いていないように見せるため、ScrollRectの大きさを拡げた分位置を上にずらす
				scrollContent.transform.localPosition = new Vector2 (scrollContent.transform.localPosition.x, scrollContent.transform.localPosition.y - (potisionIncY * 2));
			}
		}
	}

}
