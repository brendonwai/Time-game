using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	GameObject PlayerStatus;
	GameObject UI_GameOver;

	void Awake(){
		PlayerStatus = GameObject.FindGameObjectWithTag("Player");
		UI_GameOver = GameObject.FindGameObjectWithTag("GameOverUI");
		UI_GameOver.active = false;		//Makes GameOver UI invisible
		Time.timeScale = 1.0f;
	}

	void Update(){
		if(PlayerStatus.GetComponent<Player2DController>().GameOver)
			RunGameOver ();
	}

	void RunGameOver(){
		Time.timeScale = 0.0f;	//Pauses the game
		UI_GameOver.active = true;		//Makes Gameover UI visible 

	}
}
