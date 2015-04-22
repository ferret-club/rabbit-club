using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ResultTime : MonoBehaviour {

	public float resultTime = 0.0f;
    private Text secText;
    private Text minText;

	void Start() {
        secText = this.transform.FindChild("Sec").GetComponent<Text>();
        minText = this.transform.FindChild("Min").GetComponent<Text>();
		resultTime = DontDestroy.resultTime;
        initialize();
	}
	
    void initialize() {
        minText.text = string.Format("{0:00}", Math.Floor (resultTime / 60f), Math.Floor (resultTime % 60f));
        secText.text = string.Format("{1:00}", Math.Floor (resultTime / 60f), Math.Floor (resultTime % 60f));
	}
}
