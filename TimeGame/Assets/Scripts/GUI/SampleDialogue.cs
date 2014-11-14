using UnityEngine;
using System.Collections;

public class SampleDialogue : MonoBehaviour {

	public GameObject dialogue;
	bool display = false;

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("TESFSDFSd");
		if(other.tag == "Player"){
			display = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player"){
			display = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player")
			display = false;
	}

	void Update(){
		if(display)
			dialogue.renderer.enabled = true;
		else
			dialogue.renderer.enabled = false;
	}
}
