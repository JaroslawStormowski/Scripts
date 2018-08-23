using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

	bool alive;
	float health;
	float maxHealth;
	//float damageByTime;
	int damageTime;

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


	void FixedUpdate(){
		alive = gameObject.GetComponent<EnemyStats> ().alive;
		health = gameObject.GetComponent<EnemyStats> ().health;
	}




	public void Damage1() {
		smoke = Instantiate (smokePref, transform.position, Quaternion.Euler(0,0,0)) as Transform;
		smoke.parent = transform;
	}


	public void Damage2() {
		if (smoke != null)
			Destroy(smoke.gameObject);;
		fire = Instantiate (firePref, transform.FindChild("ControlPoint").position, Quaternion.Euler(0,0,0)) as Transform;
		fire.parent = transform;
		maxHealth = gameObject.GetComponent<EnemyStats> ().maxHealth;
		StartCoroutine(damageOverTime (maxHealth*0.03f, 0));
	}


	public void Die(){

		Explosion1 ();

		forceX=Random.Range(-100,100);
		forceY=Random.Range(150,500);

		gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (forceX, forceY));
		Destroy (explosion.gameObject, 3f);

		StartCoroutine (destroyObject());
	}



	IEnumerator destroyObject(){
		yield return new WaitForSeconds (3);

		gameObject.GetComponent<PolygonCollider2D> ().enabled = false;

		for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
		{
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
			transform.FindChild ("MoveAnim").gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f,1f,1f,alpha);
			if (fire != null) {
				fire.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, alpha);
				fire.FindChild ("FireSmoke").GetComponent<ParticleSystem> ().startColor = new Color (1f, 1f, 1f, alpha);
			}

			if (smoke != null) {
				smoke.FindChild ("Smoke").GetComponent<ParticleSystem> ().startColor = new Color (1f, 1f, 1f, alpha);
			}
				
			yield return new WaitForSeconds(0.5f);
		}

		if(fire != null)
			Destroy(fire.gameObject);
		if (smoke != null)
			Destroy (smoke.gameObject);
		
		Destroy(this.gameObject);
	}


	IEnumerator damageOverTime(float damageByTime, int damageTime){
		if (damageTime == 0 || damageTime == null)
			damageTime = 9999;
		int i=0;
		while (health > 0 && i < damageTime) {
			this.GetComponent<EnemyStats> ().health -= damageByTime;
			yield return new WaitForSeconds (1);
			i += 1;
		}
	
	}

	public void Explosion1(){
		int a = Random.Range (3, 5);
		for (int i = 0; i < a; i+=1) {
			parts = Instantiate (partsPref,transform.position, Quaternion.Euler(0,0,0)) as Transform;
			forceX=Random.Range(-20,20);
			forceY=Random.Range(15,50);

			parts.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceX, forceY));
			parts.GetComponent<Rigidbody2D> ().AddTorque(forceX/5);
		}

		explosion = Instantiate (explosionPref, transform.position, Quaternion.Euler(-90,0,0)) as Transform;

		if(transform.FindChild("EnemyWeapon") != null)
			Destroy (transform.FindChild("EnemyWeapon").gameObject, 0);
		if(transform.FindChild("Accessories") != null)
			Destroy (transform.FindChild("Accessories").gameObject, 0);
	}


}
