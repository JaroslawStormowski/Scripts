using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyData : MonoBehaviour {
	//string name;
	Dictionary<string, float[]> enemyData;
	public float[] data = new float[5] ;
	int len;
	//Setting enemy data table
	void Awake(){
		enemyData = new Dictionary <string, float[]>(){
		//	health, speed, seeingRange, fireRange, damage
			{"Humm", new float[] { 100f, 5f, 25f, 15f, 10f} },
			{"Heli", new float[] { 100f, 10f, 30f, 15f, 10f} },
			{"Apache", new float[] { 150f, 10f, 30f, 20f, 20f} },
		};
	} 

	//Looking for certain data
	public float[] GetEnemyData(string name) {
		if (name.Contains (" (")) {
			string[] arr = name.Split(' ');
			name = arr [0];
		}
			
		if (enemyData.ContainsKey (name)) {
			data = enemyData [name];

		} else {
			print ("ERROR: Enemy data not found!");
		}

		//print (name);
		//print (data[0]);

		return(data);

	}

}
