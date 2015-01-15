using UnityEngine;
using System.Collections;

public class EnemyHack : MonoBehaviour {
	
	public bool inRadius = false;	//Determines if enemy is in Hack Radius
	public int EnergyCost = 0;		//How much energy needs to be spent to hack
	
	//Colors
	Color OutOfHackRadius = new Color(.6f, .1f, .1f, 1f);
	Color InHackRadius = new Color(0f, .4f, .5f, 1f);
	
	// Use this for initialization
	void Start () {
	}
	
	void OnMouseOver(){
		if(inRadius){
			renderer.material.color = InHackRadius;
			if(Input.GetMouseButtonDown(1)){}
		}
		else
			renderer.material.color = OutOfHackRadius;
	}
	
	void OnMouseExit(){
		renderer.material.color = Color.white;
	}
}
