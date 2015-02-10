using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemplateEnemyAI : MonoBehaviour {
	public GameObject Energy;
	public GameObject explosionRange;
	public GameObject explosion;
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
											//Set by child script and collider
	CircleCollider2D detectRadius;		//Sets when enemy detects player
	public bool facingRight = true;			//Determines direction enemy facing
	Animator anim;						//For controlling animation
	GameObject target;					//Enemy's target
	float randX;                        // randX and randY is a random coordinate that 
	float randY;                            // the enemy will move towards when the target is not in sight
	float randInterval = 0;             // The interval between the points of time when the enemy changes direction
	float timeCount;                    // Last update for random movement
	bool alive=true;
	bool informedGlobal = false;

	//Efficiency
	GameObject[] children;

	// Use this for initialization
	void Awake () {
		target = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
		timeCount = Time.time;
		explosionRange.SetActive (false);
	}

	void Start(){
		int count = 0;

		children = new GameObject[transform.childCount];
		foreach(Transform child in transform){
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
		collider2D.enabled = status;
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
	}

	// Activates while target is in trigger collider radius
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player"){
			GetComponent<EnemyInfo>().TargetInSight = true;
			if(!informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer += 1;
				informedGlobal = true;
			}
		}
		if(other.tag == "Enemy"){
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
			col.gameObject.SendMessage("takeDamage",10);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!GetComponent<EnemyInfo>().isHacked) {
			if (GetComponent<EnemyInfo> ().Health <= 0 && alive) {
				alive=false;
				Dead ();
			}
			else if(GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted){
				ApproachTarget();
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
	}

	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
		Debug.Log("done");
		//reduce health by amount of damage
		GetComponent<EnemyInfo>().Health -= damage;
		GetComponent<EnemyInfo> ().healthBar.value = GetComponent<EnemyInfo> ().Health;

		//sprite flashes red upon taking damage
		renderer.material.color = Color.red;
		yield return new WaitForSeconds (.1f);
		renderer.material.color=Color.white;
		if(GetComponent<EnemyInfo>().Health<=0)
			StartCoroutine("Dead");
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
	}

	IEnumerator Explode(){
		explosionRange.SetActive (true);
		yield return new WaitForSeconds (.1f);
		if(!GetComponent<EnemyInfo>().isHacked)
			explosionRange.SetActive (false);
	}

	void Dead(){
		anim.SetBool("IsDead", true);
		
		for (int i=0;i<Random.Range(5.0f,15.0f);i++){
			Instantiate(Energy,new Vector2(transform.position.x+Random.Range(-1f,1f),transform.position.y+Random.Range(-1f,1f)),transform.rotation);
		}
		
		StartCoroutine (Explode());
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy (gameObject,.375f);
	}
}
