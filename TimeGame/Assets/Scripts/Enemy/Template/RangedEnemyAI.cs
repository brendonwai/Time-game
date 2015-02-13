using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemyAI : MonoBehaviour {
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
	public GameObject smoke;
	public GameObject Energy;
	//Set by child script and collider
	//Made public so it's viewable by child
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	bool TargetInSight = false;			//Determines if target is in sight
	float randX;                        // randX and randY is a random coordinate that 
	float randY;                        // the enemy will move towards when the target is not in sight
	float randInterval = 0;             // The interval between the points of time when the enemy changes direction
	float timeCount;                    // Last update for random movement

	public GameObject deadBody;

	bool informedGlobal = false;		//Keeps track if enemy already informed global of player pos
	bool lostSight = true;				//Keeps track if enemy lost sight of player after exiting Trigger
	float stopFollow = 1.0f;			//Sets time after enemy gets LoS'ed that they stop following

	//Efficiency
	GameObject[] children;
	bool OnScreen = false;

	
	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
		timeCount = Time.time;
		foreach(Transform child in transform){
			if(child.gameObject.name == "smoke")
				smoke = child.gameObject;
		}
		smoke.SetActive(false);
	}

	void Start(){
		int count = 0;
		
		children = new GameObject[(transform.childCount-2)];
		foreach(Transform child in transform){
			if(child.gameObject.name != "smoke"&&child.gameObject.name != "Canvas"){
				children[count++] = child.gameObject;
			}
		}
		SetActiveChildren(false);
	}
	
	void OnBecameVisible(){
		SetActiveChildren(true);
	}
	
	void OnBecameInvisible(){
		SetActiveChildren(false);
	}
	
	
	//For efficiency.
	void SetActiveChildren(bool status){
		collider2D.enabled = status;
		OnScreen = status;
		foreach(GameObject child in children)
			child.SetActive(status);
	}


	void Update(){
		if(GetComponent<EnemyInfo>().Health<=25){
			smoke.SetActive(true);
		}
	}


	// Activates when enemy enters trigger collider
	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){
			if(other.gameObject.GetComponentInParent<EnemyInfo>().TargetInSight ||
			   other.gameObject.GetComponentInParent<EnemyInfo>().Alerted){
				GetComponent<EnemyInfo>().Alerted = true;
			}
		}
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = true;
			if(!informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer += 1;
				informedGlobal = true;
				lostSight = false;
			}
		}
	}

	//Deals damage to Player when touches him
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag=="Player"){
			//change amount of damage deal here
			col.gameObject.SendMessage("takeDamage",10);
		}
	}
	
	//Determines what target leaves field of view
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			if(!lostSight){
				lostSight = true;
				StartCoroutine(LostSightManager());
			}
		}
	}

	//For delaying the moment the enemy stops following the player after they exit the trigger zone
	IEnumerator LostSightManager(){
		yield return new WaitForSeconds(stopFollow); //Time to stop pursuit
		if(lostSight){
			GetComponent<EnemyInfo>().TargetInSight = false;
			if(informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer -= 1;
				informedGlobal = false;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer <= 0)
			GetComponent<EnemyInfo>().Alerted = false;

		if(!GetComponent<EnemyInfo>().isHacked&& !anim.GetBool ("IsDead")){
			if (GetComponent<EnemyInfo> ().Health <= 0) { //Delete if statement once destroy() turned on
				Dead ();
			}
			else if(OnScreen && (GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted)){
				ApproachTarget();
			}
			else if(OnScreen){
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
	}
	
	//Moves enemy closer to target
	void ApproachTarget(){
		FaceTarget();
		
		if(!stopMove){
			transform.position = Vector2.MoveTowards(transform.position,
			                                         target.transform.position,
			                                         moveSpeed * Time.deltaTime); 
		}
		if(target.transform.position.y>=transform.position.y)
		{
			anim.SetBool("movingup",true);
		}
		else
		{
			anim.SetBool("movingup",false);
		}
	}

	// Enemy moves towards arbitrary point
	void RandomMovement() {
		FaceTarget ();
		if (!stopMove) {
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(randX, randY), 
			                                         moveSpeed * Time.deltaTime);
			anim.SetFloat ("Speed", 1);
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
		foreach (Transform child in transform) {
			if(child.gameObject.tag == "EnemyStats") {
				Vector3 scale = child.gameObject.transform.localScale;
				scale.x *= -1;
				child.gameObject.transform.localScale = scale;
			}
		}

	}



	//Activates death sequence
	void Dead(){
		anim.SetBool("IsDead", true);
		Vector2 location = transform.position;
		for (int i=0;i<Random.Range(5.0f,15.0f);i++){
			Instantiate(Energy,new Vector2(transform.position.x+Random.Range(-1f,1f),transform.position.y+Random.Range(-1f,1f)),transform.rotation);
		}
		Destroy (gameObject,2.0f);
		Instantiate(deadBody, location, Quaternion.identity);
	}
}
