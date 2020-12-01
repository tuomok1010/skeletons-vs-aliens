using ECM.Controllers;
using UnityEngine;

namespace ECM.Walkthrough.CustomInput
{
    /// <summary>
    /// Example of a custom character controller.
    ///
    /// This show how to create a custom character controller extending one of the included 'Base' controller.
    /// In this example, we override the HandleInput method to add our custom input code.
    /// </summary>

    public class TwoPlayerController : BaseCharacterController
    {
        //string tag;

/*         public override void Awake()
        {
            string tag = gameObject.tag;
        } */

        protected override void HandleInput()
        {
            // Default ECM Input as used in BaseCharacterController HandleInput method.
            // Replace this with your custom input code here...

            // Toggle pause / resume.
            // By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

            if (Input.GetKeyDown(KeyCode.P))
                pause = !pause;

            // Handle user input

            //string tag = gameObject.tag;

            if (gameObject.tag=="Player1")
            {
                moveDirection = new Vector3
                {
                    x = Input.GetAxisRaw("P1Horizontal"),
                    y = 0.0f,
                    z = Input.GetAxisRaw("P1Vertical")
                };
            }
            if (gameObject.tag=="Player2")
            {
                moveDirection = new Vector3
                {
                    x = Input.GetAxisRaw("P2Horizontal"),
                    y = 0.0f,
                    z = Input.GetAxisRaw("P2Vertical")
                };
            }
            
            //jump = Input.GetButton("Jump");

            //crouch = Input.GetKey(KeyCode.C);


            //
            // For example, you can replace the above code to use the Unity NEW Input system as follows

            //var gamepad = Gamepad.current;
            //if (gamepad == null)
            //    return; // No gamepad connected.

            //moveDirection = gamepad.leftStick.ReadValue();

            //... Add gamepad button inputs here
            //etc...


            //
            // Example using Eeasy Touch to add movile input

            // Easy touch input

            //moveDirection = new Vector3
            //{
            //    x = ETCInput.GetAxis("P1Left_Horizontal"),
            //    y = 0f,
            //    z = ETCInput.GetAxis("Left_Vertical")
            //};

            //jump = ETCInput.GetButton("Jump");
        }
    }
}
