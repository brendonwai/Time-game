using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HackRadius : MonoBehaviour {

	Animator anim;								     
	public int HackCD;								//Hacking ability Cooldown
	SpriteRenderer hacksprite;						//CHILD. Hacking sprite displayed above player head
	ArrayList objectsInRange;

	float HorizHackTeleport = 0.3f;

	SkillButtonHandler buttonController;
	Player2DController playerScript;

	//Keys
	public KeyCode hackKey;							//Button to Hack an enemy

	void Start() {
		anim = this.transform.parent.GetComponent<Animator> ();
		hacksprite = GetComponentInChildren<SpriteRenderer> ();
		objectsInRange = new ArrayList ();
		HackCD = 10;
		playerScript = GetComponentInParent<Player2DController>();
		buttonController = playerScript.buttonController.GetComponent<SkillButtonHandler>();
	}

	void FixedUpdate(){
		//Hacking
		//NOTE YOU CAN STILL HACK WHILE DEAD TAKE THAT OUT
		// Check the energy and subtract the energy
		if (!anim.GetBool ("IsHackingEnemy")) {				//If you aren't controlling an enemy			
			if (Input.GetKeyDown(hackKey) && playerScript.canHack) {
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
		RaycastHit2D hit = Physics2D.Linecast(transform.parent.gameObject.transform.position, hitObject.transform.position, 1 << LayerMask.NameToLayer("Walls"));
		return objectsInRange.Contains(hitObject) && hit.collider == null;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			objectsInRange.Add (other.transform.parent.gameObject);
		}
		if(other.gameObject.tag == "GateConnector")
			other.GetComponent<ObjectInfo>().inRange = true;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy" && objectsInRange.Contains (other.transform.parent.gameObject)) {
			objectsInRange.Remove (other.transform.parent.gameObject);
		}
		if(other.gameObject.tag == "GateConnector")
			other.GetComponent<ObjectInfo>().inRange = false;
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

		if (objtag == "Enemy") {
			HackEnemy(other, otherpos);
		}
		else {
			transform.parent.GetComponent<Player2DController> ().inHackingAnim = true;
		}
		GetComponentInParent<PlayerInfo>().SpendEnergy(other.GetComponentInParent<EnemyInfo>().requiredEnergy);
	}

	//For hacking enemies
	void HackEnemy(GameObject other, Vector3 otherpos){
		/*if (this.GetComponentInParent<Player2DController> ().facingLeft) {
				this.GetComponentInParent<Player2DController> ().Flip();
			}*/

		int hackType = other.GetComponent<EnemyInfo>().enemyType;
		if(hackType != 2){
			other.GetComponent<EnemyInfo>().isHacked = true; //Stops enemy movement
			anim.SetBool ("IsHackingEnemy", true);
			anim.SetBool ("HackedEnemyDead", false);
			StartCoroutine(HackEnemyAnim(other, hackType));
		}
		else{
			anim.SetBool("IsHackingEnemy", true);
			other.GetComponent<Animator> ().SetBool("isHacked", true);
			StartCoroutine(HackHealthDroneAnim());
			buttonController.StartCD(0);
			playerScript.StartCoroutine("HackRecovery");
		}

		transform.parent.GetComponent<Player2DController> ().inHackingAnim = true;	//Stop player from moving while hacking anim is playing
		transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, 0f);
		if (otherpos.x < transform.position.x) {											//If player is to the right of target
			if (!transform.parent.GetComponent<Player2DController> ().facingLeft) {	//If player is on the right while not facing left then flip to face left.
				transform.parent.GetComponent<Player2DController> ().Flip();
			}
			transform.parent.transform.position = new Vector3(otherpos.x + HorizHackTeleport, otherpos.y, transform.parent.transform.position.z);
		}
		else {
			if (transform.parent.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
				transform.parent.GetComponent<Player2DController> ().Flip();
			}
			transform.parent.transform.position = new Vector3(otherpos.x - HorizHackTeleport, otherpos.y, transform.parent.transform.position.z);
		}
	}

	IEnumerator HackEnemyAnim (GameObject other, int hackType) {
		//StartCoroutine(StopHackedEnemyMovement (other));
		if(other != null && hackType == 1) {
				other.GetComponent<Animator>().SetBool("BeingHacked", true);
				other.GetComponent<EnemyAI>().Flip();
		}
		yield return new WaitForSeconds (0.82f);
		transform.parent.GetComponent<Player2DController> ().inHackingAnim = false;	//Allows player to move after hacking anim is done
		if(other != null){
			anim.SetInteger("EnemyType", hackType);
			GetComponentInParent<Player2DController>().hackState = hackType;
			GetComponentInParent<Player2DController>().lastAttack = Time.time - 10f;		//Allows you to immediately attack after hacking an enemy
			GetComponentInParent<PlayerInfo> ().SwapPlayerToEnemyHealth (other.GetComponent<EnemyInfo> ().Health);
			GetComponentInParent<PlayerInfo> ().healthBar.value = GetComponentInParent<PlayerInfo> ().Health;
			GetComponentInParent<PlayerInfo>().HealthDrainActive = true;
			StartCoroutine(GetComponentInParent<PlayerInfo> ().HealthDrain ());
			Destroy (other);	//Destroys the enemy
		}
		else{
			anim.SetBool ("IsHackingEnemy", false);
		}
	}

	IEnumerator HackHealthDroneAnim () {	//Special case because we're not taking over the HP drone just hacking it for HP
		yield return new WaitForSeconds (0.82f);
		anim.SetBool ("IsHackingEnemy", false);
		transform.parent.GetComponent<Player2DController> ().inHackingAnim = false;	//Allows player to move after hacking anim is done
		//It'll destroy itself after being hacked
	}

}
