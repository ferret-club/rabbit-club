using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatManager : MonoBehaviour {

	[SerializeField]
	private GameObject nodeMePrefab;
	[SerializeField]
	private GameObject nodeOtherPrefab;
	private GameObject scrollContent;
	// 縦位置
	private float potisionY = 940.0f;
	// 縦位置増分
	private float potisionIncY = 120.0f;
	// 横位置
	private float potisionX = 0.0f;
	private int scrollResizeStartCnt = 16;
	private bool useScrollMove = true;
	private RectTransform scrollRect;
	private float firstSizeDeltaY;
	private struct Node {
		public GameObject body; // ノード本体
		public int speaker;     // 発言者
		public bool speakMe;    // 自分の発言か
		public string message;  // 発言内容
	}
	// ノードを管理するリスト
	private List<Node> nodeList = new List<Node>();
	[SerializeField]
	private InputField inputField;
	private bool isMaximize = true;

	void Start () {
		scrollContent = this.transform.FindChild("Content").gameObject;
		scrollRect = scrollContent.GetComponent<RectTransform>();
		// 初期の大きさを保存しておく
		firstSizeDeltaY = scrollRect.sizeDelta.y;
//		resizePanelMinimize();
		// 確認用
//		for(int i = 1; i <= 100; i++) {
//			this.addNode(false, "てすと" + i);
//		}
//		this.refreshShowNode();
	}

	public void addMessage() {
		if(string.IsNullOrEmpty(inputField.text)) {
			return;
		}
		string msg = inputField.text;
		this.addNode(true, msg);
		this.refreshShowNode();
	}

	public GameObject addNode(bool speakMe, string msg) {
		GameObject nodeBody = null;
		nodeBody = (speakMe)? Instantiate (nodeMePrefab) as GameObject: Instantiate (nodeOtherPrefab) as GameObject;
		Node node = new Node();
		node.body = nodeBody;
		node.speakMe = speakMe;
		node.message = msg;
		nodeBody.transform.FindChild("Balloon/Text").GetComponent<Text>().text = msg;
		nodeList.Add(node);

		return nodeBody;
	}

	public void refreshShowNode() {
		int cnt = 0;
		if(nodeList.Count > scrollResizeStartCnt) {
			// ノード数が増えたらScrollRectの大きさを拡げる
			if(scrollRect == null) {
				scrollRect = scrollContent.GetComponent<RectTransform>();
			}
			// 拡げる大きさ
			float extentSize = potisionIncY * (nodeList.Count - scrollResizeStartCnt) + 5;
			scrollRect.sizeDelta = new Vector2(scrollRect.sizeDelta.x, firstSizeDeltaY + extentSize);
		}
		foreach (var node in nodeList) {
			cnt++;
			showNode(node.body, cnt);
		}
	}

	private void showNode(GameObject _node, int _nodeCnt) {
		if(scrollContent == null) {
			scrollContent = this.transform.FindChild("Content").gameObject;
		}
		_node.transform.SetParent(scrollContent.transform);
		if (isMaximize) {
			_node.GetComponent<RectTransform> ().sizeDelta = new Vector2 (-0.5f, -1895);
		} else {
			_node.GetComponent<RectTransform> ().sizeDelta = new Vector2 (-0.5f, -700);
		}

		if(_nodeCnt >= scrollResizeStartCnt) {
			if(useScrollMove) {
				// ScrollRectの大きさを拡げた分位置を下にずらす
				scrollContent.transform.localPosition = new Vector2 (scrollContent.transform.localPosition.x, scrollContent.transform.localPosition.y + potisionIncY / 2 + 2);
			} else {
				// 動いていないように見せるため、ScrollRectの大きさを拡げた分位置を上にずらす
				scrollContent.transform.localPosition = new Vector2 (scrollContent.transform.localPosition.x, scrollContent.transform.localPosition.y - potisionIncY / 2 - 2);
			}
		}
		// ノードを個数に応じた形にずらして配置する
		if (nodeList.Count >= scrollResizeStartCnt) {
			// ノード数に応じてパネルを拡げた高さ/2 + 基準点
			float extentSize = potisionIncY * (nodeList.Count + 1 - scrollResizeStartCnt) / 2 + potisionY;
			_node.transform.localPosition = new Vector2(potisionX, extentSize -(potisionIncY * _nodeCnt));
		} else {
			_node.transform.localPosition = new Vector2(potisionX, potisionY - (potisionIncY * _nodeCnt));
		}
	}

	public void resizePanelMinimize() {
		isMaximize = false;
		resizePanel(-486, -1210);
	}

	public void resizePanelMaximize() {
		isMaximize = true;
		resizePanel(57.75006f, -115.5f);
	}

	private void resizePanel(float localPositionX, float sizeDeltaY) {
		this.transform.localPosition = new Vector2(this.transform.localPosition.x, localPositionX);
		RectTransform scrollRectMe = this.transform.GetComponent<RectTransform>();
		scrollRectMe.sizeDelta = new Vector2(scrollRectMe.sizeDelta.x, sizeDeltaY);
		refreshShowNode();
	}
}
