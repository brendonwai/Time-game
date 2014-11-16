using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour {
	//Stores enemy information to exchange with others

	//Alert System
	public bool TargetInSight = false;			//Determines if target is in sight
	public bool Alerted = false;				//Triggers alert state from other enemies

	//Enemy Stats
	public int Health = 100;					//Enemy health
	public string Name = "Enemy";

}
