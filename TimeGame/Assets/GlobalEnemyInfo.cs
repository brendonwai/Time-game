using UnityEngine;
using System.Collections;

public class GlobalEnemyInfo : MonoBehaviour {

	public int CanSeePlayer;			//How many enemies on the map can see the player
											//Used to deactivate Alert status
	void Awake(){
		CanSeePlayer = 0;
		}

	void FixedUpdate(){
		CanSeePlayer = 0;
	}
}

