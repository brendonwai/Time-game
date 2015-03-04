using UnityEngine;
using System.Collections;

public class TemplateEnemyAttack : MonoBehaviour {
	public bool player_in_range;

	void Start() {
		player_in_range = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player") {
			player_in_range = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			player_in_range = false;
		}
	}
/*
	Animator anim;
	bool TargetInRange = false;

	void Awake(){
		anim = GetComponentInParent<Animator>();
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player")
			TargetInRange = true;
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player")
			TargetInRange = false;
			//anim.SetBool("IsAttacking", false);
	}
	 
	void FixedUpdate(){
		if(TargetInRange)
			Attack ();
	}

	void Attack(){
		//anim.SetBool("IsAttacking", true);
		//INSERT ATTACKING CODE
	}
	*/
}
