using UnityEngine;
using System.Collections;

public class ObjectBehavior : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	//Change this later so only the player can change this state and
	//this is not continually updated
	void Update () {
		if (GetComponent<ObjectInfo>().hacked)	//Take out this if-statement later
			Hacked();

	}

	//Accessed by player to run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
	}
}
