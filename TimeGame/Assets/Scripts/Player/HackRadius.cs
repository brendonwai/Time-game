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
	SpriteRenderer hacksprite;				//Hacking sprite displayed above player head
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
		HackableObjectTags.Add("Gate");

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
		//ShowHackSprite(InHackRadiusList[hackselection]);			//UNCOMMENT WHEN READY TO TEST
		if (isHacking == true) {
			if (InHackRadiusList.Count == 0) {
				isHacking = false;
				Debug.Log ("Nothing in Radius. Hacking stopped.");
			}
			else if (Input.GetKeyDown(hackLockInKey)) {					//Locks-in Selection
				Debug.Log ("Hack Lock in: " + hackselection);
				isHacking = false;
				HackCD = 10; 										//Change to max CD
				HackObject(InHackRadiusList[hackselection]);
			}
			else if (Input.GetKeyDown(selectRight)) {				//Press Right Key move selection right
				if (hackselection+1 >= InHackRadiusList.Count){		//If increasing selection is over the limit go back to start
					hackselection = 0;
				}
				else {												//Increase selection by 1
					hackselection ++;
				}
				Debug.Log ("Curr: " + hackselection + ", Max:" + InHackRadiusList.Count);
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
				Debug.Log ("Curr: " + hackselection + ", Max:" + InHackRadiusList.Count);
			}
		}
		else {
			if (Input.GetKey(hackKey) && InHackRadiusList.Count > 0)	//Insert hack cooldown here
			{
				Debug.Log("Hack Key Pressed");
				if (HackCD == 10) {		//Change this once we add in cooldowns
					isHacking = true;
					Debug.Log("Now Hacking");
					//ChooseHackObject();
				}
			}
		}


		//IF NEEDED: put InHackRadiusList.Sort (); here
	}

	//Calls when objects enter the collider
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {										//Checks for Enemy Tags
			InHackRadiusList.Add (other.transform.parent.gameObject);				//Adds Enemy GameObject to the list of hackable objects in range with all it's components and children.
			Debug.Log ("Added " + other.transform.parent.gameObject.ToString());
		}
		else if (other.gameObject.tag == "Gate") {									//Checks for Gate Tags
			InHackRadiusList.Add (other.transform.parent.gameObject);				//Adds Gate GameObject to the list of hackable objects in range iwth all it's components and children.
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (InHackRadiusList.Contains (other.transform.parent.gameObject)) {
			InHackRadiusList.Remove (other.transform.parent.gameObject);
			if (hackselection >= InHackRadiusList.Count && InHackRadiusList.Count != 0) {
				hackselection--;
				Debug.Log ("Hackselection decreased by 1 (Obj left) at max: " + hackselection);
			}
			Debug.Log ("Removed " + other.transform.parent.gameObject.ToString());
		}
	}

	 /*void ChooseHackObject() {
		//Go SlowMo/Freeze and Display List or GUI of hackable objects
		//Decide on which hack object you want to select with some menu (Think Assassin's Creed weapon select or like a left and right inventory)
		while (isHacking == true) {
			Debug.Log ("ChooseHackObject() Loop");
			//ShowHackSprite(InHackRadiusList[hackselection]);						//Display Sprite of Enemy/Gate above Player's head. CRASHES UNITY
			
			if (Input.GetKeyDown(hackLockInKey)){					//Locks-in Selection
				Debug.Log ("Hack Lock in" + hackselection);
				isHacking = false;								//Breaks out of loop
			}
			else if (Input.GetKeyDown(selectRight)) {				//Press Right Key move selection right
				if (hackselection+1 >= InHackRadiusList.Count){	//If increasing selection is over the limit go back to start
					hackselection = 0;
				}
				else {											//Increase selection by 1
					hackselection ++;
				}
				Debug.Log ("Hackselection " + hackselection);
			}
			else if (Input.GetKeyDown(selectLeft)) {				//Press Left Key move selection left
				if (hackselection == 0){						//If decreasing selection is less than 0 go to end of list
					hackselection = InHackRadiusList.Count-1;
				}
				else {											//Decreases selection by 1
					hackselection--;
				}
				Debug.Log ("Hackselection " + hackselection);
			}
			//Display Hackable Object's sprite above player
		}

		Debug.Log ("Out of Loop");
		//HackObject (InHackRadiusList[hackselection]);
		hackselection = 0;
	}*/

	void ShowHackSprite(GameObject other){													//HAS PROBLEMS WILL CRASH UNITY
		Debug.Log ("ShowHackSprite()");
		string newspritename = other.GetComponent<SpriteRenderer> ().sprite.ToString ();
		hacksprite.sprite = Resources.Load<Sprite> (newspritename);
	}

	void HackObject(GameObject other) {
		Debug.Log ("HackObject()");
		Vector3 temppos = other.transform.position;//Store enemy/gate position
		Debug.Log ("temppos " + temppos);
		Debug.Log ("Name: " + other.ToString());
		//Will need to put position horizontally to the side of hacked game object here then  
		//PlayerTrans.position = temppos.
		//put animation here
		//

		//NOTE: with gates will probably need to store gate facing direction to position player next to it.

		Destroy (other);
		PlayerTrans.position = temppos;

		/*if(hackedname == "enemy") {
			Vector3 hackedpos = other.transform.position;
			Destroy(other.gameObject);
			transform.position = hackedpos;
			
		}
		else if (hackedname == "gate") {
			//gate open commands	
		}
		*/
	}
	/*public void AntiFlipSword() {	//not needed for now
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		anglesign *= -1;
		transform.localScale = theScale;
	}*/
}
