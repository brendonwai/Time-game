using UnityEngine;
using System.Collections;

public class MortarBotAI : MonoBehaviour {

	private GameObject player;
	private GameObject targetReticle;	//Child target reticle

	private const float attackRate = 2.5f;
	private const int attackDamage = 10;
	private const float timeUntilExplosion = 1f;
	private float lastAttack = 0f;

	private bool targetLocked;			//To keep the target reticle on a position after firing.
	private bool inRange;
	private Animator anim;
	private Animator explosionAnim;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		targetReticle = transform.FindChild("TargetReticle").gameObject;
		targetReticle.GetComponent<SpriteRenderer>().enabled = false;
		targetReticle.GetComponent<SpriteRenderer>().color = Color.red;
		targetLocked = false;
		inRange = false;
		anim = GetComponent<Animator>();
		explosionAnim = targetReticle.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!targetLocked && inRange) {
			targetReticle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.1f, player.transform.position.z);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			inRange = true;
			targetReticle.GetComponent<SpriteRenderer>().enabled = true;
			if (Time.time >= lastAttack + attackRate) {
				lastAttack = Time.time;	//Update last attack timestamp

				targetLocked = true;
				targetReticle.GetComponent<SpriteRenderer>().color = Color.black;
				targetReticle.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.1f, player.transform.position.z);	//Put target reticle on player position

				StartCoroutine ("MortarAttack");
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			inRange = false;
			if (!targetLocked) {
				targetReticle.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}

	IEnumerator MortarAttack() {
		anim.SetBool("isAttacking", true);
		yield return new WaitForSeconds(0.183f);		//Fire Animation Time
		anim.SetBool("isAttacking", false);

		//PROJECTILE SHOOTS HERE

		yield return new WaitForSeconds(timeUntilExplosion);

		//EXPLOSION EFFECT GOES HERE. PROJECTILE DISAPPEARS
		explosionAnim.SetBool("attackHit", true);

		if (targetReticle.GetComponent<AOEAttack>().inCollider) {
			player.SendMessage("takeDamage", attackDamage);
		}

		yield return new WaitForSeconds (0.483f);
		if (!inRange) {
			targetReticle.GetComponent<SpriteRenderer>().enabled = false;
		}
		explosionAnim.SetBool("attackHit", false);
		targetReticle.GetComponent<SpriteRenderer>().color = Color.red;
		targetLocked = false;
	}
}
