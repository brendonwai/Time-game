﻿using UnityEngine.UI;

using UnityEngine;
using System.Collections;

public class SkillButtonHandler : MonoBehaviour {
	
	public Image[] buttons;				//Stores cooldown image over buttons
	public float[] CoolDownDuration;	//Duration of cooldown
	public bool[] onCD;					//Determines if button is on CD
	public Text[] countDown;			//Counts down seconds to cooldown
	int i;

	// Use this for initialization
	void Start () {
		i = 0;
		foreach(Image cover in buttons){
			cover.fillAmount = 0.0f;
			//CoolDownDuration[i] = 1.0f/CoolDownDuration[i]/100.0f;
			onCD[i] = false;
			i++;
		}
		InvokeRepeating("CheckCD", 0f, .01f);
	
	}
	
	//Starts skill cool dow 
	public void StartCD(int skill){
		if(!onCD[skill]){
			buttons[skill].fillAmount = 1.0f;
			onCD[skill] = true;
			countDown[skill].text = CoolDownDuration[skill].ToString();
		}
	}

	//Checks all buttons for if on CD and runs CD effect
	void CheckCD(){
		for(int n = 0; n<i; n++)
			RunCD(n);
	}

	//Runs CD effect
	void RunCD(int skill){
		if(buttons[skill].fillAmount > 0.0f){
			buttons[skill].fillAmount = buttons[skill].fillAmount - 1.0f/CoolDownDuration[skill]/100.0f;
			countDown[skill].text = (CoolDownDuration[skill]*buttons[skill].fillAmount).ToString("0");
		}
		else{
			onCD[skill] = false;
			countDown[skill].text = "";
		}
	}


}
