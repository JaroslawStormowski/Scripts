using UnityEngine;
using System.Collections;

public class HeliDie : MonoBehaviour {

	public Transform explosionPref;
	public Transform smokePref;
	public Transform firePref;
	public Transform partsPref;

	private Transform smoke;
	private Transform fire;
	private Transform explosion;
	//private Transform obj;
	private Transform parts;
	private int forceX;
	private int forceY;



	private string objectName;
	private int damage;
	public float health;
	public float maxHealth;
	public bool alive=true;

	// Use this for initialization
	void Start () {
		if (gameObject.name.Contains("Humm")) 
			maxHealth = 100f;
		if (gameObject.name.Contains("Heli")) 
			maxHealth = 100f;

		health = maxHealth;
		damage = 0;
	}

	// Update is called once per frame
	void Update () {

		// Damage 1
		if (health > 0 && health <= (maxHealth / 2) && alive && damage == 0) {
			if (gameObject.name.Contains ("Humm"))
				Damage1 ();
			//gameObject.GetComponent<HummDie>().Damage1();		
			if (gameObject.name.Contains("Heli"))
				Damage1 ();
			//gameObject.GetComponent<HeliDie>().Damage1();	
			damage += 1;
		}

		// Damage 2
		if (health > 0 && health <= (maxHealth / 4) && alive && damage == 1) {
			if (gameObject.name.Contains("Humm"))
				Damage2 ();
			//gameObject.GetComponent<HummDie>().Damage2();
			if (gameObject.name.Contains("Heli"))
				Damage2 ();
			//gameObject.GetComponent<HeliDie>().Damage2();	
			damage += 1;
		}


		// Die
		if (health <= 0 && alive) {
			alive = false;
			if (gameObject.name.Contains("Humm"))
				Die ();
			//gameObject.GetComponent<HummDie>().Die();
			if (gameObject.name.Contains("Heli"))
				Die ();
			//gameObject.GetComponent<HeliDie>().Die();			
		}
	}




	public void Damage1() {
		smoke = Instantiate (smokePref, transform.position, Quaternion.Euler(-90,0,0)) as Transform;
		smoke.parent = transform;
	}


	public void Damage2() {
		if (smoke != null)
			Destroy(smoke.gameObject);;

		fire = Instantiate (firePref, transform.position, Quaternion.Euler(0,0,0)) as Transform;
		fire.parent = transform;
	}

	public void Die(){
		int a = Random.Range (3, 5);
		for (int i = 0; i < a; i+=1) {
			parts = Instantiate (partsPref,transform.position, Quaternion.Euler(0,0,0)) as Transform;
			forceX=Random.Range(-20,20);
			forceY=Random.Range(15,50);

			parts.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceX, forceY));
			parts.GetComponent<Rigidbody2D> ().AddTorque(forceX/5);
		}

		explosion = Instantiate (explosionPref, transform.position, Quaternion.Euler(-90,0,0)) as Transform;

		forceX=Random.Range(-100,100);
		forceY=Random.Range(150,500);

		gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (forceX, forceY));
		Destroy (explosion.gameObject, 3f);


		if (fire != null)
			StartCoroutine (destroyObject());


		StartCoroutine (destroyObject());
	}



	IEnumerator destroyObject(){
		yield return new WaitForSeconds (3);

		gameObject.GetComponent<PolygonCollider2D> ().enabled = false;


		for (float alpha = 1f; alpha >= 0; )
		{

			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);

			fire.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, alpha);
			fire.FindChild ("FireSmoke").GetComponent<ParticleSystem> ().startColor = new Color (1f, 1f, 1f, alpha);

			yield return new WaitForSeconds(0.5f);
			alpha -= 0.1f;
		}

		Destroy(this.gameObject);
		Destroy(fire.gameObject);
	}
}
