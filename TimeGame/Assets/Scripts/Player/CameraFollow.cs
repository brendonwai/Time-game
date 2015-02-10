using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform Player;
	static float z = -1000f;

    public float radius = 0f;
    public float lerp = 0f;

    private Vector2 previousPosition;

	// Use this for initialization
	void Start () {
        previousPosition = new Vector2(Player.position.x, Player.position.y);
		transform.position = new Vector3(Player.position.x, Player.position.y, z);
	}

	// Update is called once per frame
	void Update () {
        Vector2 playerPosition = new Vector2(Player.position.x, Player.position.y);
		Vector2 mouse = new Vector2(Camera.main.ScreenToWorldPoint (Input.mousePosition).x,
                                    Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

        Vector2 distance = mouse - playerPosition;

        if(distance.sqrMagnitude > radius * radius)
        {
            distance.Normalize();
            distance *= radius;
        }

        Vector2 targetPosition = playerPosition += distance;
        Vector2 lerpedPosition = Vector2.Lerp(previousPosition, targetPosition, lerp);

        transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, z);
        previousPosition = lerpedPosition;
	}
}
