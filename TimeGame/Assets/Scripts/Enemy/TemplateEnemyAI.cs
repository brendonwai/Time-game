using UnityEngine;
using System.Collections;

public class TemplateEnemyAI : MonoBehaviour {
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public float closeEnough = 0.9f;	//Determines when enemy is close enough to
											//to player to stop moving towards player
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	bool TargetInSight = false;					//Determines if target is in sight

	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	// Activates when target enters trigger collider
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player")
			TargetInSight = true;
	}

	//Determines what target leaves field of view
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			TargetInSight = false;
			anim.SetFloat("Speed", 0);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(TargetInSight){
			ApproachTarget();
		}
	}

	//Moves enemy closer to target
	void ApproachTarget(){
		float distance = Vector2.Distance(transform.position,target.transform.position);

		FaceTarget();
	
		if(distance > closeEnough){
			transform.position = Vector2.MoveTowards(transform.position,
			                                         target.transform.position,
			                                         moveSpeed * Time.deltaTime); 
			anim.SetFloat ("Speed", 1);		//Tells animator enemy is moving	
		}
	}

	//Faces enemy towards target
	void FaceTarget(){
		if((target.transform.position.x >= transform.position.x) && !facingRight)
			Flip();
		if((target.transform.position.x < transform.position.x) && facingRight)
			Flip ();
	}

	//Flips enemy sprite to face direction of movement
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	
}
