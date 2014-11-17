using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public float scale = 1.0f;

	void Update(){
		Time.timeScale = scale;
	}
}
