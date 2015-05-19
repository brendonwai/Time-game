using UnityEngine;
using System.Collections;

public abstract class EnemyAI : MonoBehaviour {
	
	public float moveSpeed = 0.1f;	 	//Sets momvement speed
	public bool stopMove = false;		//Determines if enemy can stop moving towards player
											//Set by child script and collider

	CircleCollider2D detectRadius;		//Sets when enemy detects player
	public bool facingRight = false;			//Determines direction enemy facing
	protected Animator anim;						//For controlling animation
	protected GameObject target;					//Enemy's target
	float randX;                        // randX and randY is a random coordinate that 
	float randY;                            // the enemy will move towards when the target is not in sight
	float randInterval = 0;             // The interval between the points of time when the enemy changes direction
	float timeCount;                    // Last update for random movement
	protected bool alive = true;

	bool informedGlobal = false;
	bool lostSight = true;				//Keeps track if enemy lost sight of player after exiting Trigger
	float stopFollow = 1.0f;			//Sets time after enemy gets LoS'ed that they stop following

	//Efficiency
	GameObject[] children;
	bool OnScreen = false;

	// Use this for initialization
	protected virtual void Awake () {
		anim = GetComponent<Animator>();
		timeCount = Time.time;
	}

	protected virtual void Start(){
		target = GameObject.FindGameObjectWithTag("Player");
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
	protected virtual void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){
			if(other.gameObject.GetComponentInParent<EnemyInfo>().TargetInSight ||
			   other.gameObject.GetComponentInParent<EnemyInfo>().Alerted){
				GetComponent<EnemyInfo>().Alerted = true;
			}
		}
		if(other.tag == "Player") {
			GetComponent<EnemyInfo>().TargetInSight = true;
			if(!informedGlobal){
				GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer += 1;
				informedGlobal = true;
				lostSight = false;
			}
		}
	}

	//Determines what target leaves field of view
	protected virtual void OnTriggerExit2D(Collider2D other){
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

	protected virtual void OnCollisionEnter2D(Collision2D col){
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
	protected virtual void FixedUpdate () {
		if(GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer <= 0)
			GetComponent<EnemyInfo>().Alerted = false;

		if (!GetComponent<EnemyInfo>().isHacked) {
			if (GetComponent<EnemyInfo> ().Health <= 0 && alive) {
				alive = false;
				Dead ();
			}
			else if((GetComponent<EnemyInfo>().TargetInSight || GetComponent<EnemyInfo>().Alerted) && OnScreen){
				Move();
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

	protected virtual void Move() {
		ApproachTarget();
	}

	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
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

	public GameObject GetPlayer() {
		return target;
	}

	//Moves enemy closer to target
	protected void ApproachTarget(){
		FaceTarget(target.transform.position);
	
		if(!stopMove){
			transform.position = Vector2.MoveTowards(transform.position,
			                                         target.transform.position,
			                                         moveSpeed * Time.deltaTime); 
			anim.SetBool ("IsMoving", true);		//Tells animator enemy is moving	

		}
	}

	// Enemy moves towards arbitrary point
	protected virtual void RandomMovement() {
		Vector2 tempPos = new Vector2(randX, randY);
		FaceTarget(tempPos);
		if (!stopMove) {
			transform.position = Vector2.MoveTowards(transform.position, tempPos, 
			                                         moveSpeed * Time.deltaTime);
			anim.SetBool("IsMoving", true);
		}
	}

	//Faces enemy towards target
	void FaceTarget(Vector2 pos){
		if((pos.x >= transform.position.x) && !facingRight)
			Flip();
		if((pos.x < transform.position.x) && facingRight)
			Flip ();
	}

	//Flips enemy sprite to face direction of movement
	public void Flip() {
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

	protected virtual void Dead() {
		if (!alive) {	//If it died because it has <= 0 HP
			anim.SetBool("IsDead", true);
		}
	}
}
