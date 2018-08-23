using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour {
	float[] data = new float[5];
	string name; 
	public float health;
	public float maxHealth;
	public float speed;
	public float seeingRange;
	public float fireRange;
	public float damage;
	public int damageDone;

	bool smoking=false;
	bool atFire=false;
	public bool Exploded=false;
	public bool alive=true;

	public bool facingRight=true;
	public bool inSeeRange;
	public bool inFireRange;
	public Vector3 targetPos;

	// Use this for initialization
	void Start() {
		name = gameObject.name;
		//	health, speed, seeingRange, fireRange, damage
		data= GameObject.Find("_GM").GetComponent<EnemyData>().GetEnemyData(name);
		health = data[0];
		speed = data[1];
		seeingRange = data[2];
		fireRange = data[3];
		damage = data[4];

		maxHealth = health;
		/*print (name);
		print (health);
		print (alive);*/
		if (health==0 || speed ==0 || seeingRange == 0 || fireRange==0) Debug.Log("DATA TRANSFER ERROR!");

		//if(this.name == "Apache")
			//print ("Apache" + data[0]);
	}
	
	// Update is called once per frame
	void Update () {
		//Alive or not
		if (health <= maxHealth / 10)
			alive = false;

		// Damage 1
		if (health > (maxHealth/4) && health <= (maxHealth / 2) && alive && !smoking) {
			if (gameObject.tag.Equals("metal"))
				this.GetComponent<EnemyDamage>().Damage1();
			smoking = true;
		}

		// Damage 2
		if (health <= (maxHealth / 4) && !atFire) {
			if (gameObject.tag.Equals("metal"))
				this.GetComponent<EnemyDamage>().Damage2 ();
			atFire=true;
		}


		// Die
		if (health <= 0 && !Exploded) {
			if (gameObject.tag.Equals("metal"))
				this.GetComponent<EnemyDamage>().Die ();
			Exploded=true;
		}
	}
		

}
