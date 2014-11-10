using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject[] enemies;
	public GameObject world;
	private Bounds worldBounds;

	// Use this for initialization
	void Start () {
		worldBounds = world.GetComponent<EdgeCollider2D>().bounds;
		StartCoroutine(Spawn());
	}
	
	IEnumerator Spawn() {
		while(true) {
			bool invalidLocation = true;
			Vector3 location = new Vector3(
				Random.Range(-worldBounds.extents.x, worldBounds.extents.x),
				Random.Range(-worldBounds.extents.y, worldBounds.extents.y),
				0f);
			Instantiate(enemies[Random.Range(0, enemies.Length)], location, Quaternion.identity);
			// Random enemy every 15 to 20 seconds
			yield return new WaitForSeconds(Random.Range(15.0f, 20.0f));
		}
	}
}
