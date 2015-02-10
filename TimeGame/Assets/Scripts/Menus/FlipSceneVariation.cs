using UnityEngine;
using System.Collections;

public class FlipSceneVariation : MonoBehaviour {

	//Don't enable this unless you're testing.

	// Use this for initialization
	void Start () {
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Level");
		foreach(GameObject obj in allObjects) {
			obj.transform.localScale = new Vector3 (obj.transform.localScale.x * -1, obj.transform.localScale.y, obj.transform.localScale.z);
		}
	}
}
