using UnityEngine;
using System.Collections;
 
public class PickUpChest : MonoBehaviour {
 
void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			Destroy (gameObject);
            Debug.Log ("why wont you work ;_;");
		}
	}
}