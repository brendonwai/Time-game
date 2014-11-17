using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HackRadius : MonoBehaviour {
	bool isHacking;
	float angle;
	Animator anim;
	//int anglesign;
	public int HackCD;
	public KeyCode hackKey;
	List<GameObject> InHackRadiusList;
	List<string> HackableObjectTags;

	void Start() {
		anim = GetComponentInParent<Animator> ();
		//anglesign = 1;
		HackCD = 10;
		InHackRadiusList = new List<GameObject>();
		HackableObjectTags = new List<string> ();
		HackableObjectTags.Add("Enemy");
		HackableObjectTags.Add("Gate");
		isHacking = false;
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

		//Hacking
		if (Input.GetKey(hackKey) && isHacking == false )	//Insert hack cooldown here
		{
			Debug.Log("Hack Key Pressed");
			if (HackCD == 10) {		//Change this once we add in cooldowns
				Debug.Log("Hacking");
				ChooseHackObject();
			}
		}

	}

	void FixedUpdate(){
		//do cooldown iteration here I think
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
			Debug.Log ("Removed " + other.transform.parent.gameObject.ToString());
		}
	}

	void ChooseHackObject() {
		//Decide on which hack object you want to select with some menu (Think Assassin's Creed weapon select or like a left and right inventory)
		while (isHacking == true) {
			int hackselection = 0;

			InHackRadiusList.Find ("Basic Enemy");
			if (Input.GetKey(hackKey)){

			}
		}
		InHackRadiusList.Find ("Basic Enemy");
		HackObject (InHackRadiusList.Find(""));
	}

	void HackObject(GameObject other) {
		//Go SlowMo/Freeze and Display List or GUI of hackable objects

		//INSERT BRING UP HACK MENU HERE

		//InHackRadiusList.Find



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
