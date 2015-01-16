using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HackRadius : MonoBehaviour {
	
	Animator anim;								     
	public int HackCD;								//Hacking ability Cooldown
	SpriteRenderer hacksprite;						//CHILD. Hacking sprite displayed above player head
	Vector3 PlayerTrans;
	ArrayList objectsInRange;
	
	
	//Keys
	public KeyCode hackKey;							//Button to Hack an enemy
	
	
	
	void Start() {
		anim = this.transform.parent.GetComponent<Animator> ();
		hacksprite = GetComponentInChildren<SpriteRenderer> ();
		PlayerTrans = this.transform.parent.transform.position;
		objectsInRange = new ArrayList ();
		//anglesign = 1;
		HackCD = 10;
	}
	
	void Update() {
		
		/*
		//Scrapped but can be used for something else
		//Angle of mouse around object
		//Box Collider for sword was center(0.2, 0) size(0.4, 0.1)
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);											//Mouse Transform
		float sign = Mathf.Sign (mouse.y - transform.position.y);														//Sign of y of mouse
		angle = Vector2.Angle (Vector2.right, mouse-transform.position) * Mathf.Sign (mouse.y - transform.position.y);	//Get angle from transform to mouse
		angle *= anglesign;
		if (angle > -22.5 && angle < 22.5) {
			//Right
			transform.rotation = Quaternion.AngleAxis (0, Vector3.forward);
		}
		else if(angle >= 22.5 && angle < 67.5){
			//Top Right
			transform.rotation = Quaternion.AngleAxis (45, Vector3.forward);
		}
		else if (angle >= 67.5 && angle < 112.5) {
			//Top
			transform.rotation = Quaternion.AngleAxis (90, Vector3.forward);
		}
		else if (angle >= 112.5 && angle < 157.5) {
			//Top Left
			transform.rotation = Quaternion.AngleAxis (135, Vector3.forward);
		}
		else if (angle >= 157.5 || angle <= -157.5) {
			//Left
			transform.rotation = Quaternion.AngleAxis (180, Vector3.forward);
		}
		else if (angle > -67.5 && angle <= -22.5) {
			//Bottom Right
			transform.rotation = Quaternion.AngleAxis (-45, Vector3.forward);
		}
		else if (angle > -112.5 && angle <= -67.5) {
			//Bottom
			transform.rotation = Quaternion.AngleAxis (-90, Vector3.forward);
		}
		else if (angle > -157.5 && angle <= -112.5) {
			//Bottom Left
			transform.rotation = Quaternion.AngleAxis (-135, Vector3.forward);
		}*/
		
		
		
	}	
	
	void FixedUpdate(){
		//do cooldown iteration here I think
		//Hacking
		//NOTE YOU CAN STILL HACK WHILE DEAD TAKE THAT OUT
		// Check the energy and subtract the energy
		if (!anim.GetBool ("IsHackingEnemy")) {				//If you aren't controlling an enemy			
			if (Input.GetKeyDown(hackKey)) {
				// Checks whether you click on the enemy and not one of its larger, surrounding colliders
				RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				for(int i = 0; i < hits.Length; i++) {
					if(hits[i].collider != null) {
						GameObject hitObject = hits[i].collider.gameObject;
						if(hitObject.transform.tag == "Enemy" && inRange(hitObject.transform.parent.gameObject)) {
							if(enoughEnergy(hitObject.transform.parent.gameObject)) {
								HackObject (hitObject.transform.parent.gameObject);
							}
						}
					}
				}
			}
		} else {		//If you are controlling an enemy
			hacksprite.enabled = false;
		}
	}
	
	public bool inRange(GameObject hitObject) {
		return objectsInRange.Contains (hitObject);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			objectsInRange.Add (other.transform.parent.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy" && objectsInRange.Contains (other.transform.parent.gameObject)) {
			objectsInRange.Remove (other.transform.parent.gameObject);
		}
	}

	public bool enoughEnergy(GameObject other) {
		return GetComponentInParent<PlayerInfo> ().Energy >= other.GetComponentInParent<EnemyInfo> ().requiredEnergy;
	}

	// Keeping the method HackObject even though this doesn't take care of gates anymore because the player may still
	//    be able to hack a health drone which is different from an enemy
	void HackObject(GameObject other) {
		Vector3 otherpos = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);//Store enemy/gate position
		
		//NOTE: make player face enemy hackobject. for gates only care if horizontal
		
		string objtag = "";		//Note every enemy should have an ObjecTag gameobject with tag
		
		for (int i = 0; i < other.transform.childCount; i++) {
			if(other.transform.GetChild(i).name == "ObjectTag") {
				objtag = other.transform.GetChild(i).gameObject.tag;
			}
		}
		
		PlayerTrans = this.transform.parent.transform.position;
		
		if (objtag == "Enemy")
			HackEnemy(other, otherpos);
		GetComponentInParent<PlayerInfo>().Energy -= other.GetComponentInParent<EnemyInfo>().requiredEnergy;
	}

	//For hacking enemies
	void HackEnemy(GameObject other, Vector3 otherpos){
		/*if (this.GetComponentInParent<Player2DController> ().facingLeft) {
				this.GetComponentInParent<Player2DController> ().Flip();
			}*/
		if (otherpos.x < PlayerTrans.x) {											//If player is to the right of target
			PlayerTrans = new Vector3(otherpos.x + 0.5f, otherpos.y, otherpos.z);
		}
		else {
			PlayerTrans = new Vector3(otherpos.x - 0.5f, otherpos.y, otherpos.z);
		}
		
		int hackType = other.GetComponent<EnemyInfo>().enemyType;
		
		anim.SetBool ("IsHackingEnemy", true);
		anim.SetBool ("HackedEnemyDead", false);
		anim.SetInteger("EnemyType", hackType);
		
		GetComponentInParent<Player2DController>().hackState = hackType;
		
		GetComponentInParent<PlayerInfo> ().SwapPlayerToEnemyHealth (other.GetComponent<EnemyInfo> ().Health);
		GetComponentInParent<PlayerInfo> ().healthBar.value = GetComponentInParent<PlayerInfo> ().Health;
		Destroy (other);
		
		this.GetComponentInParent<Player2DController> ().HackFlip();			//NOTE: this is here because player sprites are drawn to the left and enemy sprites are drawn to the right. Must add one when player exits machine.
		
	}
	
	/*public void AntiFlipSword() {	//not needed for now
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		anglesign *= -1;
		transform.localScale = theScale;
	}*/
}
