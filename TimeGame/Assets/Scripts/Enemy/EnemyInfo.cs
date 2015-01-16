using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour {
	//Stores enemy information to exchange with others

	//Alert System
	public bool TargetInSight = false;			//Determines if target is in sight
	public bool Alerted = false;				//Triggers alert state from other enemies
	

	//Player Accessible
	public int enemyType = 0;
	public int requiredEnergy;
	/*
		*Tells the player which enemy is being hacked/interacted
		*with.
		0 = Basic Melee
		1 = Basic Ranged
		2 = To be determined
		. . .
		n = etc
		
	 */
	public int Health = 100;					//Enemy health
	public string Type = "Enemy";


}
