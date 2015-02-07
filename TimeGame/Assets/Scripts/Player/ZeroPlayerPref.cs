using UnityEngine;
using System.Collections;

public class ZeroPlayerPref : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		PlayerPrefs.SetInt("Death Count",-1);
	}
}
