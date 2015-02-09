using UnityEngine;
using System.Collections;

public class HPDroneRunFaster : MonoBehaviour {

	public float moveFastSpeed = 1.5f;
	public float OldMoveSpeed;



	// Use this for initialization
	void Start () {
		OldMoveSpeed = GetComponentInParent<HealthDroneAI> ().moveSpeed;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GetComponentInParent<HealthDroneAI> ().moveSpeed = moveFastSpeed;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GetComponentInParent<HealthDroneAI> ().moveSpeed = OldMoveSpeed;
		}
	}
}
