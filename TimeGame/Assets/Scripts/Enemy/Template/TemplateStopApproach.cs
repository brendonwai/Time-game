using UnityEngine;
using System.Collections;

public class TemplateStopApproach : MonoBehaviour {
	//Used to stop enemy from approaching

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<TemplateEnemyAI>().stopMove = true;
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<TemplateEnemyAI>().stopMove = true;
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<TemplateEnemyAI>().stopMove = false;
		}
	}
}
