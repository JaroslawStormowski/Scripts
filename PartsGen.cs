using UnityEngine;
using System.Collections;

public class PartsGen : MonoBehaviour {
	private Sprite[] partSprites;
	private int i;
	// Use this for initialization
	void Start () {
		partSprites = Resources.LoadAll<Sprite>("Sprites/PartzA_100x100");
		i = Random.Range (1, 35);
		gameObject.GetComponent<SpriteRenderer> ().sprite = partSprites [i];
		StartCoroutine (destroyObject());
	}
	IEnumerator destroyObject(){
		yield return new WaitForSeconds (3);

		for (float alpha = 1f; alpha >= 0; )
		{
			gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(0.5f);
			alpha -= 0.1f;
		}
		Destroy(this.gameObject);
}
}
