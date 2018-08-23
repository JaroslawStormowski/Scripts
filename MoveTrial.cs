using UnityEngine;
using System.Collections;

public class MoveTrial : MonoBehaviour {

	public Transform metalHitPrefab;
	public Transform fleshHitPrefab;
	public Transform groundHitPrefab;
	private Transform effectPrefab;
	private Transform effectClone;
	public int moveSpeed = 320;
	public float damage;

	void Start ()
	{
		//GameObject robo = GameObject.Find ("Robo1");
		//damage = robo.transform.Find("Arm_position/RoboRifle").GetComponent<Weapon> ().damage;
		damage = GameObject.Find("Robo1").transform.Find("Arm_position/RoboWeapon").GetComponent<RoboWeapon>().damage;
		Destroy (gameObject, 1);
	}
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);

	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		GameObject.Find ("_GM").transform.GetComponent<Hit> ().Effect (collision);
		Destroy (gameObject);
	}
}
