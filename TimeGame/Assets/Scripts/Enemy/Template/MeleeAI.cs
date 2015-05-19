using UnityEngine;
using System.Collections;

public class MeleeAI : EnemyAI {
	public GameObject Energy;
	public float attackLength;
	float attackStart;
	float attackTime;
	public float chargeSpeed;
	float orgSpeed;

	public bool inContact;

	protected override void Awake () {
		base.Awake();
		inContact = false;
		attackStart = 0f;
		orgSpeed = moveSpeed;
	}

	protected override void Start(){
		base.Start();
	}

	protected override void Move() {
		if(!anim.GetBool("IsAttacking") && !GetComponentInChildren<MeleeAttack>().charging && !GetComponentInChildren<MeleeAttack>().onCoolDown) {
			ApproachTarget();
		}
	}

	protected override void RandomMovement() {
		if(!anim.GetBool("IsAttacking") && !GetComponentInChildren<MeleeAttack>().charging && !GetComponentInChildren<MeleeAttack>().onCoolDown) {
			base.RandomMovement();
		}
	}

	void Update() {
		if(anim.GetBool("IsAttacking")) {			
			ApproachTarget();
			if(attackStart == 0) {
				attackStart = Time.time;
				moveSpeed = chargeSpeed;
			}
			attackTime += Time.deltaTime;
			if(attackTime >= attackLength || inContact) {
				anim.SetTrigger("Attack");
				if(inContact) {
					target.GetComponent<Player2DController>().SendMessage("takeDamage", 20);
					//StartCoroutine(PlayAttack());
				}
				//anim.SetBool("Attack", false);
				attackTime = 0f;
				anim.SetBool("IsAttacking", false);
				moveSpeed = orgSpeed;
				attackStart = 0;
			}

		}
	}

	IEnumerator PlayAttack() {
		anim.SetBool("Attack", true);
		float len = anim.GetCurrentAnimatorStateInfo(0).length;
		Debug.Log(len);
		yield return new WaitForSeconds(len);
		anim.SetBool("Attack", false);
	}

	//Activates death sequence
	protected override void Dead(){
		base.Dead();
		Vector2 location = transform.position;
		for (int i=0;i<Random.Range(5.0f,15.0f);i++){
			Instantiate(Energy,new Vector2(transform.position.x+Random.Range(-1f,1f),transform.position.y+Random.Range(-1f,1f)),transform.rotation);
		}
		Destroy (gameObject,2.0f);
	}
}
