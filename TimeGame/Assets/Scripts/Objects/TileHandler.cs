using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {

	//Connectors
	public GameObject Connector2LinkBot;		//For creating connector with 2 links
	public GameObject Connector1LinkBotLeft;	//For creating connector with 1 link

	//Gate
	public GameObject Gate;						//For creating gate

	GameObject[] connectors;

	void Awake(){
		CreateConnectors();
		CreateGate();
	}

	//Creates Gate of all types on map
	//NOTE: NEVER CALL BEFORE CONNECTORS ARE CREATED
	void CreateGate(){
		CreateObjectAtTag(Gate, "GateH",1,1, Quaternion.identity,.48f,-.16f);
		CreateObjectAtTag(Gate, "GateV",1,1, Quaternion.Euler(0,0,90),.16f,-.48f);
	}


	//Creates all types on map
	void CreateConnectors(){
		CreateObjectAtTag(Connector2LinkBot, "Connector2LinkBot",1,1, Quaternion.identity,.16f,0f);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotLeft",1,1,Quaternion.identity,.16f,0f);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotRight",-1,1, Quaternion.identity,.16f,0f);
		CreateObjectAtTag(Connector2LinkBot,"Connector2LinkRight",1,1, Quaternion.Euler(0,0,90),0f,-.16f);
		CreateObjectAtTag(Connector1LinkBotLeft,"Connector1LinkRightTop",-1,1, Quaternion.Euler(0,0,90),0f,-.16f);
		CreateObjectAtTag(Connector1LinkBotLeft,"Connector1LinkRightBot",1,1, Quaternion.Euler(0,0,90),0f,-.16f);
		CreateObjectAtTag(Connector2LinkBot,"Connector2LinkLeft",1,-1, Quaternion.Euler(0,0,90),.32f,-.16f);
		CreateObjectAtTag(Connector1LinkBotLeft,"Connector1LinkLeftTop",-1,-1, Quaternion.Euler(0,0,90),.32f,-.16f);
		CreateObjectAtTag(Connector1LinkBotLeft,"Connector1LinkLeftBot",1,-1, Quaternion.Euler(0,0,90),.32f,-.16f);
		CreateObjectAtTag(Connector2LinkBot, "Connector2LinkTop",1,-1, Quaternion.identity,.16f,-.32f);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkTopLeft",1,-1, Quaternion.identity,.16f,-.32f);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkTopRight",-1,-1, Quaternion.identity,.16f,-.32f);
	}

	//Instantiates given objects at given tag
	void CreateObjectAtTag(GameObject createMe, string object_tag, int x_scale, int y_scale, Quaternion rotation, float x_adjust, float y_adjust){
		connectors = GameObject.FindGameObjectsWithTag(object_tag);
		foreach(GameObject temp in connectors){
			Vector3 position = new Vector3((temp.transform.position.x+x_adjust), temp.transform.position.y+y_adjust, -100f);
			GameObject clone = (GameObject)Instantiate(createMe, position, rotation);
			clone.transform.parent = temp.transform.parent;
			SetXScale(clone,x_scale);
			SetYScale(clone,y_scale);
			if(object_tag == "Gate")
				Debug.Log(clone.transform.parent.name);

		}
	}

	//Sets x scale for object
	void SetXScale(GameObject setMe, int scale){
		Vector3 theScale = setMe.transform.localScale;
		theScale.x *= scale;
		setMe.transform.localScale = theScale;
	}

	//Sets y scale for object
	void SetYScale(GameObject setMe, int scale){
		Vector3 theScale = setMe.transform.localScale;
		theScale.y *= scale;
		setMe.transform.localScale = theScale;
	}

}
