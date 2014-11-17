﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	
	public GameObject obj; // object that will be spawned
	public GameObject world; // the world in which the object will spawn
	public float timeMin;
	public float timeMax;
	public int countMax;
	Bounds worldBounds;
	List<GameObject> objects; // to keep track how many objects have spanwed

	// Use this for initialization
	void Start () {
		worldBounds = world.GetComponent<EdgeCollider2D>().bounds;
		StartCoroutine(Spawn());
		objects = new List<GameObject>();
	}
	
	IEnumerator Spawn() {
		while(true) {
			Vector3 location = new Vector3(
				Random.Range(-worldBounds.extents.x, worldBounds.extents.x),
				Random.Range(-worldBounds.extents.y, worldBounds.extents.y),
				0f);

			// if an object has been killed/picked up, remove it from the list
			for(int i = objects.Count - 1; i > -1; i--) { 
				if(objects[i] == null){
					objects.RemoveAt(i);
				}
			}

			// create an object if there are less than the max on the map
			if(objects.Count < countMax) {
				GameObject clone = (GameObject) Instantiate(obj, location, Quaternion.identity);

				clone.transform.parent = obj.transform.parent;
				objects.Add (clone);
			}

			yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
		}
	}
}