using UnityEngine;
using System.Collections;

public class redScreen : MonoBehaviour {
	Color toColor = new Color (255, 0, 0, .005f);
	Color blendColor = new Color (255, 0, 0, 0);
	float speed=2;
	
	// Use this for initialization
	void Start () {
		renderer.material.color=new Color(255, 255, 255, 0);
	}

	IEnumerator FlashRedScreen(){
		renderer.material.color=new Color(255, 255, 255, .005f);
		yield return new WaitForSeconds(.1f);
		renderer.material.color=new Color(255, 255, 255, 0);
	}

	void DeathRedScreen(){
		renderer.material.color=Color.Lerp(blendColor,toColor,Time.deltaTime*speed);
	}

}


