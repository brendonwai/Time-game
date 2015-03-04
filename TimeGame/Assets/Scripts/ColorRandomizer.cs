using UnityEngine;
using System.Collections;

public class ColorRandomizer : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		InvokeRepeating("ColorChange",1.0f, 4.0f);
	}

	void ColorChange(){
		float red = Random.Range(0f,1.0f);
		float blue = Random.Range(0f,1.0f);
		float green = Random.Range(0f,1.0f);

		GetComponent<Renderer>().material.color = new Color(red,blue,green, 1.0f);
	}
}
