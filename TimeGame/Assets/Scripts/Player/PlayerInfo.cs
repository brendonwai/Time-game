using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	public int Health=100;
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
