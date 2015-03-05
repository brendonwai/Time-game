using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemplateEnemyAI : MonoBehaviour {
	public GameObject Energy;
	
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
											//Set by child script and collider

	//Explosion
	public GameObject explosionRange;	//Explosion Range
	public GameObject explosion;		//Particle

	private float first_half_explosion_charge_time = 0.5f;	//Half because it'll check halfway through if the player is still in range
	private float second_half_explosion_charge_time = 0.3f;	//Half because it'll check halfway through if the player is still in range

	private bool player_in_explosion_range;

	CircleCollider2D detectRadius;		//Sets when enemy detects player
	public bool facingRight = false;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	float randX;                        // randX and randY is a random coordinate that 
	float randY;                            // the enemy will move towards when the target is not in sight
	float randInterval = 0;             // The interval between the points of time when the enemy changes direction
	float timeCount;                    // Last update for random movement
	bool alive = true;

	bool informedGlobal = false;
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
		explosionRange.SetActive (true);
		player_in_explosion_range = false;
	}

	void Start(){
		int count = 0;

		children = new GameObject[transform.childCount-2];
		foreach(Transform child in transform){
			if(child.gameObject.name != "AttackRadius"&&child.gameObject.name != "Canvas")
				children[count++] = child.gameObject;
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
		OnScreen = status;
		GetComponent<Collider2D>().enabled = status;
		foreach(GameObject child in children)
			child.SetActive(status);
	}
	
	// Activates when target enters trigger collider
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

	//Determines what target leaves field of view
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player"){
			if(!lostSight){
				lostSight = true;
				StartCoroutine(LostSightManager());
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
			StartCoroutine(Explode ());
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

		if (!GetComponent<EnemyInfo>().isHacked) {
			if (GetComponent<EnemyInfo> ().Health <= 0 && alive) {
				alive = false;
				Dead ();
			}
			else if((GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted) && OnScreen){
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

	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
		Debug.Log("done");
		//reduce health by amount of damage
		GetComponent<EnemyInfo> ().Health -= damage;
		GetComponent<EnemyInfo> ().healthBar.value = GetComponent<EnemyInfo> ().Health;

		//sprite flashes red upon taking damage
		GetComponent<Renderer> ().material.color = Color.red;
		yield return new WaitForSeconds (.1f);
		GetComponent<Renderer> ().material.color=Color.white;
		if(GetComponent<EnemyInfo> ().Health<=0)
			Dead ();
	}

	//Moves enemy closer to target
	void ApproachTarget(){
		FaceTarget();
	
		if(!stopMove){
			transform.position = Vector2.MoveTowards(transform.position,
			                                         target.transform.position,
			                                         moveSpeed * Time.deltaTime); 
			anim.SetBool ("IsMoving", true);		//Tells animator enemy is moving	

		}
	}

	// Enemy moves towards arbitrary point
	void RandomMovement() {
		FaceTarget ();
		if (!stopMove) {
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(randX, randY), 
			                                         moveSpeed * Time.deltaTime);
			anim.SetBool ("IsMoving", true);
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

	IEnumerator Check_Before_Explode() {
		if (GetComponent<EnemyInfo> ().isHacked) {	//If the Kamikaze was hacked which caused the Death
			StartCoroutine(Explode());
		}
		else {
			stopMove = true;

			//PLAY SOME SIZZLE SOUND HERE OR SOMETHING

			yield return new WaitForSeconds (first_half_explosion_charge_time);					//Wait a bit

			if (explosionRange.GetComponent<TemplateEnemyAttack> ().player_in_range) {		//If Player is IN RANGE of the explosion

				yield return new WaitForSeconds (second_half_explosion_charge_time);

				StartCoroutine(Explode());

			}
			else {																			//If Player LEFT the explosion range then go on as normal
				stopMove = false;
			}
		}		
	}

	IEnumerator Explode(){

		target.GetComponent<Player2DController> ().SendMessage("takeDamage", 100);

		yield return new WaitForSeconds(0.1f);

		anim.SetBool("IsDead", true);

		stopMove = true;

		//Create Energy Balls when Dead
		for (int i = 0; i < Random.Range(5.0f, 15.0f); i++){
			Instantiate(Energy, new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f)), transform.rotation);
		}

		Instantiate (explosion, transform.position, transform.rotation);	//Explosion Animation
		Destroy (gameObject, 0.375f);										//Destory Kamikaze After Explosion
	}



	void Dead() {
		if (!alive) {	//If it died because it has <= 0 HP
			StartCoroutine(Explode());
		}
		else {
			StartCoroutine(Check_Before_Explode ());
		}
	}
}
