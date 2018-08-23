using UnityEngine;
using System.Collections;

public class BulletShellController : MonoBehaviour {

	bool Right;
	float rotate;
	// Use this for initialization
	void Start () {
		Right = GameObject.Find ("Robo1").GetComponent<PlatformerCharacter2D> ().facingRight;
		Destroy (gameObject, 2);
		rotate = Random.Range (200, 400);
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y < 3.1f) {
			Destroy (gameObject);
		}

		if (transform) {
			if (Right) {
				transform.Rotate (new Vector3 (0, 0, rotate * Time.deltaTime));
			} else {
				transform.Rotate (new Vector3 (0, 0, rotate*-1 * Time.deltaTime));
			}
		}


	}
}
