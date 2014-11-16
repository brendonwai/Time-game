using UnityEngine;
using System.Collections;

public class SwordObjectScript : MonoBehaviour {
	bool isHacking;
	float angle;
	Animator anim;
	int anglesign;
	public int HackCD;
	public KeyCode hackKey;

	void Start() {
		anim = GetComponentInParent<Animator> ();
		anglesign = 1;
		HackCD = 10;
	}
	void Update() {

		//Angle of mouse around object
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
		}

		//Hacking
		if (Input.GetKey(hackKey) && isHacking == false )	//Insert hack cooldown here
		{
			Debug.Log("Hack Key Pressed");
			if (HackCD == 10) {		//Change this once we add in cooldowns
				isHacking = true;
			}
		}
	}

	//Calls when objects enter the collider
	void OnTriggerEnter2D(Collider2D other) {		//NOTE: I think it triggers on the player's 2D collider as well
		Debug.Log ("Object in Sword collider");
		if (isHacking == true) {
			
			//Replace MainChar sprite disappear
			//Vector3 otherpos = other.transform.position;
			HackObject(other.gameObject);
			isHacking = false;
		}
	}

	void HackObject(GameObject other) {
		string hackedname = other.GetComponent <EnemyInfo> ().Name;
		//NOTE: Give all objects a ObjectInfo Script to get their names/health.
		//This is causing problems since they don't all have it.

		Debug.Log (hackedname);
		if(hackedname == "enemy") {
			Vector3 hackedpos = other.transform.position;
			Destroy(other.gameObject);
			transform.position = hackedpos;
			
		}
		else if (hackedname == "gate") {
			//gate open commands	
		}
	}
	public void AntiFlipSword() {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		anglesign *= -1;
		transform.localScale = theScale;
	}
}
