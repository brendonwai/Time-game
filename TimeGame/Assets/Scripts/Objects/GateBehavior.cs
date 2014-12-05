using UnityEngine;
using System.Collections;

public class GateBehavior : MonoBehaviour {

	public GameObject[] connectors;		//Stores connectors needed to be hacked
	public float XDirection = 0.0f;		//How far to move in x direction
	public float YDirection = 0.0f;		//How far to move in y direction
	public float moveSpeed = 1.0f;		//How fast gate moves

	int length;							//Length of list
	bool hacked = false;

	// Use this for initialization
	void Start () {
		length = connectors.Length;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!hacked){
			hacked = (length == CountHacked());
		}
		else{
			Destroy(gameObject,2);
		}
	}

	int CountHacked(){
		int result = 0;
		for (int i = 0; i < length; i++){
			if(connectors[i].GetComponent<ObjectInfo>().hacked)
				result += 1;
		}
		return result;
	}
}
