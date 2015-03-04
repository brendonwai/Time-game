using UnityEngine;
using System.Collections;

public class TrapHole : MonoBehaviour {

	Animator anim;

	bool TrapSet = false;				//Determines if trap should begin sequence
	int FollowerCount = 0;				//Determines how many traps the trap trigger is set to Lead
	GameObject[] Followers;				//Keeps track of follow traps
	public bool Leader = true;			//Determines if leader



	void Start(){
		anim = GetComponent<Animator>();
		if(Leader){

		}

	}

	
	void OnTriggerStay2D(Collider2D other){
		SetTrap(other);
		if(TrapSet){
			ApplyDamage(other);
		}
	}

	//For chaining traps
	public void ChainTrap(){
		TrapSet = true;
	}

	void TrapLeader(){

	}

	//Sets trap to activate if stepped on
	void SetTrap(Collider2D other){
		if(other.tag == "Player" || other.tag =="Enemy"&& !TrapSet){
			TrapSet = true;
			anim.SetBool("IsOpening", true);
		}
	}

	//Applies damage to player and/or enemy
	void ApplyDamage(Collider2D other){
		if(other.tag == "Player"){
			other.gameObject.SendMessage("takeDamage",3);
			Vector2 dir=other.GetComponent<Rigidbody2D>().velocity;
			other.SendMessage("KnockBack",dir);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
