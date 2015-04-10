using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public GameObject player;

	//Loads scene based on given int index
	public void LoadScene(int index){
		if (index == -1) {
			Application.LoadLevel (Application.loadedLevel);
		}
		else
			ProgressInfo.StartingLevel(index);
	}
}
