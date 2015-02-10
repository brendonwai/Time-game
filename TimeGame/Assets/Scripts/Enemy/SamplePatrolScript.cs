using UnityEngine;
using System.Collections;

public class SamplePatrolScript : MonoBehaviour {

	public GameObject[] wayPoints;		//Stores waypoints
	public float moveSpeed = 2.0f;		//Sets speed of traversal
	int length;							//Stores array length information
	int index = 0;						//Indicates which position to move to


	// Use this for initialization
	void Awake () {
		length = wayPoints.Length;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position == wayPoints[index].transform.position){
			index++;
			if(index >= length)
				index = 0;
		}
	
		transform.position = Vector2.MoveTowards(transform.position,
			                                     wayPoints[index].transform.position,
			                                     moveSpeed * Time.deltaTime);

	}
}
