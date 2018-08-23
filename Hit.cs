using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour {

	public Transform metalHitPrefab;
	public Transform fleshHitPrefab;
	public Transform groundHitPrefab;
	private Transform effectPrefab;
	private Transform effectClone;

	private float damage;

	// Use this for initialization
	public void Effect (Collision2D collision) {

		ContactPoint2D hit = collision.contacts[0];

		damage = GameObject.Find("Robo1").transform.Find("Arm_position/RoboWeapon").GetComponent<RoboWeapon>().damage;

		if (collision.gameObject.tag == "flesh") {
			effectPrefab = fleshHitPrefab;
			effectClone = Instantiate (effectPrefab, hit.point, transform.rotation) as Transform;
			Destroy (effectClone.gameObject, 1.0f);
			collision.gameObject.GetComponent<EnemyStats>().health -= damage;

		} else if (collision.gameObject.tag == "metal") {
			print ("metal hit");
			effectPrefab = metalHitPrefab;
			effectClone = Instantiate (effectPrefab, hit.point, transform.rotation ) as Transform;
			Destroy (effectClone.gameObject, 2.1f);
			collision.gameObject.GetComponent<EnemyStats>().health -= damage;
			string s = "DAMAGE: " + damage;
			print (s);

		} else if (collision.gameObject.tag == "ground") {
			print ("ground hit");
			effectPrefab = groundHitPrefab;
			effectClone = Instantiate (effectPrefab, hit.point, Quaternion.Euler(-90,0,0)) as Transform;
			Destroy (effectClone.gameObject, 1.5f);
		}
	}

}
