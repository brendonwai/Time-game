using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {
	

	GameObject[] Connectors;		//Stores connectors
	Transform parent;
	int ConnectorCount = 0;
	bool hacked = false;
	float alpha = 1.0f;				//For making gate fade

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
			InvokeRepeating("OpenGate",1.0f,20.0f);
	}

	void OpenGate(){
		if(FadeOut()){
			CancelInvoke("OpenGate");
			this.gameObject.SetActive(false);
		}
	}


	//Makes gate fade away
	bool FadeOut(){
		if(alpha > 0){
			alpha -= .1f;
			GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha);
			return false;
		}
		else
			return true;
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
