﻿using UnityEngine;
using System.Collections;

public class RoboRifle : MonoBehaviour {

	// position of rifle: x=2.3; y=0
	public float fireRate = 10;
	public float damage = 10;
	public LayerMask whatToHit;

	public float recoil = 25;
	public float kickback = 1;

	public Transform bulletShellPrefab;
	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	float effectSpawnRate;

	float timeToFire = 0;
	Transform firePoint;
	Transform shellOutPoint;
	Transform armP;

	bool Right;
	bool returnTriggered = false;
	private Animator ammoBeltAnim;

	Sprite[] hands;

	// Use this for initialization
	void Awake () {
		ammoBeltAnim = transform.FindChild("Ammo").GetComponent<Animator>();

		hands = Resources.LoadAll<Sprite> ("Sprites/Hands_1000x500");

		if (ammoBeltAnim == null) 
			Debug.LogError ("ERROR: No AmmoBeltSprite!");

		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) 
			Debug.LogError ("ERROR: No firePoint!");

		shellOutPoint = transform.FindChild ("ShellOutPoint");
		if (shellOutPoint == null)
			Debug.LogError ("ERROR: No ShellOutPoint!");

		armP =  transform.parent;
		if (armP == null)
			Debug.LogError ("ERROR: No arm!");
		
	}

	void Start (){
		effectSpawnRate = fireRate;
	}

	// Update is called once per frame
	void Update () {
		ammoBeltAnim.SetFloat ("FireRate", fireRate);

		//Start shooting
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				transform.FindChild ("Arm").GetComponent<SpriteRenderer>().sprite = hands[7];
				Shoot ();
			}
		}
		else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				transform.FindChild ("Arm").GetComponent<SpriteRenderer>().sprite = hands[7];
				timeToFire = Time.time + 1/fireRate;
				Shoot ();
			}
		}

		//Stop shooting
		if (Input.GetButtonUp ("Fire1")) {
			transform.FindChild ("Arm").GetComponent<SpriteRenderer>().sprite = hands[6];
			ammoBeltAnim.SetBool ("Shoot", false);
		}
	}

	void Shoot () {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, 100, whatToHit);
		if (Time.time >= timeToSpawnEffect) {
			Effect ();
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}

		Debug.DrawLine (firePointPosition, (mousePosition-firePointPosition)*100, Color.cyan);
		if (hit.collider != null) {
			//Debug.DrawLine (firePointPosition, hit.point, Color.red);
			//Debug.Log ("We hit " + hit.collider.name + " and did " + damage + " damage.");
		}
	}

	void Effect () {		
		Right = GameObject.Find ("Robo1").GetComponent<PlatformerCharacter2D> ().facingRight;

		ammoBeltAnim.SetBool("Shoot", true);

		Transform bulletShellClone;
		int shellForceX = Random.Range (200, 400);
		int shellForceY = 700;

		float size = Random.Range (0.9f, 1.5f);

		if (Right) {
			if(!returnTriggered)
				StartCoroutine(WeaponRecoil ());
			Instantiate (BulletTrailPrefab, shellOutPoint.position, Quaternion.Euler(0,0,firePoint.eulerAngles.z));
			bulletShellClone = Instantiate (bulletShellPrefab, shellOutPoint.position,  shellOutPoint.rotation) as Transform;
			bulletShellClone.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(shellForceX * (-1), shellForceY));
		} else {
			if(!returnTriggered)
				StartCoroutine(WeaponRecoil ());
			Instantiate (BulletTrailPrefab, shellOutPoint.position, Quaternion.Euler(0,0,firePoint.eulerAngles.z + 180));
			bulletShellClone = Instantiate (bulletShellPrefab, shellOutPoint.position, Quaternion.Euler(0,0,shellOutPoint.eulerAngles.z + 180)) as Transform;
			bulletShellClone.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(shellForceX*-1, shellForceY*-1));
		}
		Transform MuzzleFlash = Instantiate (MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		MuzzleFlash.parent = firePoint;
		MuzzleFlash.localScale = new Vector3 (size, size, size);
		Destroy (MuzzleFlash.gameObject, 0.02f);
	}
	IEnumerator WeaponRecoil () {
		returnTriggered = true;
		float angle= Random.Range(-5 , recoil)/10;
		Vector3 originPos = transform.localPosition;
		Quaternion originRot = transform.localRotation;
		transform.localPosition = Vector3.MoveTowards (transform.localPosition, new Vector3 (transform.localPosition.x-(kickback/10), transform.localPosition.y, transform.localPosition.z), 3);
		transform.localRotation = Quaternion.Euler( 0, 0 , angle);

		if (fireRate < 10){
			yield return new WaitForSeconds (0.1f);
		} else {
			yield return new WaitForSeconds (1 / fireRate);
		}
		transform.localRotation = originRot;
		transform.localPosition = originPos;

		returnTriggered = false;
	}


}
