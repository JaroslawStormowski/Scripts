using UnityEngine;
using System.Collections;

public class RoboHand : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D collision)
	{
		GameObject.Find ("_GM").transform.GetComponent<Hit> ().Effect (collision);
	}
}
