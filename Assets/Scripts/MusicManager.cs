using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip mainBgm;
	private AudioSource audioSource;

	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = mainBgm;
	}
	
	void Update () {
	
	}

	public void PlayBgm() {
		audioSource.Play();
	}

}
