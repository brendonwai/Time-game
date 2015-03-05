using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {

	//Bottom Facing Connectors
	public GameObject Connector2LinkBot;		//For creating connector with 2 links
	public GameObject Connector1LinkBotLeft;	//For creating connector with 1 link
	public GameObject ConnectorNoLinkBot;

	//Side Facing Connectors
	public GameObject Connector2LinkSide;		//For creating connector with 2 links
	public GameObject Connector1LinkSide;		//For creating connector with 1 link
	public GameObject ConnectorNoLinkSide;

	//Gate
	public GameObject GateH;						//For creating horizontal gate
	public GameObject GateV;						//For creating vertical gate

	//For Coloring
	public GameObject[] LevelElements;

	//For Randomized level coloring
	float LowerColorRange = .4f;
	float UpperColorRange = .9f;
	Color LevelColor;


	GameObject[] connectors;

	void Awake(){
		GenerateLevelColor();
		ApplyLevelColor();
		CreateConnectors();
		CreateGate();
	}

	//Generates a random color
	void GenerateLevelColor(){
		float red = Random.Range(LowerColorRange,UpperColorRange);
		float blue = Random.Range(LowerColorRange,UpperColorRange);
		float green = Random.Range(LowerColorRange,UpperColorRange);
		LevelColor = new Color(red,blue,green, 1.0f);
	}

	//Applies generated color to level elements in list
	void ApplyLevelColor(){
		foreach(GameObject element in LevelElements){
			element.GetComponent<Renderer>().material.color = LevelColor;
		}
	}

	//Creates Gate of all types on map
	//NOTE: NEVER CALL BEFORE CONNECTORS ARE CREATED
	void CreateGate(){
		CreateObjectAtTag(GateH, "GateH",1,1, Quaternion.identity,.48f,-.04f,-1);
		CreateObjectAtTag(GateV, "GateV",1,1, Quaternion.Euler(0,0,90),.16f,-.48f,-1);
	}


	//Creates all types on map
	void CreateConnectors(){
		//Creates bottom facing connectors
		CreateObjectAtTag(Connector2LinkBot, "Connector2LinkBot",1,1, Quaternion.identity,.16f,-.04f,0);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotLeft",1,1,Quaternion.identity,.16f,-.04f,0);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotRight",-1,1, Quaternion.identity,.16f,-.04f,0);
		CreateObjectAtTag(ConnectorNoLinkBot, "ConnectorNoLinkBot",1,1, Quaternion.identity,.16f,-.04f,0);

		//Creates right facing connectors
		CreateObjectAtTag(Connector2LinkSide,"Connector2LinkRight",1,1, Quaternion.Euler(0,0,90),.04f,-.16f,2);
		CreateObjectAtTag(Connector1LinkSide,"Connector1LinkRightTop",-1,1, Quaternion.Euler(0,0,90),.04f,-.16f,2);
		CreateObjectAtTag(Connector1LinkSide,"Connector1LinkRightBot",1,1, Quaternion.Euler(0,0,90),.04f,-.16f,2);
		CreateObjectAtTag(ConnectorNoLinkSide,"ConnectorNoLinkRight",1,1, Quaternion.Euler(0,0,90),.04f,-.16f,2);

		//Creates left facing connectors
		CreateObjectAtTag(Connector2LinkSide,"Connector2LinkLeft",1,-1, Quaternion.Euler(0,0,90),.28f,-.16f,1);
		CreateObjectAtTag(Connector1LinkSide,"Connector1LinkLeftTop",-1,-1, Quaternion.Euler(0,0,90),.28f,-.16f,1);
		CreateObjectAtTag(Connector1LinkSide,"Connector1LinkLeftBot",1,-1, Quaternion.Euler(0,0,90),.28f,-.16f,1);
		CreateObjectAtTag(ConnectorNoLinkSide,"ConnectorNoLinkLeft",1,-1, Quaternion.Euler(0,0,90),.28f,-.16f,1);

		//No longer need? Creates top facing connectors
		//CreateObjectAtTag(Connector2LinkBot, "Connector2LinkTop",1,-1, Quaternion.identity,.16f,-.32f);
		//CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkTopLeft",1,-1, Quaternion.identity,.16f,-.32f);
		//CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkTopRight",-1,-1, Quaternion.identity,.16f,-.32f);
	}

	//Instantiates given objects at given tag
	void CreateObjectAtTag(GameObject createMe, string object_tag, int x_scale, int y_scale, Quaternion rotation, float x_adjust, float y_adjust, int direction){
		connectors = GameObject.FindGameObjectsWithTag(object_tag);
		foreach(GameObject temp in connectors){
			Vector3 position = new Vector3((temp.transform.position.x+x_adjust), temp.transform.position.y+y_adjust, -100f);
			GameObject clone = (GameObject)Instantiate(createMe, position, rotation);

			if(direction != -1)
				clone.GetComponent<ObjectInfo>().orientation = direction;
			clone.GetComponent<Renderer>().material.color = LevelColor;

			clone.transform.parent = temp.transform.parent;
			SetXScale(clone,x_scale);
			SetYScale(clone,y_scale);
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
