using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {

	public GameObject Connector2LinkBot;		//For creating connectors in Tiled Objects
	public GameObject Connector1LinkBotLeft;
	//public GameObject Connector1LinkBotRight;

	GameObject[] connectors;
	int i = 0;

	void Awake(){
		CreateObjectAtTag(Connector2LinkBot, "Connector2LinkBot",1);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotLeft",1);
		CreateObjectAtTag(Connector1LinkBotLeft, "Connector1LinkBotRight",-1);
	}
	

	//Instantiates given objects at given tag
	void CreateObjectAtTag(GameObject createMe, string object_tag, int x_scale){
		connectors = GameObject.FindGameObjectsWithTag(object_tag);
		foreach(GameObject temp in connectors){
			Vector3 position = new Vector3((temp.transform.position.x+0.16f), temp.transform.position.y, -100f);
			GameObject clone = (GameObject)Instantiate(createMe, position, Quaternion.identity);
			clone.transform.parent = temp.transform.parent;
			SetXScale(clone,x_scale);

		}
	}

	//Sets x scale for object
	void SetXScale(GameObject setMe, int scale){
		Vector3 theScale = setMe.transform.localScale;
		theScale.x *= scale;
		setMe.transform.localScale = theScale;
	}

}
