using UnityEngine;
using System.Collections;

public class TofuController : MonoBehaviour {

	public GameObject target;
	public float moveSpeed = 0.1f;
	private Animator anim;
	private bool outsideRadius;

	private bool facingRight = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		outsideRadius = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (outsideRadius) {
			ApproachTarget ();
		}
		else {
			anim.SetBool ("MovingUp", false);
		}

	}

	void ApproachTarget(){
		FaceTarget();

		transform.position = Vector2.MoveTowards(transform.position,
		                                         target.transform.position,
		                                         moveSpeed * Time.deltaTime);
		if (Vector2.Distance (transform.position, target.transform.position) > 2) {
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 5);
		}
		if(target.transform.position.y != transform.position.y || target.transform.position.x != transform.position.x)
		{
			anim.SetBool("MovingUp",true);
		}
		else
		{
			anim.SetBool("MovingUp",false);
		}
	}

	void FaceTarget(){
		if((target.transform.position.x >= transform.position.x) && !facingRight)
			Flip();
		if((target.transform.position.x < transform.position.x) && facingRight)
			Flip ();
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
	}

	/*void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Ignoring?");
			Physics2D.IgnoreCollision(other.gameObject.collider2D, collider2D);
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			outsideRadius = true;
		}
	}*/

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			outsideRadius = false;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			outsideRadius = true;
		}
	}
}
