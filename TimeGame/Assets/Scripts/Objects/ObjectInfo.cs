using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public bool hacked = false;
	Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	//Accessed by player to run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
		hacked = true;		//Uncomment this later once player can hack
		transform.FindChild ("ObjectTag").tag = "Untagged";
	}
}
