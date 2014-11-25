﻿using UnityEngine;
using System.Collections;

public class Player2DController : MonoBehaviour {

	public float maxSpeed = 5f;			//Sets momvement speed
	bool facingLeft = true;			//Determines direction character facing
	Animator anim;						//Animation object
	public GameObject redscreen;
	
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

	// Call this when damage dealt to enemy
	void takeDamage(int damage){
		//sprite flashes red upon taking damage
		renderer.material.color = Color.red;
		redscreen.SendMessage ("FlashRedScreen");
		renderer.material.color=Color.white;
		//reduce health by amount of damage
		GetComponent<PlayerInfo>().Health -= damage;
		if (GetComponent<PlayerInfo>().Health<=0){
			GetComponent<PlayerInfo>().Health=0;
			anim.SetBool("IsDead",true);
			PlayerDeath();

		}

	}

	void PlayerDeath(){
		Application.LoadLevel ("GameOverScene");
	}

	//Flips character sprite to face direction of movement
	void Flip() {
		facingLeft = !facingLeft;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
