
using UnityEngine;
using System.Collections;

public class HealthDroneAI : MonoBehaviour {

	public float moveSpeed = 1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
	public int HealthGain = 25;

	//Set by child script and collider
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	float randX;                        // randX and randY is a random coordinate that 
	float randY;                            // the enemy will move towards when the target is not in sight
	float randInterval = 0;             // The interval between the points of time when the enemy changes direction
	float timeCount;                    // Last update for random movement

	bool isHacked;

	bool informedGlobal = false;

	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
		timeCount = Time.time;
		isHacked = false;
	}



	// Activates when target enters trigger collider
	void OnTriggerEnter2D(Collider2D other){									//Gets alerted if an enemy in the Radius is also alerted or
		if(other.tag == "Enemy"){
			if(other.gameObject.GetComponentInParent<EnemyInfo>().TargetInSight ||
			   other.gameObject.GetComponentInParent<EnemyInfo>().Alerted){
				GetComponent<EnemyInfo>().Alerted = true;
			}
		}
	}
	
	// Activates while target is in trigger collider radius
	void OnTriggerStay2D(Collider2D other){										//Sets TargetInSight to true if It can see the player and lets GlobalEnemyInfo know
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = true;
			if(!informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer += 1;
				informedGlobal = true;
			}
		}
		if(other.tag == "Enemy"){												//Similar to the OnTriggerEnter2D above
			if(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer == 0)
				GetComponent<EnemyInfo>().Alerted = false;
			else
				if(other.gameObject.GetComponentInParent<EnemyInfo>().TargetInSight ||
				  other.gameObject.GetComponentInParent<EnemyInfo>().Alerted){
				GetComponent<EnemyInfo>().Alerted = true;	//Not redundant. Requires multiple
				//enemy alert states
			}
		}
	}
	
	//Determines what target leaves field of view
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = false;
			if(informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer -= 1;
				informedGlobal = false;
			}
		}
		if(other.tag == "Enemy"){
			if(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer == 0)
				GetComponent<EnemyInfo>().Alerted = false;
		}
	}

	
	//Deals damage to Player when touches him
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag=="Player"){
			//change amount of damage deal here
			col.gameObject.SendMessage("takeDamage", 10);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!anim.GetBool("isHacked")) {
			if (GetComponent<EnemyInfo> ().Health <= 0) { //Delete if statement once destroy() turned on
				Dead ();
			}
			else if(GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted){
				RunAway();		//Flee from Player if in sight/alerted
			}
			else{
				if (Time.time - timeCount >= randInterval){
					// Creates a random target point in an arbitrary rectangle to move towards
					randX = Random.Range(-750,750);
					randY = Random.Range(-600,600);
					randInterval = Random.Range(1.0f, 2.5f);
					timeCount = Time.time;
				}
				else{
					RandomMovement();
				}
			}
		}
		else {
			if (!isHacked) {				//So that it heals only once
				StartCoroutine(HealPlayer());
			}
		}
	}

	IEnumerator HealPlayer() {		//Heals the player
		isHacked = true;
		if (target.GetComponent<PlayerInfo> ().Health >= (100 - HealthGain)) {	//Limits Heal to Max HP
			target.GetComponent<PlayerInfo> ().Health = 100;
		}
		else {			//Heal for flat value, HealthGain
			target.GetComponent<PlayerInfo> ().Health += HealthGain;
		}
		target.GetComponent<PlayerInfo> ().healthBar.value = target.GetComponent<PlayerInfo> ().Health;
		target.GetComponent<PlayerInfo> ().healthNum.text = target.GetComponent<PlayerInfo> ().Health.ToString();
		yield return new WaitForSeconds (0.45f);
		Dead ();		//Dies after it is hacked
	}
	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
		//reduce health by amount of damage
		GetComponent<EnemyInfo>().Health -= damage;
		//sprite flashes red upon taking damage
		renderer.material.color = Color.red;
		yield return new WaitForSeconds (.1f);
		renderer.material.color=Color.white;
	}

	//Moves enemy closer to target
	void RunAway(){
		FaceAwayFromTarget();		//Faces away from player
		if(!stopMove){
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - (target.transform.position.x - transform.position.x),		//Runs in opposite direction of Player
			                                                                         transform.position.y - (target.transform.position.y - transform.position.y)),
			                                                                         moveSpeed * Time.deltaTime);
			//anim.SetFloat ("Speed", 1);		//Tells animator enemy is moving. Only need if you have an Idle animation
			
		}
	}
	
	// Enemy moves towards arbitrary point
	void RandomMovement() {
		FaceAwayFromTarget();
		if (!stopMove) {
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(randX, randY), 
			                                         moveSpeed * Time.deltaTime);
			//anim.SetFloat ("Speed", 1);	//Again only need if you have an Idle animation
		}
	}
	
	//Faces enemy away from target.
	void FaceAwayFromTarget(){
		if((target.transform.position.x >= transform.position.x) && facingRight)
			Flip();
		if((target.transform.position.x < transform.position.x) && !facingRight)
			Flip ();
	}
	
	//Flips enemy sprite to face direction of movement
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void Dead() {
		anim.SetBool("isDead", true);
		Destroy (gameObject, 0.3f);
	}
}
