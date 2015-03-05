using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public bool hacked = false;		//Determines hacked state
	public bool inRange = false;	//Determines if object is in hacking range
	public int orientation = 0;		//Gets what direction it is facing

	int EnergyCost = 5;		//How much energy needs to be spent to hack

	Animator anim;				//Sets sprite image after hack
	GameObject player;			//Gets player info to determine if in or out of hack radius
	PlayerInfo playerscript;

	//Colors
	Color OutOfHackRange = new Color(.6f, .1f, .1f, 1f);
	Color InHackRange = new Color(0f, .4f, .5f, 1f);
	Color LevelColor;

	int i = 0;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerscript = player.GetComponent<PlayerInfo>();
		LevelColor = GetComponent<Renderer>().material.color;
	}

	void OnMouseOver(){
		if(!hacked){
			if(playerscript.Energy>= EnergyCost){
				GetComponent<Renderer>().material.color = InHackRange;
				if(inRange && Input.GetMouseButtonDown(1)){
					playerscript.SpendEnergy(EnergyCost);
					Hacked();
				}
			}
			else
				GetComponent<Renderer>().material.color = OutOfHackRange;
		}
	}

	void OnMouseExit(){
		GetComponent<Renderer>().material.color = LevelColor;
	}


	//Run hacked behavior
	public void Hacked(){
		anim.SetBool("IsHacked", true);
		hacked = true;
		StartCoroutine (GateHackAnim());	//For gates if ObjectInfo needs to be in other gameobjects then add an if statement here.
	}

	IEnumerator GateHackAnim() {
		player.GetComponent<Player2DController> ().inHackingAnim = true;
		if(orientation == 0)
			GateHackBot();
		else if(orientation == 1)
			GateHackLeft();
		else
			GateHackRight();
		player.GetComponent<Animator> ().SetBool ("IsHackingGate", true);
		yield return new WaitForSeconds (0.433f);
		player.GetComponent<Player2DController> ().inHackingAnim = false;
		player.GetComponent<Animator> ().SetBool ("IsHackingGate", false);

	}
	

	void PlayerPosition(float X_Delta, float Y_Delta){
		player.transform.position = new Vector3(transform.position.x + X_Delta, transform.position.y + Y_Delta, player.transform.position.z);
	}

	//For specifying teleporting behavior
	void GateHackBot(){
		if (transform.position.x < player.transform.position.x) {											//If player is to the right of target
			if (!player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the right while not facing left then flip to face left.
				player.GetComponent<Player2DController> ().Flip();
			}
		}
		else {
			if (player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
				player.GetComponent<Player2DController> ().Flip();
			}
		}
		PlayerPosition(0f,-0.3f);
	}

	void GateHackLeft(){
		if (player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
			player.GetComponent<Player2DController> ().Flip();
		}
		PlayerPosition(-0.3f,0.0f);
	}

	void GateHackRight(){
		if (!player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
			player.GetComponent<Player2DController> ().Flip();
		}
		PlayerPosition(0.3f,0.0f);
	}
}
