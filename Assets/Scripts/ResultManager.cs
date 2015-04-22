using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {

	public GameObject successPanel;
	public GameObject failedPanel;

	void Start() {
		if (DontDestroy.successFlg) {
			successPanel.SetActive(true);
			failedPanel.SetActive(false);
		} else {
			successPanel.SetActive(false);
			failedPanel.SetActive(true);
		}
	}
}
