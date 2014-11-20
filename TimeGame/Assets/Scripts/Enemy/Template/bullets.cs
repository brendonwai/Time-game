using UnityEngine;
using System.Collections;

public class bullets : MonoBehaviour {
	public int damage = 10;

	void Start(){

	}
	// deal damage to collided target depending on color of bullet
	// enemy shoot blue bullets --> damage player
	// player hacked robots shoot red bullets --> damage enemies
	// destroy 
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag=="Environment"){
			Destroy(gameObject);
		}
		if (other.tag=="Background"){
			Destroy (gameObject);
		}
		if (other.tag=="Player"){
			if (renderer.material.color==Color.blue){
				other.SendMessage("takeDamage",damage);
				Destroy (gameObject);
			}
		}
		if (other.tag=="Enemy"){
			if (renderer.material.color==Color.red){
				other.SendMessage("takeDamage",damage);
				Destroy (gameObject);
			}
		}

	}


	void OnBecameInvisible(){
			Destroy (gameObject);
	}
}
