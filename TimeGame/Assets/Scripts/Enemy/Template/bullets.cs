using UnityEngine;
using System.Collections;

public class bullets : MonoBehaviour {
	public int damage = 10;

	void Start(){
		StartCoroutine(SelfDestruction());
	}
	// deal damage to collided target depending on color of bullet
	// enemy shoot blue bullets --> damage player
	// player hacked robots shoot red bullets --> damage enemies
	// destroy 
	void OnTriggerEnter2D(Collider2D other){
		//Tag obstacles as Environment to destroy bullets on contact
		if(other.tag=="Background"||other.tag=="Gate"){
			Destroy(gameObject);
		}
		if (other.tag=="Player"){
			if (renderer.material.color==Color.cyan){
				other.SendMessage("takeDamage",damage);
				Vector2 dir=rigidbody2D.velocity;
				other.SendMessage("KnockBack",dir);
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


	IEnumerator SelfDestruction() {
		yield return new WaitForSeconds(1);
		Destroy (gameObject, 2);
	}



}
