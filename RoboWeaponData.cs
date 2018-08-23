using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoboWeaponData : MonoBehaviour {

	private Dictionary<string, float[]> roboWeapons;
	private float[] weaponData;

	private Dictionary<string, Transform> roboWeaponsPref;
	private Transform weaponPref;

	public Transform roboHandPref;
	public Transform roboRevolverPref;
	public Transform roboPistolPref;
	public Transform roboRiflePref;
	public Transform roboMiniGunPref;
	public Transform roboGrenadeLauncherPref;
	public Transform roboSniperRifle;
	public Transform roboRocketLauncher;

	void Awake (){
		//roboWeaponsArray = new string[] {"RoboRevolver","RoboPistol","RoboRifle","RoboMiniGun","RoboGrenadeLauncher","RoboSniperRifle","RoboRocketLauncher",};
		roboWeapons = new Dictionary<string, float[]> {
			//                              damage, fireRate, ammo, reloadTime, armRotSpeed, weaponType
			{"RoboHand", new float[] 			{30f,   0f,    0f, 0.5f,  0.8f, 0f}},
			{"RoboRevolver", new float[] 		{10f,   0f,    6f,   3f,  0.2f, 1f}},	
			{"RoboPistol", new float[] 			{10f,   10f,  20f,   3f,  0.2f, 1f}},
			{"RoboRifle", new float[] 			{10f,   10f, 100f,   3f,  0.1f, 1f}},
			{"RoboMiniGun", new float[] 		{20f,   30f, 100f,   3f, 0.07f, 2f}},
			{"RoboGrenadeLauncher", new float[] {50f,   0f,    6f,   3f, 0.03f, 1f}},
			{"RoboSniperRifle", new float[] 	{1000f, 0f,    3f,   3f, 0.03f, 1f}},
			{"RoboRocketLauncher", new float[] 	{100f,  0f,    1f,   3f, 0.03f, 1f}},
		};

		roboWeaponsPref = new Dictionary<string, Transform> { 
			{"RoboHand", roboHandPref},
			//{"RoboRevolver", roboRevolverPref},
			//{"RoboPistol", roboPistolPref},
			{"RoboRifle", roboRiflePref},
			{"RoboMiniGun", roboMiniGunPref},
			//{"RoboGrenadeLauncher", roboGrenadeLauncherPref},
			//{"RoboSniperRifle", roboSniperRifle},
			//{"RoboRocketLauncher", roboRocketLauncher},
		};
	}

	public float[] GetRoboWeaponData(string name){
		if (roboWeapons.ContainsKey (name)) {
			weaponData = roboWeapons [name];
		} else {
			Debug.Log("ERROR: Weapon data not found!");
		}
		return weaponData; 
	}

	public Transform GetRoboWeaponPref(string name){
		if (roboWeaponsPref.ContainsKey (name)) {
			weaponPref = roboWeaponsPref [name];
		} else {
			Debug.Log ("ERROR: Weapon prefab not found!");
		}
		return weaponPref;
	}
}
