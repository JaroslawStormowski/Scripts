using UnityEngine;
using System.Collections;

public class keep_dir : MonoBehaviour {

	public float speed = 10.0f;
	public float[] vector = new float[3];
	public bool useMoveSlope;
	float velocity;
	Vector3 prevPosition;
	Vector3 targetDir;
	Vector3 posDiff;
	float angle;

	// Use this for initialization
	void Awake () {
		vector [0] = 0;
		vector [1] = 1;
		vector [2] = 0;
	}
	void moveSlope(float velocity){
		vector [0] = (velocity/7) * -1;
		vector [1] = 1;
	}

	void Update(){
		if (useMoveSlope) {
			velocity = (transform.position.x - prevPosition.x) / Time.deltaTime;
			prevPosition = transform.position;
			moveSlope (velocity);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		

		targetDir = new Vector3 (transform.position.x + vector[0], transform.position.y + vector[1], transform.position.z + vector[2]);
		posDiff = targetDir - transform.position;
		angle = Mathf.Atan2 (posDiff.y, posDiff.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(0,0,angle-90f);

	}
}
