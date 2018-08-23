using UnityEngine;
using System.Collections;

public class AimAtPlayer : MonoBehaviour {


	bool inSeeRange;
	bool inFireRange;
	bool vehicleFacingRight; 
	bool facingRight;
	Vector3 targetPos;
	Vector3 dir;
	float angle;
	public float speed = 5f;
	Vector3 scale;

	bool firing;

	Transform firePoint;
	public Transform muzzleFlashPrefab;


	// Use this for initialization
	void Start () {
		facingRight = transform.parent.parent.GetComponent<EnemyStats> ().facingRight;
		firePoint = transform.FindChild ("FirePoint");
	}
	
	// Update is called once per frame
	void Update () {

		vehicleFacingRight = transform.parent.parent.GetComponent<EnemyStats> ().facingRight;
		inSeeRange = transform.parent.parent.GetComponent<EnemyStats> ().inSeeRange;
		inFireRange = transform.parent.parent.GetComponent<EnemyStats> ().inFireRange;

		if (inSeeRange)
			targetPos = transform.parent.parent.GetComponent<EnemyStats> ().targetPos;
		else {
			if (facingRight != vehicleFacingRight)
				flip ();
			if (vehicleFacingRight)
				targetPos = new Vector3 (transform.position.x + 3, transform.position.y + 1, transform.position.z);
			else
				targetPos = new Vector3 (transform.position.x - 3, transform.position.y + 1, transform.position.z);
		}

		//Start fire
		if(inFireRange && !firing) 
			StartCoroutine(fire());
			

		//Weapon possition issues
		dir = targetPos - transform.position;
		angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

		//Flip toward target
		if (vehicleFacingRight) {
			if (dir.x > 0) {
				if (!facingRight)
					flip ();
			} else {
				if (facingRight)
					flip ();
			}
		} else {
			if (dir.x > 0) {
				if (facingRight)
					flip ();
			} else {
				if (!facingRight)
					flip ();
			}
		}

		//Set right rotate angle base on case
		if (facingRight && vehicleFacingRight)
			angle = angle;
		if (!facingRight && vehicleFacingRight)
			angle = angle + 180f;
		if (facingRight && !vehicleFacingRight)
			angle = -angle - 180f;
		if (!facingRight && !vehicleFacingRight)
			angle = -angle;

		transform.localRotation = Quaternion.Euler (0f, 0f, angle);


	}

	void flip(){
		scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		facingRight = !facingRight;
	} 

	IEnumerator fire(){
		firing = true;

		int i_cycl;
		if (this.name == "Rifle") {
			i_cycl = Random.Range (5, 10);
		} else if (this.name == "MiniGun") {
			i_cycl = Random.Range (15, 25);
		} else {
			i_cycl = Random.Range (2, 3);
		}

		for (int i=0; i<=i_cycl; i++){
			
			Transform MuzzleFlash = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
			MuzzleFlash.parent = firePoint;
			MuzzleFlash.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			Destroy (MuzzleFlash.gameObject, 0.02f);

			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (1);
		firing=false;
	}
		
}
