using UnityEngine;
using System.Collections;

public class ChangeLevelInGame : MonoBehaviour {

	public int Level = 0;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player")
			Application.LoadLevel(Level);
	}
}
