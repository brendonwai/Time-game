using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {


	//Loads scene based on given int index
	public void LoadScene(int index){
		Application.LoadLevel(index);
	}
}
