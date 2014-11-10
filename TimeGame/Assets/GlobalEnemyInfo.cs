using UnityEngine;
using System.Collections;

public class GlobalEnemyInfo : MonoBehaviour {
	public int CanSeePlayer;

	// Update is called once per frame
	void FixedUpdate () {
		CanSeePlayer = 0;
	}
}
