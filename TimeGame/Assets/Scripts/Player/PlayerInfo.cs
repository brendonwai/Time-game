using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {


	public int Health=100;
	//Hacking Energy, max=100
	public int Energy=0;
	//Amount of energy regenerated per second
	public int EnergyRegen=1;

	void Start(){
		InvokeRepeating("RegenerateEnergy",1,1);
	}

	void RegenerateEnergy(){
		if (Energy<100){
			Energy+=EnergyRegen;
		}
	}
	void GainEnergyBoost(int EnergyBoost){
		if (Energy+EnergyBoost<=100)
		{
			Energy+=EnergyBoost;
		}
		else
		{
			Energy=100;
		}
	}

}
