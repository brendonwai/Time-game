using UnityEngine;
using System.Collections;

public class TemplateEnemyAttack : MonoBehaviour {

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
			anim.SetBool("IsAttacking", false);
	}
	 
	void FixedUpdate(){
		if(TargetInRange)
			Attack ();
	}

	void Attack(){
		anim.SetBool("IsAttacking", true);
		//INSERT ATTACKING CODE
	}
}
