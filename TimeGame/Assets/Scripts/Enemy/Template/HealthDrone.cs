using UnityEngine;
using System.Collections;

public class HealthDrone : MonoBehaviour {
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
	//Set by child script and collider
	//Made public so it's viewable by child
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	bool TargetInSight = false;			//Determines if target is in sight
	float timeCount = 0;				//Keeps track of time for random unit pathing
	float timeCount2 = 0;				//Keeps track of time for Alert status
	// replace these with the bounds of the room
	float randx = Random.value;
	float randy = Random.value;
	bool alert;							//Determines whether unit is on alert or not
	
	
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
	
	// Activates while target is in trigger collider radius
	void OnTriggerStay2D(Collider2D other){
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
		
		if (TargetInSight) {
			alert = true;
			timeCount2 = Time.time;
		}
		if (Time.time - timeCount2 >= 1.5) {
			alert = false;
		}
		if (alert == true) {
			FleeTarget ();
		}
		else{
			
			if (Time.time - timeCount >= 1.5){
				//replace these with the bounds of the room
				randx = Random.Range(-750,750);
				randy = Random.Range(-600,600);
				timeCount = Time.time;
			}
			else{
				RandomMovement (randx, randy);
			}
		}
	}
	
	//Moves enemy away from target
	void FleeTarget(){
		FaceTarget();
		
		if(!stopMove){
			transform.position = Vector2.MoveTowards(transform.position,
			                                         target.transform.position,
			                                         -moveSpeed * Time.deltaTime); 
			anim.SetFloat ("Speed", 1);		//Tells animator enemy is moving	
		}
	}
	
	void RandomMovement(float randX, float randY){
		FaceTarget ();
		if (!stopMove) {
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(randX, randY), 
			                                         moveSpeed * Time.deltaTime);
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
