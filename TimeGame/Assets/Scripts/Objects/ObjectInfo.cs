using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public bool hacked = false;		//Determines hacked state
	public bool inRadius = false;	//Determines if object is in Hack Radius
	public int EnergyCost = 0;		//How much energy needs to be spent to hack

	Animator anim;				//Sets sprite image after hack
	GameObject player;			//Gets player info to determine if in or out of hack radius		

	//Colors
	Color OutOfHackRadius = new Color(.6f, .1f, .1f, 1f);
	Color InHackRadius = new Color(0f, .4f, .5f, 1f);

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnMouseOver(){
		if(!hacked)
			if(inRadius){
				renderer.material.color = InHackRadius;
				if(Input.GetMouseButtonDown(1))
					Hacked();
			}
			else
				renderer.material.color = OutOfHackRadius;
	}

	void OnMouseExit(){
		renderer.material.color = Color.white;
	}


	//Accessed by player to run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
		hacked = true;		//Uncomment this later once player can hack
		transform.FindChild ("ObjectTag").tag = "Untagged";
	}
}
