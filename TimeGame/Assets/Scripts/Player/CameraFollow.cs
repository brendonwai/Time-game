using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform Player;
	static float z = -10f;
	//public float maxHorizUnits;
	//public float maxVertUnits;
	// Use this for initialization
	void Start () {
		//Screen.showCursor = true;		//not sure if this works
		transform.position = new Vector3(Player.position.x, Player.position.y, z);
	}
	// Update is called once per frame
	void Update () {
		float x = Player.position.x;
		float y = Player.position.y;

		//transform.position = new Vector3 (x, y, z);	//OLD set camera position to center on player

		float mouseX = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
		float mouseY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
		
		//Set camera pos to midpoint between player and cursor. Has problems if you try to change the 2s
		transform.position = new Vector3 ((x + mouseX)/2, (y + mouseY)/2, z);

		/*
		//Tried to make the camera have a max distance from the player but it didn't really work out
		if ((mouseX - x) >= maxHorizUnits){
			//
			if ((mouseY - y) >= maxVertUnits){
				//
				transform.position = new Vector3 ((x + maxHorizUnits)/2, (x + maxVertUnits)/2, z);
			}
			else if ((mouseY - y) <= -maxVertUnits){
				//
				transform.position = new Vector3 ((x + maxHorizUnits)/2, (x - maxVertUnits)/2, z);
			}
			else{
				//
				transform.position = new Vector3 ((x + maxHorizUnits)/2, (y + mouseY)/2, z);
			}
		}
		else if ((mouseX - x) <= -maxHorizUnits){
			//
			if ((mouseY - y) >= maxVertUnits){
				//
				transform.position = new Vector3 ((x - maxHorizUnits)/2, (x + maxVertUnits)/2, z);
			}
			else if ((mouseY + y) <= -maxVertUnits){
				//
				transform.position = new Vector3 ((x - maxHorizUnits)/2, (x - maxVertUnits)/2, z);
			}
			else{
				//
				transform.position = new Vector3 ((x - maxHorizUnits)/2, (y + mouseY)/2, z);
			}
		}
		else{
			//
			if ((mouseY - y) >= maxVertUnits){
				//
				transform.position = new Vector3 ((x + mouseX)/2, (x + maxVertUnits)/2, z);
			}
			else if ((mouseY - y) <= -maxVertUnits){
				//
				transform.position = new Vector3 ((x + mouseX)/2, (x - maxVertUnits)/2, z);
			}
			else{
				//Sets camera pos to midpoint between player and cursor. Has problems if you try to change the 2s
				transform.position = new Vector3 ((x + mouseX)/2, (y + mouseY)/2, z);
			}
		}
		*/
		//Debug for camera/mouse/player pos
		//Debug.Log ("Camera" + transform.position + "Mouse: " + mouseX + ", " + mouseY + "Player: " + x + ", " + y);
	}
}
