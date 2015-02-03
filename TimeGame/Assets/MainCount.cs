using UnityEngine;
using System.Collections;

public class MainCount : MonoBehaviour {

	public int death_count = -1;
	public int pause_count = -1;

	void Awake(){
		DontDestroyOnLoad(this);
	}

	public void SetPause(int value){
		pause_count = value;
	}
}
