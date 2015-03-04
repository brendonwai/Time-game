using UnityEngine;
using System.Collections;

public class SampleDialogue : MonoBehaviour {

	public GameObject dialogue;
	bool display = false;

	void OnTriggerEnter2D(Collider2D other){
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
			dialogue.GetComponent<Renderer>().enabled = true;
		else
			dialogue.GetComponent<Renderer>().enabled = false;
	}
}
