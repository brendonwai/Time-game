using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HackRadius : MonoBehaviour {
	bool isHacking;							//True if player is in process of hacking
	//float angle;							
	Animator anim;							//
	//int anglesign;
	public int HackCD;						//Hacking ability Cooldown
	List<GameObject> InHackRadiusList;		//List of objects in hack radius
	List<string> HackableObjectTags;		//List of Hackable Object Tags
	int hackselection;						//Hack selection number for menu
	SpriteRenderer hacksprite;				//CHILD. Hacking sprite displayed above player head
	Transform PlayerTrans;


	//Keys
	public KeyCode hackKey;					//Button to start Hack ability
	public KeyCode hackLockInKey;			//Locks in choice of Hack ability
	public KeyCode selectLeft;				//Moves Hack choice left (Decrease by 1)
	public KeyCode selectRight;				//Moves Hack hcoice right (Increase by 1)
	


	void Start() {
		anim = this.transform.parent.GetComponent<Animator> ();
		hacksprite = GetComponentInChildren<SpriteRenderer> ();
		PlayerTrans = this.transform.parent.transform;
		//anglesign = 1;
		HackCD = 10;
		InHackRadiusList = new List<GameObject>();

		//HackableObjectList
		HackableObjectTags = new List<string> ();
		HackableObjectTags.Add("Enemy");
		HackableObjectTags.Add("GateConnector");

		isHacking = false;
		hackselection = 0;
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
		//NOTE: poss move to FixedUpdate()
		//NOTE YOU CAN STILL HACK WHILE DEAD TAKE THAT OUT
		if (isHacking == true) {
			if (InHackRadiusList.Count <= 0) {
				isHacking = false;
				//Debug.Log ("Nothing in Radius. Hacking stopped.");
			}
			else if (Input.GetKeyDown(hackLockInKey)) {				//Locks-in Selection
				//Debug.Log ("Hack Lock in: " + hackselection + ", Name: " + InHackRadiusList[hackselection].name);
				isHacking = false;
				//HackCD = 10; 										//Change to max CD
				HackObject(InHackRadiusList[hackselection]);
				//change facing direction for the change of sprite
				GetComponentInParent<Player2DController> ().facingLeft = false;
			}
			else if (Input.GetKeyDown(selectRight)) {				//Press Right Key move selection right
				if (hackselection+1 >= InHackRadiusList.Count){		//If increasing selection is over the limit go back to start
					hackselection = 0;
				}
				else {												//Increase selection by 1
					hackselection ++;
				}
				//Debug.Log ("Curr: " + hackselection + ", Max:" + InHackRadiusList.Count + ", Name: " + InHackRadiusList[hackselection].name);
			}
			else if (Input.GetKeyDown(selectLeft)) {				//Press Left Key move selection left
				if (InHackRadiusList.Count == 0) {

				}
				else if(hackselection == 0){						//If decreasing selection is less than 0 go to end of list
					hackselection = InHackRadiusList.Count-1;
				}
				else {												//Decreases selection by 1
					hackselection--;
				}
				//Debug.Log ("Curr: " + hackselection + ", Max:" + InHackRadiusList.Count + ", Name: " + InHackRadiusList[hackselection].name);
			}
			if (isHacking == true && InHackRadiusList.Count != 0) {
				ShowHackSprite(InHackRadiusList[hackselection]);
			}
		}
		else {
			hacksprite.enabled = false;
			if (Input.GetKey(hackKey) && InHackRadiusList.Count > 0)	//Insert hack cooldown here
			{
				//Debug.Log("Hack Key Pressed");
				if (HackCD == 10) {										//Change this once you add in cooldowns
					isHacking = true;
					anim.SetBool ("HackedEnemyDead", false);
					hacksprite.enabled = true;
					//Debug.Log("Now Hacking");
				}
			}
		}
		//IF NEEDED: put InHackRadiusList.Sort (); here
	}

	//Calls when objects enter the collider

	void OnTriggerEnter2D(Collider2D other) {
		if (HackableObjectTags.Contains(other.gameObject.tag)) {										//Checks for Enemy Tags
			InHackRadiusList.Add (other.transform.parent.gameObject);				//Adds Enemy GameObject to the list of hackable objects in range with all it's components and children.
			//Debug.Log ("Added " + other.transform.parent.gameObject.name);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (HackableObjectTags.Contains(other.gameObject.tag)) {										//Checks for Enemy Tags
			if (InHackRadiusList.Contains (other.transform.parent.gameObject)) {	//Unsure if you can take out "transform.parent." part
				InHackRadiusList.Remove (other.transform.parent.gameObject);
				if (hackselection >= InHackRadiusList.Count && InHackRadiusList.Count >= 1) {
					hackselection--;
					//Debug.Log ("Hackselection decreased by 1 (Obj left) at max: " + hackselection);
				}
				//Debug.Log ("Removed " + other.transform.parent.gameObject.name);
			}
		}



	}

	void ShowHackSprite(GameObject other){										//Display current hackselection above player's head
		hacksprite.sprite = other.GetComponent<SpriteRenderer> ().sprite;		//NOTE: May need to flip sprite when char flips
		//string newspritename = other.GetComponent<SpriteRenderer> ().sprite.name;
		//Debug.Log (newspritename);
	}

	void HackObject(GameObject other) {
		Vector3 otherpos = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);//Store enemy/gate position

		//NOTE: make player face enemy hackobject. for gates only care if horizontal

		string objtag = other.transform.FindChild ("ObjectTag").tag;		//Note every enemy and gate should have an ObjecTag gameobject with tag

		//Debug.Log ("Tag: " + objtag);
		if (objtag == "Enemy") {
			if (otherpos.x < PlayerTrans.position.x) {											//If player is to the right of target
				PlayerTrans.position = new Vector3(otherpos.x + 0.5f, otherpos.y, otherpos.z);
			}
			else {
				PlayerTrans.position = new Vector3(otherpos.x - 0.5f, otherpos.y, otherpos.z);
			}

			string hackname = other.name;
			anim.SetBool ("IsHackingEnemy", true);
			anim.SetBool ("HackedEnemyDead", false);
			switch(hackname) {
			case "BasicEnemy":
				anim.SetInteger ("EnemyType", 0);
				break;
			case "ranged":
				anim.SetInteger ("EnemyType", 1);
				break;
			//Insert more enemies here
			
			}
			InHackRadiusList.Remove (other);
			Destroy (other);
		}
		else if (objtag == "GateConnector") {		//Take out second part if you don't need
			//Note: must check if gate is horizontal facing but that's for later once/if we add them in.

			PlayerTrans.position = new Vector3(otherpos.x, otherpos.y - 0.65f, otherpos.z);		//For vertical GateConnectors only

			anim.SetBool ("IsHackingGate", true);
			other.GetComponent<ObjectInfo>().Hacked();
			InHackRadiusList.Remove (other);			//Don't know if need or not
			anim.SetBool ("IsHackingGate", false);
		}

		//Will need to put position horizontally to the side of hacked game object here then  
		//PlayerTrans.position = new Vector3(temppos.x, temppos.y, temppos.z)


	}
	/*public void AntiFlipSword() {	//not needed for now
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		anglesign *= -1;
		transform.localScale = theScale;
	}*/
}
