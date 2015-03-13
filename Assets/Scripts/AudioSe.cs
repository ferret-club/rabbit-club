using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AudioSe : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

	public AudioClip audioClip;
	private AudioSource audioSource;

	void Start() {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClip;
	}
	
	void Update() {
	
	}

	public void OnPointerDown (PointerEventData eventData)
	{
//		Debug.Log ( "OnPointerDown : ");
	}

	public void OnPointerUp (PointerEventData eventData)
	{
//		Debug.Log ( "OnPointerUp : ");
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		audioSource.Play();
	}

}
