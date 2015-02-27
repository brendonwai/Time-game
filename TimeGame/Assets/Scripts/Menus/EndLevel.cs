using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player") {
			ProgressInfo.ChangeLevel(true, 1);	//Increments the level by one
		}
	}
}
