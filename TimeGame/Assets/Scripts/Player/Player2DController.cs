using UnityEngine;
using System.Collections;

public class Player2DController : MonoBehaviour {

	public float maxSpeed = 5f;			//Sets momvement speed
	public bool facingLeft = true;		//Determines direction character facing
	public int hackState = -1;			//Determines action of player depending on hacked enemy
	public Rigidbody2D rangedBullet;	//Bullet from basic ranged enemy
	int bulletspeed=10;					//Speed of bullet
	float timestamp=0.0f;				//Variable for managing bullet release
	public GameObject PushBack;
	public GameObject Explosion;		// explosion prefab for hacked kamikaze robot death
	Animator anim;						//Animation object
	
	public bool GameOver = false;		//For activating game over 
	bool death = false;					//Determines if player is dead
	
	float KnockBackForce = 500;			//Knockback distance
	
	float attackRate=1.0f;				//Attack handlers
	float nextAttack=0;

	public bool invincible = false;		//Makes player invincible
	float invinTime = 1.0f;				//Sets time player is invincible for

	public bool inHackingAnim;			//If the player is in the middle of the Hacking animation so you don't move or change your direction while it's playing.
	public bool paused = false;

	public AudioClip SoundTakeDamage;

	private AudioSource SoundSource;

	private GameObject spawnRoom;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		PushBack.SetActive (false);
		SoundSource = GetComponent<AudioSource>();
		spawnRoom = GameObject.FindGameObjectWithTag("SpawnRoom");
		transform.position = spawnRoom.transform.position;
	}

	void FixedUpdate () {
		if (!death&&!paused){
			//Directional command handlers
			float move_x = Input.GetAxis ("Horizontal");    //Get input for x-axis
			float move_y = Input.GetAxis ("Vertical");      //Get input for y-axis
			
			//Changes value of Speed
			//*Speed defined in Unity Animator Controller
			anim.SetFloat ("Speed", Mathf.Abs (move_x));
			anim.SetFloat ("YSpeed", Mathf.Abs (move_y));

			//Makes character move based on inputs
			if (!inHackingAnim) {		//So you don't move or change direction while hacking.
				rigidbody2D.velocity = new Vector2 (move_x * maxSpeed, move_y * maxSpeed);
				/*
			 	* If moving in the positive x direction (right) and the character is not
			 	* facing right, makes the character sprite flip to face the right.
			 	*/
				if((move_x < 0) && !facingLeft){
					Flip();
				}
				else if((move_x > 0) && facingLeft){
					Flip();
				}
			}
		}
	}

	void Update(){
		if(!death&&!paused)
			HackStateActions();
	}

	//Handles player attack in default state
	void PlayerAttack(){
		if(Input.GetMouseButtonDown(0) && Time.time>=nextAttack && !anim.GetBool("IsAttacking")){
			nextAttack+=attackRate;
			StartCoroutine("Attack");
		}
	}

	IEnumerator Attack(){
		anim.SetBool ("IsAttacking", true);
		yield return new WaitForSeconds (.15f);
		PushBack.SetActive (true);
		yield return new WaitForSeconds (.1f);
		PushBack.SetActive (false);
		yield return new WaitForSeconds (.15f);
		anim.SetBool ("IsAttacking", false);
	}
	

	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
		
		if(!death && !invincible){
			//trigger camera shake.
			//shake magnitude based on damage taken
			SoundSource.PlayOneShot(SoundTakeDamage,.7f);
			SendMessage ("CamShakeOnDamage", damage);
			//sprite flashes red upon taking damage
			renderer.material.color = Color.red;
			yield return new WaitForSeconds(.1f);
			renderer.material.color = Color.white;
			//reduce health by amount of damage
			if (anim.GetBool ("IsHackingEnemy")){			//If you are controlling a hacked enemy
				SubtractHealth(damage);
				if (GetComponent<PlayerInfo> ().Health <= 0){
					//trigger camera shake
					SendMessage ("CamShakeOnDamage", damage + 50);		//Not sure if we should have this here
					StartCoroutine(HackDeath());
				}
			}
			else {											//If you are human
				SubtractHealth(damage);
				if (GetComponent<PlayerInfo> ().Health <= 0){
					//trigger camera shake
					SendMessage ("CamShakeOnDamage", damage + 50);
					StartCoroutine(PlayerDeath());
				}
			}
			
			invincible = true;
			yield return new WaitForSeconds(invinTime);		//Temporarily makes player invulnerable
			invincible = false; 
		}
	}

	void SubtractHealth(int damage) {
		GetComponent<PlayerInfo> ().Health -= damage;
		if (GetComponent<PlayerInfo> ().Health < 0) {
			GetComponent<PlayerInfo> ().Health = 0;
		}
		GetComponent<PlayerInfo> ().healthBar.value = GetComponent<PlayerInfo> ().Health;
		GetComponent<PlayerInfo> ().healthNum.text = GetComponent<PlayerInfo> ().Health.ToString();
	}

	void KnockBack(Vector2 dir){
		if(!invincible && !inHackingAnim)
			rigidbody2D.AddForce (dir.normalized * KnockBackForce);
	}

	IEnumerator PlayerDeath(){
		death = true;
		gameObject.rigidbody2D.isKinematic = true;	//Prevents enemy from pushing player after death
		rigidbody2D.velocity = new Vector2 (0,0);
		anim.SetBool("IsDead",true);
		yield return new WaitForSeconds(2.583f);
		GameOver = true;
	}

	public IEnumerator HackDeath () {
		if(hackState==0)
			Instantiate(Explosion,transform.position,transform.rotation);
		if(hackState==0)
			HackFlip ();				//This is here because of sprite direction differences
		rigidbody2D.velocity = new Vector2 (0,0);
		anim.SetBool ("HackedEnemyDead", true);
		anim.SetBool ("IsHackingEnemy", false);
		anim.SetInteger ("EnemyType", -1);
		anim.SetBool("IsAttacking", false);
		hackState = -1;
		timestamp = 0.0f;
		if (GetComponent<PlayerInfo> ().HealthDrainActive) {
			GetComponent<PlayerInfo> ().HealthDrainActive = false;
			StopCoroutine(GetComponent<PlayerInfo>().HealthDrain());
		}
		GetComponent<PlayerInfo> ().SwapToPreHackHealth();
		GetComponent<PlayerInfo> ().healthBar.value = GetComponent<PlayerInfo> ().Health;
		GetComponent<PlayerInfo> ().healthNum.text = GetComponent<PlayerInfo> ().Health.ToString();
		yield return new WaitForSeconds(.1f);
		anim.SetBool ("HackedEnemyDead", false);
		
		invincible = true;
		yield return new WaitForSeconds(invinTime);		//Temporarily makes player invulnerable
		invincible = false; 
	}

	//Flips character sprite to face direction of movement
	public void Flip() {
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		GetComponentInChildren<AntiFlip> ().Flip();		//Changes direction of hacksprite
	}
	public void HackFlip() {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//Sets player moveset based on what enemy currently hacked if any.
	void HackStateActions(){
		switch(hackState){

			//BASIC ENEMY
			case 0:
				break;
			//BASIC RANGED ENEMY
			case 1:
				anim.SetBool("IsAttacking", true);
				if (Time.time>=timestamp){
					RangedEnemyShoot ();
					timestamp=Time.time+.5f;
					}
				break;
			default:
				PlayerAttack();
				break;
			      
		}
	}
	

	//Mimics ranged enemy shooting
	void RangedEnemyShoot(){
		Vector2 bulletClonePos = transform.position;
		bulletClonePos.x -= .1f;
		Vector2 bulletClone2Pos = transform.position;
		bulletClone2Pos.x += .1f;
		Rigidbody2D bulletClone=Instantiate (rangedBullet, bulletClonePos, transform.rotation) as Rigidbody2D;
		Rigidbody2D bulletClone2=Instantiate (rangedBullet, bulletClone2Pos, transform.rotation) as Rigidbody2D;

		Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 dir = (mousePos- bulletClonePos).normalized;
		Vector2 dir2 = (mousePos - bulletClone2Pos).normalized;

		bulletClone.AddForce (dir*bulletspeed);
		bulletClone2.AddForce (dir2*bulletspeed);
		bulletClone.renderer.material.color = Color.red;
		bulletClone2.renderer.material.color = Color.red;
	}
}