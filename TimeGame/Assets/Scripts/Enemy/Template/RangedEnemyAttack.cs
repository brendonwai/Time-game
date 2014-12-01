using UnityEngine;
using System.Collections;

public class RangedEnemyAttack : MonoBehaviour {

	Animator anim;
	bool TargetInRange = false;
	public Rigidbody2D bullet;
	int bulletspeed=10;
	public GameObject player;
	private float timestamp=0.0f;
	
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
		if(TargetInRange){
			Attack ();
			//sets fire rate to 2 shoots per second
			if (Time.time>=timestamp){
				Shoot ();
				timestamp=Time.time+.5f;
			}
		}
	}
	
	void Attack(){
		anim.SetBool("IsAttacking", true);
	}

	void Shoot(){
		Vector2 bulletClonePos = transform.position;
		bulletClonePos.x -= .2f;
		Vector2 bulletClone2Pos = transform.position;
		bulletClone2Pos.x += .2f;
		Rigidbody2D bulletClone=Instantiate (bullet, bulletClonePos, transform.rotation) as Rigidbody2D;
		Rigidbody2D bulletClone2=Instantiate (bullet, bulletClone2Pos, transform.rotation) as Rigidbody2D;
		Vector2 dir = (player.transform.position - bulletClone.transform.position).normalized;
		Vector2 dir2 = (player.transform.position - bulletClone2.transform.position).normalized;
		bulletClone.AddForce (dir*bulletspeed);
		bulletClone2.AddForce (dir2*bulletspeed);
		bulletClone.renderer.material.color = Color.cyan;
		bulletClone2.renderer.material.color = Color.cyan;

	}
}