using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

//namespace UnitySampleAssets._2D
//{

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;
		private bool jumpDown;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if(!jump)
            // Read the jump input in Update so button presses aren't missed.
            jump = CrossPlatformInputManager.GetButtonDown("Jump");

			if(!jumpDown)
			// Read the jumpDown input in Update so button presses aren't missed.
			jumpDown = CrossPlatformInputManager.GetButtonDown("JumpDown");
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            character.Move(h, crouch, jump, jumpDown);
            jump = false;
			jumpDown = false;
        }
    }
//}