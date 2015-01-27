using UnityEngine;
using System.Collections;

public class AntiFlip : MonoBehaviour {
	public void Flip() {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
