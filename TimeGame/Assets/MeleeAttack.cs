using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

	Animator anim;
	bool TargetInRange;
	GameObject player;

	public bool charging;
	float chargeTime;
	public float chargeMax;

	public float coolDownLength;
	float coolDownTime;
	public bool onCoolDown;

	void Awake() {
		anim = GetComponentInParent<Animator>();
		charging = false;
		chargeTime = 0f;
		onCoolDown = false;
		coolDownTime = 0f;
	}

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<MeleeAI>().GetPlayer();
	}
	
	// Update is called once per frame
	void Update () {
		if(TargetInRange && !charging && !anim.GetBool("IsAttacking") && !onCoolDown) {
			charging = true;
			chargeTime = 0f;
		}
		else if(charging) {
			anim.SetBool("IsMoving", false);
			GetComponentInParent<MeleeAI>().stopMove = true;
			chargeTime += Time.deltaTime;
			if(chargeTime >= chargeMax) {
				chargeTime = 0f;
				charging = false;
				GetComponentInParent<MeleeAI>().stopMove = false;
				anim.SetBool("IsAttacking", true);
				onCoolDown = true;
			}
		}
		else if(onCoolDown) {
			coolDownTime += Time.deltaTime;
			if(coolDownTime >= coolDownLength) {
				coolDownTime = 0f;
				onCoolDown = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player") {
			TargetInRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			TargetInRange = false;
		}
	}
}
