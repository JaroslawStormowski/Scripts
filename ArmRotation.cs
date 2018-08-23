using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	bool grounded;
    public int rotationOffset = 0;
	bool Right;
	float rotZ;

	public float weaponType = 0;
	public float armRotSpeed = 1;

	int upBlock;
	int lowBlock;

	// Update is called once per frame
	void Update () {
		Right = this.GetComponentInParent<PlatformerCharacter2D> ().facingRight;
		grounded = this.GetComponentInParent<PlatformerCharacter2D>().grounded;
		//Right = GameObject.Find ("Robo1").GetComponentInParent<PlatformerCharacter2D> ().facingRight;

		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

		if (weaponType != 0)
			MoveBlock (weaponType);

		if (Right) {
			rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;
			rotZ = rotZ + rotationOffset;
			if (weaponType != 0) {
				if (rotZ < -lowBlock && rotZ > -180)
					return;
					//rotZ = -lowBlock;
				if (rotZ < 180 && rotZ > upBlock)
					return;
					//rotZ = upBlock;
			}
			if (grounded) {
				if (rotZ < -30 && rotZ > -90)
					rotZ = -30;
				if (rotZ > -150 && rotZ < -90)
					rotZ = -150;
			}
		} else {
			rotZ = Mathf.Atan2 (difference.y*-1, difference.x*-1) * Mathf.Rad2Deg;
			rotZ = rotZ + rotationOffset;
			if (weaponType != 0) {
				if (rotZ > lowBlock && rotZ < 180)
					return;
					//rotZ = lowBlock;
				if (rotZ > -180 && rotZ < -upBlock)
					return;
					//rotZ = -upBlock;
			}
			if (grounded) {
				if (rotZ > 30 && rotZ < 90)
					rotZ = 30;
				if (rotZ < 150 && rotZ > 90)
					rotZ = 150;
			}
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, rotZ), armRotSpeed);
	}

	void MoveBlock(float weaponType){
		switch((int)weaponType){
		case 1:
			upBlock = 100;
			lowBlock = 100;
			break;
		case 2:
			upBlock = 70;
			lowBlock= 70;
			break;
		case 3:
			upBlock = 50;
			lowBlock= 50;
			break;
		default:
			upBlock = 100;
			lowBlock = 100;
			break;
		}
	
	}

}
