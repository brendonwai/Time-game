using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform Player;		//Player object
	static float z = -10f; 			//Camera z position

	//Camera constraints
	public float MaxHorizonView = 0f;		//Max distance in x direction
	public float MaxVerticalView = 0f;		//Max distance in y direction


	// Use this for initialization
	void Start () {
		//Screen.showCursor = true;		//not sure if this works
		transform.position = new Vector3(Player.position.x, Player.position.y, z);
	}
	// Update is called once per frame
	void Update () {
		float x = Player.position.x;
		float y = Player.position.y;

		float mouseX = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
		float mouseY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;

		maxViewSet(ref mouseX, ref mouseY, x,y);	//Prevents camera from going beyond max constraints


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

	//Sets camera position to be within max range
	void maxViewSet(ref float cam_x, ref float cam_y, float char_x, float char_y)
	{
		if(cam_x > MaxHorizonView+char_x){
			cam_x = MaxHorizonView + char_x;
		}
		if(cam_x < char_x-MaxHorizonView){
			cam_x = char_x-MaxHorizonView;
		}
		if(cam_y > MaxVerticalView+char_y){
			cam_y = MaxVerticalView + char_y;
		}
		if(cam_y < char_y-MaxVerticalView){
			cam_y = char_y-MaxVerticalView;
		}
	}
}
