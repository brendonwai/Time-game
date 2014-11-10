using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour {
	//Stores enemy information to exchange with others
	
	public bool TargetInSight = false;			//Determines if target is in sight
	public bool Alerted = false;				//Triggers alert state from other enemies

	void FixedUpdate(){

		if(TargetInSight)
			GetComponentInParent<GlobalEnemyInfo>().CanSeePlayer += 1;
	}

}
