using UnityEngine;
using System.Collections;

public class Player2DController : MonoBehaviour {

	//Movement/Animations
	public float maxSpeed = 5f;			//Sets momvement speed
	bool facingRight = true;			//Determines direction character facing
	Animator anim;						//Animation object
	public int health = 1;

	//Hacking
	public BoxCollider2D sword;
	bool isHacking = false;


	//Keys
	public KeyCode hackKey;



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate () {

		//Directional command handlers
		float move_x = Input.GetAxis ("Horizontal");    //Get input for x-axis
		float move_y = Input.GetAxis ("Vertical");      //Get input for y-axis
		
		//Changes value of Speed
		//*Speed defined in Unity Animator Controller
		anim.SetFloat ("Speed", Mathf.Abs (move_x));

		//Changes value of Health
		//anim.SetFloat ("Health", health); 

		//Makes character move based on inputs
		rigidbody2D.velocity = new Vector2 (move_x * maxSpeed, move_y * maxSpeed);

		/*
		 * If moving in the positive x direction (right) and the character is not
		 * facing right, makes the character sprite flip to face the right.
		 */	
		if((move_x > 0) && !facingRight){
			Flip();
		}
		else if((move_x < 0) && facingRight){
			Flip();
		}
		if (Input.GetKey(hackKey))
		{

		}
	}

	//Flips character sprite to face direction of movement also flips attached colliders
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//Hack function
	//shouldn't work well since
	void OnTriggerEnter2D(Collider2D other) {
		if (isHacking == true) {
			transform.position = other.transform.position;
			//if sword's collider is within enemy's collider
		}
		else if (isHacking==false) {
		
		}
		else {
			
			
		}
	}


}
