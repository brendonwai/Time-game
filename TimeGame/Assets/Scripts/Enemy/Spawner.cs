using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	
	public GameObject obj; // object that will be spawned
	public GameObject world; // the world in which the object will spawn
	public float timeMin;
	public float timeMax;
	public int countMax;
	public GameObject parent;
	public List<GameObject> spawnPoints;
	Bounds worldBounds;
	List<GameObject> objects; // to keep track how many objects have spanwed
	
	// Use this for initialization
	void Start () {
		//worldBounds = world.GetComponent<EdgeCollider2D>().bounds;
		objects = new List<GameObject>();
		StartCoroutine(Spawn());
	}
	
	IEnumerator Spawn() {
		while(true) {
			/*Vector3 location = new Vector3(
				Random.Range(world.transform.position.x, -world.transform.position.x),
				Random.Range(-world.transform.position.y, world.transform.position.y),
				0f);*/
			Vector3 location = spawnPoints[Random.Range (0, spawnPoints.Count)].transform.position;
			
			// if an object has been killed/picked up, remove it from the list
			for(int i = objects.Count - 1; i > -1; i--) {
				if(objects[i] == null){
					objects.RemoveAt(i);
				}
			}
			
			// create an object if there are less than the max on the map
			if(objects.Count < countMax) {
				GameObject clone = (GameObject) Instantiate(obj, location, Quaternion.identity);
				
				clone.transform.parent = parent.transform;
				objects.Add (clone);
			}
			
			yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
		}
	}
}