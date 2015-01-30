using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start(){
		particleSystem.renderer.sortingLayerName = "Foreground";
		Destroy (this, .5f);
	}
}
