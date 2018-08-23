using UnityEngine;
using System.Collections;

public class HummAI : MonoBehaviour {

	float[] data = new float[5];
	public bool facingRight = true;
	public float speed;
	public float seeingRange;
	public float fireRange;
	public float damage;

	bool targetFound;
	public bool inFireRange;
	public bool inSeeRange;
	private Collider2D [] targetColliders = new Collider2D [1];


	bool exploded;
	bool alive;
	bool move;
	bool destroy=false;

	public LayerMask targetLayer;
	//public Transform target;
	public Vector3 targetPos;
	private float posTolerance;
	private Vector3 posDelta;
	private Transform controlPoint;
	Vector2 hummVelocity;


	// Use this for initialization
	void Start () {
		/*
		speed = this.GetComponent<EnemyStats> ().speed;
		seeingRange = this.GetComponent<EnemyStats> ().seeingRange;
		fireRange = this.GetComponent<EnemyStats> ().fireRange;
		damage = this.GetComponent<EnemyStats> ().damage;
		*/
		data= GameObject.Find("_GM").GetComponent<EnemyData>().GetEnemyData(name);
		//health = data[0];
		speed = data[1];
		seeingRange = data[2];
		fireRange = data[3];
		damage = data[4];

		controlPoint = transform.Find("ControlPoint");
		if (controlPoint == null)
			print ("Control point in Humm not found.");
		
		posTolerance = Random.Range (-10, 10) / 10;

	}
		

	// Move vehicle
	void Update () {
		this.GetComponent<EnemyStats> ().facingRight = facingRight;
		this.GetComponent<EnemyStats> ().inFireRange = inFireRange;
		this.GetComponent<EnemyStats> ().inSeeRange = inSeeRange;
		this.GetComponent<EnemyStats> ().targetPos = targetPos;

		alive = this.GetComponent<EnemyStats> ().alive;
		exploded = this.GetComponent<EnemyStats> ().Exploded;

		transform.FindChild ("MoveAnim").GetComponent<Animator> ().SetBool ("Alive", alive);
		transform.FindChild ("MoveAnim").GetComponent<Animator> ().SetBool ("Move", move);

		hummVelocity = GetComponent<Rigidbody2D> ().velocity;

		targetColliders = Physics2D.OverlapCircleAll(controlPoint.position, seeingRange, targetLayer);
		inSeeRange = Physics2D.OverlapCircle (controlPoint.position, seeingRange, targetLayer);
		inFireRange = Physics2D.OverlapCircle(controlPoint.position, fireRange + posTolerance, targetLayer);

		if (Mathf.Abs (hummVelocity.x) < 0.1)
			move = false;

		if(alive){

			// Target found
			if (inSeeRange) 
			{
				//targetPos = target.position;
				targetPos = targetColliders[0].attachedRigidbody.transform.position;
				posDelta = targetPos - transform.position;
				targetFound = true;
			}
			
			// Move toward targed
			if(targetFound && !inFireRange)
			{
				if (posDelta.x > 0 && (hummVelocity.x < speed)) {                 // Move right
					if (hummVelocity.x > 0 && !facingRight)
						flip ();
					GetComponent<Rigidbody2D> ().AddForce (new Vector2 (10,0));
					move = true;
				}
				if (posDelta.x < 0 && (hummVelocity.x > -speed)) {                // Move left
					if (hummVelocity.x < 0 && facingRight)
						flip ();
					GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-10, 0));
					move = true;
				}
			}
			
			// Target in fire range
			if ((inFireRange == true) && ((hummVelocity.x < speed) || (hummVelocity.x > -speed)))
				hummVelocity = new Vector2 (0, hummVelocity.y);     // Stop
			
			// Tagret lost
			if(targetFound == true && transform.position.x < (targetPos.x + posTolerance + 1f) && transform.position.x > (targetPos.x - posTolerance - 1f))
				targetFound = false;
			
		}
		/*
		if (exploded && !destroy) {
			//Destroy (transform.FindChild("EnemyWeapon").gameObject,0);
			//Destroy (transform.FindChild("Accessories").gameObject,0);
			//Destroy (transform.FindChild("MoveAnim").gameObject,0);
			destroy = true;
		}
*/
	}
		
	void flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
