using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemplateEnemyAI : EnemyAI {
	public GameObject Energy;
	//Explosion
	public GameObject explosionRange;	//Explosion Range
	public GameObject explosion;		//Particle

	private float first_half_explosion_charge_time = 0.5f;	//Half because it'll check halfway through if the player is still in range
	private float second_half_explosion_charge_time = 0.3f;	//Half because it'll check halfway through if the player is still in range

	private bool player_in_explosion_range;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		explosionRange.SetActive (true);
		player_in_explosion_range = false;
	}

	protected override void Start(){
		base.Start();
	}

	//Deals damage to Player when touches him
	protected override void OnCollisionEnter2D(Collision2D col){
		base.OnCollisionEnter2D(col);
		if (col.gameObject.tag=="Player"){
			StartCoroutine(Explode ());
		}
	}

	IEnumerator Check_Before_Explode() {
		if (GetComponent<EnemyInfo> ().isHacked) {	//If the Kamikaze was hacked which caused the Death
			StartCoroutine(Explode());
		}
		else {
			stopMove = true;

			//PLAY SOME SIZZLE SOUND HERE OR SOMETHING

			yield return new WaitForSeconds (first_half_explosion_charge_time);					//Wait a bit

			if (explosionRange.GetComponent<TemplateEnemyAttack> ().player_in_range) {		//If Player is IN RANGE of the explosion

				yield return new WaitForSeconds (second_half_explosion_charge_time);

				StartCoroutine(Explode());

			}
			else {																			//If Player LEFT the explosion range then go on as normal
				stopMove = false;
			}
		}		
	}

	IEnumerator Explode(){

		target.GetComponent<Player2DController> ().SendMessage("takeDamage", 100);

		yield return new WaitForSeconds(0.1f);

		stopMove = true;

		//Create Energy Balls when Dead
		for (int i = 0; i < Random.Range(5.0f, 15.0f); i++){
			Instantiate(Energy, new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f)), transform.rotation);
		}

		Instantiate (explosion, transform.position, transform.rotation);	//Explosion Animation
		Destroy (gameObject, 0.375f);										//Destory Kamikaze After Explosion
	}



	protected override void Dead() {
		base.Dead();
		if (!alive) {	//If it died because it has <= 0 HP
			StartCoroutine(Explode());
		}
		else {
			StartCoroutine(Check_Before_Explode ());
		}
	}
}
