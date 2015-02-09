using UnityEngine;
using System.Collections;

public class PushBack : MonoBehaviour {

	public float speed;

	void OnTriggerStay2D(Collider2D other){

		if (other.tag == "Enemy") {
			Transform enemy=other.transform.parent;
			Vector2 endPosition=enemy.position+(enemy.position-transform.position).normalized;
			enemy.position=Vector3.Lerp (enemy.position,endPosition,speed*Time.deltaTime);
		}
		if (other.tag=="Bullet"){
			Destroy(other.gameObject);
		}
	}

}