using UnityEngine;
using System.Collections;

public class TemplateEnemyAI : MonoBehaviour {
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
											//Set by child script and collider
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target

	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	// Activates when target enters trigger collider
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = true;
		}
		if(other.tag == "Enemy"){
			GetComponent<EnemyInfo>().Alerted = (other.gameObject.GetComponent<EnemyInfo>().TargetInSight ||
			                                     other.gameObject.GetComponent<EnemyInfo>().Alerted);
		}
	}

	// Activates while target is in trigger collider radius
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player")
			GetComponent<EnemyInfo>().TargetInSight = true;
		if(other.tag == "Enemy"){
			if(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer == 0)
				GetComponent<EnemyInfo>().Alerted = false;
			else
				GetComponent<EnemyInfo>().Alerted = (other.gameObject.GetComponent<EnemyInfo>().TargetInSight ||
			                                         other.gameObject.GetComponent<EnemyInfo>().Alerted);
		}
	}

	//Determines what target leaves field of view
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = false;
		}
		if(other.tag == "Enemy"){
			GetComponent<EnemyInfo>().Alerted = false;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		ReportToGlobal();
		Debug.Log(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer);
		if(GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted){
			ApproachTarget();
		}
		else{
			//Random movement script reference here?
			anim.SetFloat("Speed",0);	//Defaults to setting Idle state
		}
	}

	//Moves enemy closer to target
	void ApproachTarget(){
		FaceTarget();
	
		if(!stopMove){
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

	//Reports if enemy can see player or not
	void ReportToGlobal(){
		if(GetComponent<EnemyInfo>().TargetInSight)
			GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer +=1;
	}
}
