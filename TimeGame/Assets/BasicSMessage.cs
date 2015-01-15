using UnityEngine;
using System.Collections;

public class BasicSMessage : MonoBehaviour {

	//Applies damage to Enemy
	IEnumerator takeDamage(int damage){
		//reduce health by amount of damage
		GetComponentInParent<EnemyInfo>().Health -= damage;
		//sprite flashes red upon taking damage
		GetComponentInParent<Renderer>().material.color = Color.red;
		yield return new WaitForSeconds (.1f);
		GetComponentInParent<Renderer>().material.color =Color.white;
	}
}
