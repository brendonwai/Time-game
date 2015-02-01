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
	public Slider energyBar;
	public Text energyNum;
	public Text hundred;
	public int maxEnergy;
	//Amount of energy regenerated per second
	public float EnergyRegen = 0.25f;
	

	void Start(){
		InvokeRepeating("RegenerateEnergy", 1, 3.0f);
		hundred.text = "/ " + maxEnergy.ToString();
	}

	void RegenerateEnergy(){
		if (Energy < maxEnergy){
			Energy += EnergyRegen;
			float displayedEnergy = Mathf.Floor (Energy);
			energyBar.value = displayedEnergy;
			energyNum.text = displayedEnergy.ToString();
		}
	}
	void GainEnergyBoost(int EnergyBoost){
		if (Energy + EnergyBoost <= maxEnergy)
		{
			Energy += EnergyBoost;
			energyBar.value = Energy;
			energyNum.text = Energy.ToString();
		}
		else
		{
			Energy = maxEnergy;
			energyBar.value = Energy;
			energyNum.text = Energy.ToString(); 
		}
	}
	public IEnumerator HealthDrain() {
		bool notDead = true;
		while (notDead && HealthDrainActive) {
			Health -= 20;
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

}
