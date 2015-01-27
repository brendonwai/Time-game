using UnityEngine;
using System.Collections;

public class HealthDroneStopApproach : MonoBehaviour {
	//Used to stop enemy from approaching player any further
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<HealthDroneAI>().stopMove = true;
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<HealthDroneAI>().stopMove = true;
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<HealthDroneAI>().stopMove = false;
		}
	}
}
