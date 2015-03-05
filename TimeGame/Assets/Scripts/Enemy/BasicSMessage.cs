using UnityEngine;
using System.Collections;

public class BasicSMessage : MonoBehaviour {

	//Applies damage to Enemy
	IEnumerator takeDamage(int damage){
		if(GetComponentInParent<EnemyInfo>().Health>=0){
			//reduce health by amount of damage
			GetComponentInParent<EnemyInfo>().Health -= damage;
			if (GetComponentInParent<EnemyInfo> ().enemyType != 2) {	//Health Drone doesn't have a healthbar
				GetComponentInParent<EnemyInfo> ().healthBar.value = GetComponentInParent<EnemyInfo> ().Health;
			}
			//sprite flashes red upon taking damage
			GetComponentInParent<Renderer>().material.color = Color.red;
			yield return new WaitForSeconds (.1f);
			GetComponentInParent<Renderer>().material.color =Color.white;
		}
	}
}
