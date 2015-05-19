using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemyAI : EnemyAI {
	public GameObject Energy;
	public GameObject smoke;
	//Set by child script and collider
	//Made public so it's viewable by child

	public GameObject deadBody;

	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		foreach(Transform child in transform){
			if(child.gameObject.name == "smoke")
				smoke = child.gameObject;
		}
		smoke.SetActive(false);
	}

	protected override void Start(){
		base.Start();
	}

	void Update(){
		if(GetComponent<EnemyInfo>().Health<=25){
			smoke.SetActive(true);
		}
	}
	
	// Update is called once per frame
	protected override void FixedUpdate () {
		base.FixedUpdate();	
	}

	//Activates death sequence
	protected override void Dead(){
		base.Dead();
		Vector2 location = transform.position;
		for (int i=0;i<Random.Range(5.0f,15.0f);i++){
			Instantiate(Energy,new Vector2(transform.position.x+Random.Range(-1f,1f),transform.position.y+Random.Range(-1f,1f)),transform.rotation);
		}
		Destroy (gameObject,2.0f);
		Instantiate(deadBody, location, Quaternion.identity);
	}
}