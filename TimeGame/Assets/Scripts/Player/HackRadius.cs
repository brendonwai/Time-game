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
	List<string> HackableObjectNames;

	void Start() {
		anim = GetComponentInParent<Animator> ();
		//anglesign = 1;
		HackCD = 10;
		InHackRadiusList = new List<GameObject>();
		HackableObjectNames.Add ("Basic Enemy");
		HackableObjectNames.Add ("ranged");
		HackableObjectNames.Add ("Gate");
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
				HackObject();
			}
		}

	}

	void FixedUpdate(){
		//do cooldown here I think
	}

	//Calls when objects enter the collider
	void OnTriggerEnter2D(Collider2D other) {		//NOTE: I think it triggers on the player's 2D collider as well
		Debug.Log ("Object in Hack Radius");
		if (!HackableObjectNames.Contains(other.gameObject.ToString())) {
			InHackRadiusList.Add (other.gameObject);
			Debug.Log ("Added" + other.gameObject.ToString());
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (InHackRadiusList.Contains (other.gameObject)) {
			InHackRadiusList.Remove (other.gameObject);
			Debug.Log ("Removed" + other.gameObject.ToString());
		}
	}

	void HackObject() {
		//Go SlowMo/Freeze and Display List or GUI of hackable objects




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
