using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start(){
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Foreground";
		Destroy (this, .5f);
	}
}
