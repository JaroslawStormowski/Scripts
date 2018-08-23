using UnityEngine;
using System.Collections;

public class HeliAI : MonoBehaviour {

	float[] data = new float[5];
	public bool facingRight = true;
	bool fliping;
	Vector3 trueScale;
	Vector3 theScale;
	float scaleX;

	public float speed;
	public float seeingRange;
	public float fireRange;
	public float damage;

	public bool targetFound;
	public bool inFireRange;
	//public bool inSeeRange;
	private Collider2D [] targetColliders = new Collider2D [1];

	public LayerMask targetLayer;
	//public Transform target;
	public Vector3 targetPos;
	private float posTolerance;
	private Vector3 posDelta;
	private Transform controlPoint;

	public float maxHigh = 100f;
	public float minHigh = 5f;
	private float highTolerance;
	Vector2 velocity;

	Vector3 targetDir;
	Vector3 dir;
	float angle;

	public bool grounded;
	private float groundedRadius = 1.0f;
	private Transform groundCheck;
	public LayerMask groundLayer;

	public bool alive;
	bool isFallingDown=false;
	bool crashed=false;



	// Use this for initialization
	void Start () {
		data= GameObject.Find("_GM").GetComponent<EnemyData>().GetEnemyData(name);
		//health = data[0];
		speed = data[1];
		seeingRange = data[2];
		fireRange = data[3];
		damage = data[4];

		/*
		speed = transform.GetComponent<EnemyStats> ().speed;
		seeingRange = transform.GetComponent<EnemyStats> ().seeingRange;
		fireRange = transform.GetComponent<EnemyStats> ().fireRange;
		damage = transform.GetComponent<EnemyStats> ().damage;
		*/

		controlPoint = transform.FindChild("ControlPoint");
		if (controlPoint == null)
			print ("Control point in Heli not found.");

		posTolerance = Random.Range (-10, 10) / 10;
		highTolerance = Random.Range (-10, 10) / 10;
		minHigh = minHigh + highTolerance;
		maxHigh = maxHigh + highTolerance;

		groundCheck = transform.FindChild("GroundCheck");

		trueScale = transform.localScale;
	}
		

	// Move vehicle
	void Update () {

		transform.GetComponent<EnemyStats> ().facingRight = facingRight;
		transform.GetComponent<EnemyStats> ().inFireRange = inFireRange;
		transform.GetComponent<EnemyStats> ().inSeeRange = targetFound;
		transform.GetComponent<EnemyStats> ().targetPos = targetPos;

		transform.FindChild ("MoveAnim").GetComponent<Animator> ().SetBool ("Crashed", crashed);

		alive = this.GetComponent<EnemyStats> ().alive;


		velocity = GetComponent<Rigidbody2D> ().velocity;
		targetColliders = Physics2D.OverlapCircleAll(controlPoint.position, seeingRange, targetLayer);
		//inSeeRange = Physics2D.OverlapCircle(controlPoint.position, seeingRange, targetLayer);
		inFireRange = Physics2D.OverlapCircle(controlPoint.position, fireRange - posTolerance, targetLayer);
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, groundLayer);

		if (alive) {
			// Target found
			//if (targetColliders[0] != null) 
			if(targetColliders != null)
			{
				//targetPos = new Vector3(0,0,0);
				//targetPos = GameObject.Find ("Robo1").transform.position;
				targetPos = targetColliders[0].attachedRigidbody.transform.position;
				posDelta = targetPos - transform.position;
				targetFound = true;
			}

			// Move toward targed
			if (targetFound && !inFireRange) {
				if (posDelta.x > 0 && (velocity.x < speed)) {                 // Move right
					if (velocity.x > 0 && !facingRight)
						StartCoroutine(flip (0.1f));
					gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (10, 0));
				}
				if (posDelta.x < 0 && (velocity.x > -speed)) {                // Move left
					if (velocity.x < 0 && facingRight)
						StartCoroutine(flip (0.1f));
					gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-10, 0));
				}
			}

			// Rotate durring movement
			if (Mathf.Abs (velocity.x) < speed / 4) {
				if (velocity.x > 0)
					targetDir = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
				else
					targetDir = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z);
			}

			if (Mathf.Abs (velocity.x) > speed / 3 && Mathf.Abs (velocity.x) < speed / 1.5f) {
				if (velocity.x > 0)
					targetDir = new Vector3 (transform.position.x + 3, transform.position.y - 1, transform.position.z);
				else
					targetDir = new Vector3 (transform.position.x + 3, transform.position.y + 1, transform.position.z);
			}

			if (Mathf.Abs (velocity.x) > speed / 1.5f) {
				if (velocity.x > 0)
					targetDir = new Vector3 (transform.position.x + 2, transform.position.y - 1, transform.position.z);
				else
					targetDir = new Vector3 (transform.position.x + 2, transform.position.y + 1, transform.position.z);
			}

			dir = targetDir - transform.position;
			angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0f, 0f, angle), Time.deltaTime * speed);


			// Target in fire range
			if ((inFireRange == true) && ((velocity.x < speed) || (velocity.x > -speed)))
				velocity = new Vector2 (0, velocity.y);     // Stop

			// Tagret lost
			if (targetFound == true && transform.position.x < (targetPos.x + posTolerance + 3f) && transform.position.x > (targetPos.x - posTolerance - 3f) && !targetFound)
				targetFound = false;

			//Flip toward target
			if (facingRight && (posDelta.x < 0))
				StartCoroutine(flip (0.1f));
			if (!facingRight && (posDelta.x > 0))
				StartCoroutine(flip (0.1f));

			//Keep heigh 
			//RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2 (transform.position.x, transform.position.y - 1), 10000, groundLayer );
			if (transform.position.y < maxHigh) {
				if (transform.position.y < minHigh || transform.position.y < (minHigh + targetPos.y) || velocity.y < -speed / 2) {
					if (Mathf.Abs (velocity.y) < speed)
						gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 13));
				}
			}
		}
		//Random moves while falling down
		if (!alive && !grounded && !crashed) {
			isFallingDown = true;
			int x = Random.Range (0, 6);
			if (x == 1 || x == 2 || x == 3) 
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-100, 100), 0));
			if (x == 4)
				StartCoroutine (flip (0.02f));
		}
		//Final Crash
		if (!alive && isFallingDown && !crashed && grounded) {
			GetComponent<EnemyDamage> ().Explosion1 ();
			crashed = true;
		}
	}


	IEnumerator flip(float time){
		if (!fliping) {
			fliping = true;
			facingRight = !facingRight;

			theScale.y = transform.localScale.y;

			if (facingRight)
				theScale.x = trueScale.x * -1;
			else
				theScale.x = trueScale.x;

			scaleX = theScale.x;

			for (float i = 1f; i >= 0.7f; i -= 0.1f) {
				theScale.x = scaleX * i;
				transform.localScale = theScale;
				yield return new WaitForSeconds(time);
			}

			for (float i = 0.7f; i <= 1.0f; i += 0.1f) {
				theScale.x = scaleX * i * -1;
				transform.localScale = theScale;
				yield return new WaitForSeconds (time);
			}

			if (facingRight)
				theScale.x = trueScale.x;
			else
				theScale.x = trueScale.x * -1;
		
			transform.localScale = theScale;
			fliping = false;
		}
	}

}
