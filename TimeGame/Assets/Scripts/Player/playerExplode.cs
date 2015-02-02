using UnityEngine;
using System.Collections;

public class playerExplode : MonoBehaviour {

	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag=="Enemy"){
			other.transform.parent.SendMessage("takeDamage",100);
		}
	}
}
