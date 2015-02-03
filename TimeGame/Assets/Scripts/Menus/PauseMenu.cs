using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject UI_Pause;
	public Text comment;
	bool paused = false;
		
	int i = -1;						//Keeps track of what comment to display
	int num_comments = 7;			//How many comments to cycle through


	void Awake(){
		UI_Pause.SetActive(false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.P)&&!paused){
			if(i<num_comments)
				i++;
			else
				i=0;

			comment.text = PauseComment(i);

			paused = true;
			UI_Pause.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	public void ResumeGame(){
		paused = false;
		UI_Pause.SetActive(false);
		Time.timeScale = 1.0f;
	}


	//Sets what comment to put during pause screen
	string PauseComment(int selection){
		switch(selection){
			case 1: return "Tofu Man used to be in the game. But now he's gone.";
			case 2: return "Everything comes back to shrooms.";
			case 3: return "I really have nothing better to do.";
			case 4: return "I'm being serious right now.";
			case 5: return "Nothing.";
			case 6: return "...";
			case 7: return "(ﾉ◕ヮ◕)ﾉ*:･✧";
			default: return "";
		}
	}
	
}
