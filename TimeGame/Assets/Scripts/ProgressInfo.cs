using UnityEngine;
using System.Collections;

public class ProgressInfo : MonoBehaviour { 

	//All variables that you want to keep track of between scenes put it here.

	public static GameObject player;

	public const int FinalLevelNum = 6;
	//Note: level 0 is at num 2 so beat 3 levels to reach the final level
	//And please put any new scenes you make inbetween Level 0 and FinalLevel

	public const string FinalLevelName = "FinalLevel";	
	static public int levelNum = 2;	//Current level. Set to 2 (because of BuildSettings numbering) by default

	void Awake() {
	}

	static public void StartingLevel(int levelChoice) {
		levelNum = levelChoice;
		Application.LoadLevel(levelChoice);
	}

	static public void ChangeLevel(bool increment, int level) {
		//If you just want to increment the level, then ChangeLevel(true, 1);
		//If you want to go to a specific level, then ChangeLevel(false, #oflevelyouwant );
		player = GameObject.FindGameObjectWithTag("Player");
		if (increment) {
			levelNum++;
		}
		else {
			levelNum = level;
		}

		SaveStats.saveStats (player.GetComponentInChildren<PlayerInfo> ().Health,
		                     player.GetComponentInChildren<PlayerInfo> ().Energy,
		                     0);

		//Checking if you hit the final level
		if (levelNum >= FinalLevelNum) {
			Application.LoadLevel(FinalLevelName);	//Or replace with the name
		}
		else {
			Application.LoadLevel(levelNum);	//If we only have 1 level that we keep reusing, Replace with levelNum in the future or specific level names
			//Application.LoadLevel(levelNum);	//What it should actually be (only if we have multiple levels that don't get reused)
		}
	}
}
