﻿using UnityEngine.UI;

using UnityEngine;
using System.Collections;

public class SkillButtonHandler : MonoBehaviour {
	
	public Image[] buttons;				//Stores cooldown image over buttons
	public float[] CoolDownDuration;	//Duration of cooldown
	public bool[] onCD;					//Determines if button is on CD
	

	// Use this for initialization
	void Start () {
		int i = 0;
		foreach(Image cover in buttons){
			cover.fillAmount = 0.0f;
			CoolDownDuration[i] = 1.0f/CoolDownDuration[i];
			onCD[i] = false;
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach(Image cover in buttons){
			if(cover.fillAmount > 0.0f){
				cover.fillAmount = cover.fillAmount - CoolDownDuration[i];
			}
			else
				onCD[i] = false;
			i++;
		}
	}

	public void StartCD(int skill){
		if(!onCD[skill]){
			buttons[skill].fillAmount = 1.0f;
			onCD[skill] = true;
		}
	}


}
