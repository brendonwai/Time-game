using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	GameObject PlayerStatus;
	GameObject UI_GameOver;

	public Text MainComment;
	public Text comment;
	bool paused = false;

	//Comment Stuff
	int i = -1;						//Keeps track of what comment to display
	int num_comments = 7;			//How many comments to cycle through
	int d = -1;
	int death_count = 7;
	bool DeathPrefSet = false;

	void Awake(){
		PlayerStatus = GameObject.FindGameObjectWithTag("Player");
		UI_GameOver = GameObject.FindGameObjectWithTag("GameOverUI");
		UI_GameOver.SetActive(false);		//Makes GameOver UI invisible
		Time.timeScale = 1.0f;
		d = PlayerPrefs.GetInt("Death Count");
	}

	void Update(){
		if(PlayerStatus.GetComponent<Player2DController>().GameOver){
			if(!DeathPrefSet){
				SetDeathPref();
			}
			RunGameOver ();
		}
		else
			RunPause();
	}


	//Increments death count
	void SetDeathPref(){
		if(d<death_count)
			d++;
		else
			d = 0;
		PlayerPrefs.SetInt("Death Count", d);
	}

	void RunPause(){
		MainComment.text = "Paused";
		if(Input.GetKeyDown(KeyCode.P)&&!paused){
			PlayerStatus.GetComponent<Player2DController>().paused = true;
			
			if(i<num_comments)
				i++;
			else
				i=0;
			comment.text = PauseComment(i);
			
			paused = true;
			UI_GameOver.SetActive(true);
			Time.timeScale = 0.0f;
		}
		else if(Input.GetKeyDown(KeyCode.P)&&paused)
			ResumeGame();

	}

	void RunGameOver(){
		SaveStats.ResetHealth ();
		DeathPrefSet = true;
		MainComment.text = "Game Over";
		comment.text = GameOverComment(d);
		Time.timeScale = 0.0f;	//Pauses the game
		UI_GameOver.SetActive(true);		//Makes Gameover UI visible 

	}

	public void ResumeGame(){
		paused = false;
		PlayerStatus.GetComponent<Player2DController>().paused = false;
		SaveStats.ResetHealth();
		UI_GameOver.SetActive(false);
		Time.timeScale = 1.0f;
	}

	string GameOverComment(int selection){
		switch(selection){
		case 1: return "Third times a charm.";
		case 2: return "I lied. Fourth time. Tofu Man is rooting for you.";
		case 3: return "You can do it. Tofu Man believes in you.";
		case 4: return "...";
		case 5: return "You're making Tofu Man cry.";
		case 6: return "Tofu Man expected better from you";
		case 7: return "";
		default: return "";
		}
	}
	
	//Sets what comment to put during pause screen
	string PauseComment(int selection){
		switch(selection){
		case 1: return "!!!";
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
