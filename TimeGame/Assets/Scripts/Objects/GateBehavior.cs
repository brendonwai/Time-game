using UnityEngine;
using System.Collections;

public class GateBehavior : MonoBehaviour {

	public GameObject[] connectors;		//Stores connectors needed to be hacked

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
