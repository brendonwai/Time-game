using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject obj; // object that will be spawned
	public GameObject world; // the world in which the object will spawn
	public float timeMin;
	public float timeMax;
	private Bounds worldBounds;

	// Use this for initialization
	void Start () {
		worldBounds = world.GetComponent<EdgeCollider2D>().bounds;
		StartCoroutine(Spawn());
	}
	
	IEnumerator Spawn() {
		while(true) {
			Vector3 location = new Vector3(
				Random.Range(-worldBounds.extents.x, worldBounds.extents.x),
				Random.Range(-worldBounds.extents.y, worldBounds.extents.y),
				0f);
		
			GameObject clone = (GameObject) Instantiate(obj, location, Quaternion.identity);

			clone.transform.parent = obj.transform.parent;

			// Random enemy within the specified time frame
			yield return new WaitForSeconds(Random.Range(timeMin, timeMax));
		}
	}
}