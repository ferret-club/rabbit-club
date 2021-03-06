﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CountDownTimer : MonoBehaviour {

    private float startTime = 300.0f;
	private float timer;
	private Text me;
	[HideInInspector]
	public bool paused = true;

	// Use this for initialization
	void Start () {
		me = this.GetComponent<Text>();
		paused = true;
		initialize();
	}
	
	// Update is called once per frame
	void Update () {
		timeSettings();
	}

	void timeSettings() {
		if (paused) return;
		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			timer = 0.0f;
			paused = true;
		}
//		me.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", Math.Floor (timer / 3600f), Math.Floor (timer / 60f), Math.Floor (timer % 60f), timer % 1 * 100);
		me.text = string.Format("{0:00}:{1:00}", Math.Floor (timer / 60f), Math.Floor (timer % 60f));
	}

	public void initialize() {
		timer = startTime;
	}

	// タイマーの時刻を取得する
	public float getTimer() {
		return timer;
	}

	public float getResultTime() {
		return startTime - timer;
	}
}
