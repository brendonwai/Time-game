using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

	//Health
	public int Health = 100;
	public int PreHackHealth = 100;
	public Slider healthBar;
	public Text healthNum;

	//Hacking Energy, max=100
	public int Energy = 0;
	public Slider energyBar;
	public Text energyNum;
	//Amount of energy regenerated per second
	public int EnergyRegen = 1;

	void Start(){
		InvokeRepeating("RegenerateEnergy", 1, 1);
	}

	void RegenerateEnergy(){
		if (Energy < 100){
			Energy += EnergyRegen;
			energyBar.value = Energy;
			energyNum.text = Energy.ToString();
		}
	}
	void GainEnergyBoost(int EnergyBoost){
		if (Energy + EnergyBoost <= 100)
		{
			Energy += EnergyBoost;
			energyBar.value = Energy;
			energyNum.text = Energy.ToString();
		}
		else
		{
			Energy = 100;
			energyBar.value = Energy;
			energyNum.text = Energy.ToString();
		}
	}
	public void SwapPlayerToEnemyHealth(int EnemyHealth) {
		PreHackHealth = Health;
		Health = EnemyHealth;
		//healthBar.transform.FindChild ("Fill").GetComponent<Image> ().color = Color.white;			//This and the one under mess stuff up for some reason

	}
	public void SwapToPreHackHealth () {
		Health = PreHackHealth;
		PreHackHealth = 100;
		//healthBar.transform.FindChild ("Fill").GetComponent<Image> ().color = Color.red;				//This breaks some stuff
	}

}
