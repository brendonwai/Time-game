﻿using UnityEngine;
using System.Collections;

public class RangedEnemyAttack : MonoBehaviour {

	Animator anim;
	bool TargetInRange = false;
	public GameObject bullet;
	int bulletspeed=6;
	public GameObject player;
	
	void Awake(){
		anim = GetComponentInParent<Animator>();
	}
	
	void OnTriggerStay2D(Collider2D other){
		if(other.tag == "Player")
			TargetInRange = true;
	}
	
	void OnTriggerExit2D(Collider2D other){
		if(other.tag == "Player")
			TargetInRange = false;
		anim.SetBool("IsAttacking", false);
	}
	
	void FixedUpdate(){
		if(TargetInRange)
			Attack ();
	}
	
	void Attack(){
		anim.SetBool("IsAttacking", true);
		Instantiate (bullet, transform.position, transform.rotation);
		Vector2 v = bullet.rigidbody2D.velocity;
		v.y = bulletspeed;
		bullet.rigidbody2D.velocity = v;
		bullet.renderer.material.color = Color.blue;
	}

	void GenerateBullets(){


	}
}