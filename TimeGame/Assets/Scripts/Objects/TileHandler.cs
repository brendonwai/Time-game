using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {

	public GameObject Connector2LinkBot;		//For creating connectors in Tiled Objects
	public GameObject Connector1LinkBotLeft;

	GameObject[] connectors;
	int i = 0;

	void Awake(){
		CreateObjectAtTag(Connector2LinkBot, "Connector2LinkBot");
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotLeft");
	}
	

	//Instantiates given objects at given tag
	void CreateObjectAtTag(GameObject createMe, string object_tag){
		connectors = GameObject.FindGameObjectsWithTag(object_tag);
		foreach(GameObject temp in connectors){
			Vector3 position = new Vector3((temp.transform.position.x+0.16f), temp.transform.position.y, -100f);
			Instantiate(createMe, position, Quaternion.identity);
		}
	}
}
