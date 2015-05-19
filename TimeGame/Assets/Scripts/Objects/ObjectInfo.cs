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
	Player2DController playerControlScript;
	SkillButtonHandler buttonController;

	//Colors
	Color OutOfHackRange = new Color(.6f, .1f, .1f, 1f);
	Color InHackRange = new Color(0f, .4f, .5f, 1f);
	Color LevelColor;

	int i = 0;
	float XDelta;
	float YDelta;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerscript = player.GetComponent<PlayerInfo>();
		playerControlScript = player.GetComponent<Player2DController>();
		buttonController = playerControlScript.buttonController.GetComponent<SkillButtonHandler>();
		LevelColor = GetComponent<Renderer>().material.color;
		AssignDelta();
	}


	//For getting position of keyboards 
	void AssignDelta(){
		if(orientation == 0){
			XDelta = 0.0f;
			YDelta = -.3f;
		}
		else if(orientation == 1){
			XDelta = -.3f;
			YDelta = 0.0f;
		}
		else{
			XDelta = .3f;
			YDelta = 0.0f;
		}
	}

	void OnMouseOver(){
		if(!hacked){
			if(playerscript.Energy>= EnergyCost && playerControlScript.canHackComp){
				Vector3 difference = player.transform.position - transform.position;

				GetComponent<Renderer>().material.color = InHackRange;
				if(inRange && Input.GetMouseButtonDown(1)){
					RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + difference.normalized*10f,player.transform.position - transform.position, 1000f);
					foreach(RaycastHit2D r in hits){
						if(r.transform.gameObject.tag == "Background"){break;}
						else if(r.transform.gameObject.tag.StartsWith("GateC")){
							playerscript.SpendEnergy(EnergyCost);
							buttonController.StartCD(2);
							playerControlScript.StartCoroutine("HackCompRecovery");
							Hacked();
							break;
						}
					}
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
	

	void PlayerPosition(){
		player.transform.position = new Vector3(transform.position.x + XDelta, transform.position.y + YDelta, player.transform.position.z);
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
		PlayerPosition();
	}

	void GateHackLeft(){
		if (player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
			player.GetComponent<Player2DController> ().Flip();
		}
		PlayerPosition();
	}

	void GateHackRight(){
		if (!player.GetComponent<Player2DController> ().facingLeft) {	//If player is on the left while facing left then flip to face right.
			player.GetComponent<Player2DController> ().Flip();
		}
		PlayerPosition();
	}
}
