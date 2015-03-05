using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	//Health
	public int Health = 100;
	public int PreHackHealth;
	public Slider healthBar;
	public Text healthNum;
	public bool HealthDrainActive;

	//Hacking Energy, max=100
	public float Energy = 20;
	public Image energyBar;
	public int maxEnergy;
	//Amount of energy regenerated per second
	public int EnergyRegen = 1;
	

	void Awake(){
		Health = SaveStats.GetPlayerHealth ();
		Energy = SaveStats.GetPlayerEnergy ();
		UpdateEnergyBar ();
		healthBar.value = Health;
		healthNum.text = Health.ToString ();
		InvokeRepeating("RegenerateEnergy", 1, 10.0f);
	}

	void UpdateEnergyBar(){
		energyBar.fillAmount = Energy/maxEnergy;
	}

	void RegenerateEnergy(){
		if (Energy < maxEnergy){
			Energy += EnergyRegen;
			UpdateEnergyBar();
		}
	}
	void GainEnergyBoost(int EnergyBoost){
		if (Energy + EnergyBoost <= maxEnergy)
		{
			Energy += EnergyBoost;
			energyBar.fillAmount = Energy/maxEnergy;
		}
		else
		{
			Energy = maxEnergy;
			energyBar.fillAmount = Energy/maxEnergy;
		}
	}
	public IEnumerator HealthDrain() {
		bool notDead = true;
		while (notDead && HealthDrainActive) {
			Health -= 5;
			if(Health <= 0) {
				Health = 0;
				notDead = false;
			}
			healthBar.value = Health;
			healthNum.text = Health.ToString();
			yield return new WaitForSeconds (1f);
		}
		HealthDrainActive = false;
		StartCoroutine(GetComponent<Player2DController>().HackDeath());
	}
	public void SwapPlayerToEnemyHealth(int EnemyHealth) {
		PreHackHealth = Health;
		Health = EnemyHealth;
	}
	public void SwapToPreHackHealth () {
		Health = PreHackHealth;
	}

	//Spends energy and updates Energy Bar
	public void SpendEnergy(int cost){
		Energy -= cost;
		UpdateEnergyBar();
	}

}
