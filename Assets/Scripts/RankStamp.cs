using UnityEngine;
using System.Collections;

public class RankStamp : MonoBehaviour {

	void Start () {
		Animator anim = this.GetComponent<Animator>();
		anim.SetTrigger("ScaleOutTrigger");
	}
	
}
