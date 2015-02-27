using UnityEngine;
using System.Collections;

public class AOEAttack : MonoBehaviour {
	public bool inCollider;
	private Animator anim;
	// Use this for initialization
	void Start () {
		inCollider = false;
		anim = GetComponent<Animator>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			inCollider = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Player") {
			inCollider = false;
		}
	}
}
