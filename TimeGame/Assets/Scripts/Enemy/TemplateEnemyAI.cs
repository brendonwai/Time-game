using UnityEngine;
using System.Collections;

public class TemplateEnemyAI : MonoBehaviour {
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public float detectRadius = 1f;		//Sets radius of enemy detection
	public float attackRadius = 0.5f;	//Sets when enemy starts attacking
	public float closeEnough = 0.9f;	//Determines when enemy is close enough to
											//to player to stop moving towards player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target

	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ApproachTarget();
	}

	//Moves enemy closer to target
	void ApproachTarget(){
		float distance = Vector2.Distance(transform.position,target.transform.position);

		if(distance > detectRadius){
			anim.SetFloat("Speed", 0);
		}
		else
		{
			FaceTarget();
			if(distance > closeEnough){
				transform.position = Vector2.MoveTowards(transform.position, 
				                                         target.transform.position,
				                                         moveSpeed * Time.deltaTime); 
				anim.SetFloat ("Speed", 1);		//Tells animator enemy is moving
				anim.SetBool("IsAttacking", false);
				
			}
		}
		Attack(distance);
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

	//Determines enemies attack actions
	void Attack(float howFar){
		if(howFar <= attackRadius)
		{
			anim.SetBool("IsAttacking", true);
			//INSERT ATTACKING CODE HERE
		}
		else
			anim.SetBool("IsAttacking", false);
	}
}
