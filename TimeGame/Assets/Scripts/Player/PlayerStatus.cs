using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	Animator anim;
	int health;
	int energy;
	int hackHealth;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		health = 10;
		energy = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (health <= 0) {
			anim.SetBool("isDead", true);
		}
	}
}
