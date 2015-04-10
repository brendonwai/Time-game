using UnityEngine;
using System.Collections;

public class SaveStats : MonoBehaviour {

	private static int playerHealth = 100;
	private static float playerEnergy = 20;
	private static int playerScore;

	void Awake() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.gameObject.GetComponentInChildren<PlayerInfo> ().Health;
		playerEnergy = player.gameObject.GetComponentInChildren<PlayerInfo> ().Energy;
		DontDestroyOnLoad (this);
		playerScore = 0;
	}
	
	public static void saveStats(int health, float energy, int score) {
		playerHealth = health;
		playerEnergy = energy;
		playerScore = score;
	}

	public static void ResetHealth() {
		playerHealth = 100;
	}

	public static int GetPlayerHealth() {
		return playerHealth;
	}

	public static float GetPlayerEnergy() {
		return playerEnergy;
	}

	public static int GetPlayerScore() {
		return playerScore;
	}
}
