using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {


	public int Health=100;
	public Slider healthBar;
	//Hacking Energy, max=100
	public int Energy=0;
	public Slider energyBar;
	//Amount of energy regenerated per second
	public int EnergyRegen=1;

	void Start(){
		InvokeRepeating("RegenerateEnergy",1,1);
	}

	void RegenerateEnergy(){
		if (Energy<100){
			Energy+=EnergyRegen;
			energyBar.value=Energy;
		}
	}
	void GainEnergyBoost(int EnergyBoost){
		if (Energy+EnergyBoost<=100)
		{
			Energy+=EnergyBoost;
			energyBar.value=Energy;
		}
		else
		{
			Energy=100;
			energyBar.value = Energy;
		}
	}

}
