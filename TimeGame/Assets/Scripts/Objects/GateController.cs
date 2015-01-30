using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {
	

	GameObject[] Connectors;		//Stores connectors
	Transform parent;
	int ConnectorCount = 0;
	bool hacked = false;

	// Use this for initialization
	//NOTE: VERY IMPORTANT THAT THIS STAY AS Start() AND NOT Awake()
	void Start () {
		Connectors = new GameObject[transform.parent.childCount];
		foreach(Transform child in transform.parent)
			if(child.name.IndexOf("Link")!=-1)
				Connectors[ConnectorCount++] = child.gameObject;
	}

	void FixedUpdate(){
		if(!hacked)
			hacked = (ConnectorCount == CountHacked());
		else
			this.gameObject.SetActive(false);
	}
	
	//Counts how many gates under the same parent are hacked
	int CountHacked(){
		int result = 0;
		for (int i = 0; i < ConnectorCount; i++)
			if(Connectors[i].GetComponent<ObjectInfo>().hacked)
				result += 1;
		return result;
	}
}
