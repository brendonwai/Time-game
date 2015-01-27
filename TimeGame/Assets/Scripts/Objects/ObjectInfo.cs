using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public bool hacked = false;		//Determines hacked state
	public bool inRange = false;	//Determines if object is in hacking range
	public int EnergyCost = 5;		//How much energy needs to be spent to hack

	Animator anim;				//Sets sprite image after hack
	GameObject player;			//Gets player info to determine if in or out of hack radius
	PlayerInfo playerscript;

	//Colors
	Color OutOfHackRange = new Color(.6f, .1f, .1f, 1f);
	Color InHackRange = new Color(0f, .4f, .5f, 1f);

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerscript = player.GetComponent<PlayerInfo>();
	}

	void OnMouseOver(){
		if(!hacked)
			if(inRange){
				if(Input.GetMouseButtonDown(1))
					Hacked();
			}
			else
				renderer.material.color = OutOfHackRange;
	}

	void OnMouseExit(){
		if(!inRange)	//Prevents color from turning white on mouser over while still in range
			renderer.material.color = Color.white;
	}

	//Checks if player is in front of keyboard
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			renderer.material.color = InHackRange;
			inRange = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "Player" && !hacked)
			if(Input.GetKeyDown(KeyCode.X) && CanSpendEnergy())
				Hacked();
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			inRange = false;
			renderer.material.color = Color.white;
		}
	}

	//Player has the energy to spend for hacking
	bool CanSpendEnergy(){
		if(playerscript.Energy >= EnergyCost){
			playerscript.Energy -= EnergyCost;
			return true;
		}
		else
			return false;
	}


	//Run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
		hacked = true;
		StartCoroutine (GateHackAnim());
	}

	IEnumerator GateHackAnim() {
		player.GetComponent<Animator> ().SetBool ("IsHackingGate", true);
		yield return new WaitForSeconds (1.017f);
		player.GetComponent<Animator> ().SetBool ("IsHackingGate", false);
	}
}
