using UnityEngine;
using System.Collections;

public class EnemyHover : MonoBehaviour {

	public bool inRange = false;	//Determines if object is in hacking range

	private Animator anim;				//Sets sprite image after hack
	private HackRadius hackRadius;		//Script needed in order to check whether the enemy is within the hackRadius
	
	private Color InHackRange = new Color(0f, .4f, .5f, 1f);
	
	// Use this for initialization
	void Start () {
		anim = this.transform.parent.GetComponent<Animator>();
		GameObject go = GameObject.Find("hackRadius");
		hackRadius = (HackRadius) go.GetComponent(typeof(HackRadius));
	}

	void OnMouseOver() {
		// Checks whether the user is mousing over the enemy and not one of its larger, surrounding colliders
		RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		bool mousingOver = false;
		for (int i = 0; i < hits.Length; i++) {
			if(hits[i].collider != null) {
				GameObject hitObject = hits[i].collider.gameObject;
				if(hitObject.transform.tag == "Enemy" && hackRadius.inRange(this.gameObject)) {
					renderer.material.color = InHackRange;
					mousingOver = true;
				}
			}
		}
		if (!mousingOver) { // If the user is not mousing over the enemy but mousing over one of its larger, surrounding colliders this is true
			renderer.material.color = Color.white;
		}
	}
	
	void OnMouseExit() {
		renderer.material.color = Color.white;
	}
}
