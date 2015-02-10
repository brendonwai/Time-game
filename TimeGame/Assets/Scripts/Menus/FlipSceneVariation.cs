using UnityEngine;
using System.Collections;
using Tiled2Unity;

public class FlipSceneVariation : MonoBehaviour {

	//Don't enable this unless you're testing.

	// Use this for initialization
	void Start () {
		Debug.Log ("" + Random.Range(0, 2));
		if (Random.Range (0, 2) == 1) {	//50% chance to flip the map
			transform.position = new Vector3 (transform.position.x + GetComponent<TiledMap>().GetMapWidthInPixelsScaled(),
			                                      transform.position.y, transform.position.z);	//Must be before the localScale change.
			transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

			/*	Use only if you have multiple differnet maps in the level
			GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Level");
			foreach(GameObject obj in allObjects) {
			obj.transform.position = new Vector3 (obj.transform.position.x + obj.GetComponent<TiledMap>().GetMapWidthInPixelsScaled(),
			                                      obj.transform.position.y, obj.transform.position.z);	//Must be before the localScale change.
			obj.transform.localScale = new Vector3 (obj.transform.localScale.x * -1, obj.transform.localScale.y, obj.transform.localScale.z);
			}
			*/
		}

	}
}
