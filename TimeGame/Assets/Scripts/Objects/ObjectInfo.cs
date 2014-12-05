using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public bool hacked = false;
	Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	//Change this later so only the player can change this state and
	//this is not continually updated
	void Update () {

	}
	
	//Accessed by player to run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
		hacked = true;		//Uncomment this later once player can hack
		transform.FindChild ("ObjectTag").tag = "Untagged";
	}


}
