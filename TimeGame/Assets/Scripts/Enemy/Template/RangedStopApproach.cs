using UnityEngine;
using System.Collections;

public class RangedStopApproach : MonoBehaviour {
	//Used to stop enemy from approaching
	
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<RangedEnemyAI>().stopMove = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			transform.parent.GetComponent<RangedEnemyAI>().stopMove = false;
		}
	}
}
