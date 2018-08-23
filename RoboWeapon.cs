using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySampleAssets.CrossPlatformInput;

public class RoboWeapon : MonoBehaviour {

	public Transform weaponPref;
	private Transform weapon;

	public float[] weaponData;
	public string[] roboCarriedWeapons;
	int currentWeapon=1;
	public int maxWeapons;
	//private bool nextWeapon;
	//private bool previousWeapon;

	public float damage;
	public float fireRate;
	public float ammo;
	public float reloadTime;
	public float armRotSpeed;
	public float weaponType;

	// Use this for initialization
	void Start () {
		if (maxWeapons==0) maxWeapons = 3;
		//{"RoboRevolver","RoboPistol","RoboRifle","RoboMiniGun","RoboGrenadeLauncher","RoboSniperRifle","RoboRocketLauncher",};
		roboCarriedWeapons = new string[] {"RoboHand", "RoboRifle","RoboMiniGun",};

		switchWeapon (0);
	}

	void Update() {

		if (CrossPlatformInputManager.GetAxis ("Mouse ScrollWheel") > 0 || CrossPlatformInputManager.GetButtonDown("NextWeapon")) {
			switchWeapon (1);
		}
		if (CrossPlatformInputManager.GetAxis ("Mouse ScrollWheel") < 0 || CrossPlatformInputManager.GetButtonDown("PreviousWeapon")) {
			switchWeapon (-1);
		}
			
	}


	void switchWeapon(int change) {
		
		string weaponName = roboCarriedWeapons [currentWeapon-1];
		for (int child = 0; child < transform.childCount; child++)
			Destroy (transform.GetChild (child).gameObject);
		
		currentWeapon += change;
		if (currentWeapon > maxWeapons)
			currentWeapon = 1;
		if (currentWeapon < 1)
			currentWeapon = maxWeapons;

		//Set name
		weaponName = roboCarriedWeapons [currentWeapon-1];

		//Instantiate it
		weaponPref = GameObject.Find("_GM").GetComponent<RoboWeaponData> ().GetRoboWeaponPref (weaponName);
		weapon = Instantiate (weaponPref, new Vector3 (transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform) as Transform;

		//Update data
		weaponData = GameObject.Find ("_GM").GetComponent<RoboWeaponData> ().GetRoboWeaponData (weaponName);
		damage = weaponData[0];
     	fireRate = weaponData[1];
		ammo = weaponData[2];
		reloadTime = weaponData[3];
		armRotSpeed = weaponData[4];
		weaponType = weaponData[5];

		//Update data in other scripts
		transform.parent.GetComponent<ArmRotation>().armRotSpeed = armRotSpeed;
		transform.parent.GetComponent<ArmRotation> ().weaponType = weaponType;

		string a = "Currend weapon: " + weaponName;
		Debug.Log(a);
	}

	void replaceWeapon(){
		//jump = CrossPlatformInputManager.GetButtonDown("Jump");
		//h = CrossPlatformInputManager.GetAxis("Horizontal");
	}

}
