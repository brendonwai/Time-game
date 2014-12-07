using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {


	//Loads scene based on given int index
	public void LoadScene(int index){
		if(index == -1)
			Application.LoadLevel(Application.loadedLevel);
		else
			Application.LoadLevel(index);
	}
}
