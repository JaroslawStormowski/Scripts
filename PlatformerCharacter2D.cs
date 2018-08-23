using UnityEngine;
using System.Collections;
//namespace UnitySampleAssets._2D
//{

    public class PlatformerCharacter2D : MonoBehaviour
    {
	 [SerializeField] public bool facingRight = true; // For determining which way the player is currently facing.
	 
     [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
     [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	
	 
     [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                   // Amount of maxSpeed applied to crouching movement. 1 = 100%
	 
     [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
     [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
	 [SerializeField] private LayerMask whatIsPlatform; // A mask determining what is platform to the character
	 public int numOfPlatformLay;
	 public int numOfGroundColliderLay;
	 public GameObject player;

     private Transform groundCheck; // A position marking where to check if the player is grounded.
     private float groundedRadius = .5f; // Radius of the overlap circle to determine if grounded
     public bool grounded = false; // Whether or not the player is grounded.
	 public bool onPlatform = false; // Whether or not the player is on the platform.
	 public bool standsOn = false; // Whether or not the player is grounded.
     private Transform ceilingCheck; // A position marking where to check for ceilings
     private float ceilingRadius = .1f; // Radius of the overlap circle to determine if the player can stand up

	 private Animator anim; // Reference to the player's animator component.
	 
	 Sprite[] mechUnit;
	 Sprite arm2jump;
	 Sprite arm2walk;
     //Transform playerGraphics;

     private void Awake()
     {
         // Setting up references.
        groundCheck = transform.FindChild("GroundCheck");
        ceilingCheck = transform.FindChild("CeilingCheck");
        anim = GetComponent<Animator>();


		mechUnit = Resources.LoadAll<Sprite> ("Sprites/MechUnit_500x500");
		arm2walk = mechUnit [9];
		arm2jump = mechUnit [10];
//         playerGraphics = transform.FindChild("Graphics");
 //        if (playerGraphics == null)
  //           Debug.LogError("no graphics - no objects child player");
		//Physics2D.IgnoreCollision (playerCollider2, platformCollider, true);
     }

     private void FixedUpdate()
     {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		onPlatform = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsPlatform);

		if (grounded || onPlatform) {
			standsOn = true;
		} else if(!grounded && !onPlatform) {
			standsOn = false;
		}
		anim.SetBool("Ground", standsOn);
        // Set the vertical animation
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

		if (standsOn)
			transform.FindChild ("Arm2").GetComponent<SpriteRenderer> ().sprite = arm2walk;
		else
			transform.FindChild ("Arm2").GetComponent<SpriteRenderer> ().sprite = arm2jump;
     }


	public void Move(float move, bool crouch, bool jump, bool jumpDown)
     {
         // If crouching, check to see if the character can stand up
         if (!crouch && anim.GetBool("Crouch"))
         {
             // If the character has a ceiling preventing them from standing up, keep them crouching
             if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                 crouch = true;
         }

         // Set whether or not the character is crouching in the animator
         anim.SetBool("Crouch", crouch);

         //only control the player if grounded or airControl is turned on
			if (grounded || onPlatform || airControl)
         {
             // Reduce the speed if crouching by the crouchSpeed multiplier
             move = (crouch ? move*crouchSpeed : move);

             // The Speed animator parameter is set to the absolute value of the horizontal input.
             anim.SetFloat("Speed", Mathf.Abs(move));

             // Move the character
             GetComponent<Rigidbody2D>().velocity = new Vector2(move*maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

             // If the input is moving the player right and the player is facing left...
             if (move > 0 && !facingRight)
                 // ... flip the player.
                 Flip();
                 // Otherwise if the input is moving the player left and the player is facing right...
             else if (move < 0 && facingRight)
                 // ... flip the player.
                 Flip();
         }
         // If the player should jump up
			if ((grounded || onPlatform) && jump)
         {
             // Add a vertical force to the player.
             grounded = false;
             anim.SetBool("Ground", false);
             GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
         }
		// If the player should jump down
			if (onPlatform && jumpDown) {
			
			//player.GetComponent<CircleCollider2D>().enabled = false;
			transform.FindChild ("Legs/GroundCollider").GetComponent<CircleCollider2D> ().enabled = false;

			} else if (!onPlatform) {
			
			StartCoroutine(TurnCollisionBack (0.2f));
		}
     }
		
     void Flip()
     {
         // Switch the way the player is labelled as facing.
         facingRight = !facingRight;

         // Multiply the player's x local scale by -1.
         Vector3 theScale = transform.localScale;
         theScale.x *= -1;
         transform.localScale = theScale;
     }

	IEnumerator TurnCollisionBack (float time)
	{
		yield return new WaitForSeconds (time);
		transform.FindChild ("Legs/GroundCollider").GetComponent<CircleCollider2D> ().enabled = true;
	}
    }
//}