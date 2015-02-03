using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	public GameObject UI_Pause;
	public Text comment;
	public GameObject player;
	bool paused = false;
		
	int i = -1;						//Keeps track of what comment to display
	int num_comments = 7;			//How many comments to cycle through


	void Awake(){
		UI_Pause.SetActive(false);
	}

	void Update(){
		if(!player.GetComponent<Player2DController>().GameOver){
			if(Input.GetKeyDown(KeyCode.P)&&!paused){
				player.GetComponent<Player2DController>().paused = true;

				if(i<num_comments)
					i++;
				else
					i=0;
				comment.text = PauseComment(i);

				paused = true;
				UI_Pause.SetActive(true);
				Time.timeScale = 0.0f;
			}
			else if(Input.GetKeyDown(KeyCode.P)&&paused)
				ResumeGame();
		}
	}

	public void ResumeGame(){
		paused = false;
		player.GetComponent<Player2DController>().paused = false;
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
