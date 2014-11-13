using UnityEngine;
using System.Collections;

public class GUIPanelToggle : MonoBehaviour {

	public GameObject myPanel;

	// Use this for initialization
	void Update () {
		if(Input.GetKeyDown("space")){
			myPanel.SetActive(false);
		}
	}

}
