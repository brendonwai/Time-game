using UnityEngine;
using System.Collections;

public class MeleeStopApproach : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<MeleeAI>().stopMove = true;
			transform.parent.GetComponent<MeleeAI>().inContact = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<MeleeAI>().stopMove = false;
			transform.parent.GetComponent<MeleeAI>().inContact = false;
		}
	}
}
