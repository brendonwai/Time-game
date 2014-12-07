using UnityEngine;
using System.Collections;

public class Player2DController : MonoBehaviour {

	public float maxSpeed = 5f;			//Sets momvement speed
	public bool facingLeft = true;		//Determines direction character facing
	Animator anim;						//Animation object
	bool death=false;
	float KnockBackForce=500;
	public bool GameOver = false;


	bool invincible = false;			//Makes player invincible
	float invinTime = 1.0f;				//Sets time player is invincible for
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		if (!death){

			//Directional command handlers
			float move_x = Input.GetAxis ("Horizontal");    //Get input for x-axis
			float move_y = Input.GetAxis ("Vertical");      //Get input for y-axis
			
			//Changes value of Speed
			//*Speed defined in Unity Animator Controller
			anim.SetFloat ("Speed", Mathf.Abs (move_x));
			anim.SetFloat ("YSpeed", Mathf.Abs (move_y));

			//Makes character move based on inputs
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
		if(invincible)
			StartCoroutine(InvincibilityFlash());
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Z)){
			StartCoroutine("Attack");
		}
	}

	IEnumerator Attack(){
		anim.SetBool ("IsAttacking", true);
		yield return new WaitForSeconds (.4f);
		anim.SetBool ("IsAttacking", false);
	}

	//Makes player flash while in invincible state
	IEnumerator InvincibilityFlash(){
		renderer.material.color=Color.cyan;
		yield return new WaitForSeconds(invinTime);	
		renderer.material.color = Color.white;
	}
	
	
	
	// Call this when damage dealt to enemy
	IEnumerator takeDamage(int damage){
		
		if(!death && !invincible){
			//trigger camera shake.
			//shake magnitude based on damage taken
			SendMessage ("CamShakeOnDamage", damage);
			//sprite flashes red upon taking damage
			renderer.material.color = Color.red;
			yield return new WaitForSeconds(.1f);
			renderer.material.color=Color.white;
			//reduce health by amount of damage
			GetComponent<PlayerInfo>().Health -= damage;
			GetComponent<PlayerInfo> ().healthBar.value = GetComponent<PlayerInfo> ().Health;
			if (GetComponent<PlayerInfo>().Health<=0){
				GetComponent<PlayerInfo>().Health=0;
				//trigger camera shake
				SendMessage ("CamShakeOnDamage", damage+50);
				StartCoroutine(PlayerDeath());
			}
			
			invincible = true;
			yield return new WaitForSeconds(invinTime);		//Temporarily makes player invulnerable
			invincible = false; 
		}
	}

	void KnockBack(Vector2 dir){
		//rigidbody2D.isKinematic = true;
		rigidbody2D.AddForce (dir.normalized * KnockBackForce);
		//yield return new WaitForSeconds (.1f);
		//rigidbody2D.isKinematic = false;
	}

	IEnumerator PlayerDeath(){
		death = true;
		gameObject.rigidbody2D.isKinematic = true;	//Prevents enemy from pushing player after death
		rigidbody2D.velocity = new Vector2 (0,0);
		anim.SetBool("IsDead",true);
		yield return new WaitForSeconds(2.583f);
		GameOver = true;
		//Application.LoadLevel ("GameOverScene");
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
}
