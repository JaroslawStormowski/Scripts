using UnityEngine;
using System.Collections;

public class Camera_controler : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	private bool facingRight; 
	private int cameraCon;

	private Vector3 ob_pos;
	private Vector3 mouse_pos;
	private Vector3 delta;
	private Vector3 destination;
	private Vector3 dir;

	float forward;
	float back;
	float upDown;

	// Update is called once per frame
	void Update () {

		cameraCon = Mathf.RoundToInt(GameObject.Find ("Robo1").transform.Find ("Arm_position/RoboWeapon").GetComponent<RoboWeapon> ().weaponType);

		switch (cameraCon) {
		case 0:
			{
				forward = 0.1f;
				back = 0.1f;
				upDown = 0.1f;
				break;}
		case 1:
			{
				forward = 0.5f;
				back = 0.1f;
				upDown = 0.5f;
				break;}
		case 2:
			{
				forward = 0.8f;
				back = 0.1f;
				upDown = 0.5f;
				break;}
		default:
			{
				forward = 0.5f;
				back = 0.1f;
				upDown = 0.5f;
				break;}
		}

		if (target) {
			facingRight = target.GetComponent<PlatformerCharacter2D> ().facingRight;

			ob_pos = target.position;
			mouse_pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			dir = mouse_pos - ob_pos;

			delta = mouse_pos - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 1));

			//delta = new Vector3 (delta.x * 0.7f, delta.y * 0.7f, delta.z * 0.7f );
		
			if (facingRight) {
				if (dir.x > 0)
					delta = new Vector3 (delta.x * forward, delta.y * upDown, delta.z );
				if (dir.x < 0)
					delta = new Vector3 (delta.x * back, delta.y * upDown, delta.z);
			} else {
				if (dir.x > 0)
					delta = new Vector3 (delta.x * back, delta.y * upDown, delta.z);
				if (dir.x < 0)
					delta = new Vector3 (delta.x * forward, delta.y * upDown, delta.z);
			}



			destination = ob_pos + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

			/*Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
             Vector3 destination = transform.position + delta;
             transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			 */
		}

	}
}
