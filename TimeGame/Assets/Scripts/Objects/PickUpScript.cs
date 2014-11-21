using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {
	public int energy=20;
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag=="Player"){
			other.SendMessage("GainEnergyBoost",energy);
			Destroy(gameObject);
		}
	}
}