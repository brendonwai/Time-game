using UnityEngine;
using System.Collections;

public class CollectRange : MonoBehaviour {

	public float speed;

	void OnTriggerStay2D(Collider2D other){
		if (other.tag=="PickUp")
			other.GetComponent<Rigidbody2D>().velocity=(transform.position-other.transform.position).normalized*speed;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag=="PickUp")
			other.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
	}
}
