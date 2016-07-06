using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInput_Diogo : MonoBehaviour
{
	public PlayerController playerController;
	Vector3 addXPos = new Vector3(.1f, 0, 0);

	public Animator playerAnim;
    public Slider staminaBar;

    public int runValue = 0;
    public float runTouchDelay = 0;
    float runTouchDelayMax = 2;
    public bool isTouchingRight;
    bool isTouching;

    /*
    To run, the player must double tap and hold within the second tap.  
    So, we have a runValue that can have of value 0, 1 and 2.
    0 for no touch, 1 when the player touches once, and 2 for when the player touches twice.
    
    Releasing the touch at value 1, the program waits for a while for the second touch before turning back to 0.
    The player starts running once the value reaches 2.
    (Check GetMouseButtonDown(0) and GetMouseButtonUp(0) and TouchPhase events.)
    */

	void Awake()
	{
		playerController = transform.GetComponent<PlayerController>();
		playerAnim = transform.GetComponentInChildren<Animator>();
		staminaBar = GameObject.Find("InGameUI").transform.FindChild("GUI").FindChild("StaminaBar").GetComponent<Slider>();
	}

    void Update()
    {
        if ((staminaBar.value < 100) && (runValue != 2))
        {
            staminaBar.value += Time.deltaTime * 0.05f;
        }

        if (runValue == 2)
        {
            staminaBar.value -= Time.deltaTime * 0.18f;
        }

        if ((staminaBar.value <= .01f) && (Input.GetMouseButton(0)))
        {
            runValue = 0;
            playerAnim.SetBool("isIdle", true);
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isRunning", false);
        }

        if ((staminaBar.value <= 0.01f) && runValue > 0)
        {
            runValue = 0;
            playerAnim.SetBool("isIdle", true);
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isRunning", false);
        }

        // keeps checking if player is touching the first time
        if (runValue == 1 && (Input.GetMouseButton(0)))
        {
            runTouchDelay = 2; // this will always set the timer to 2
        }

        // checks if the player hasn't touched for a while after the first touched
        else if (runValue == 1 && runTouchDelay < 0)
        {
            runValue = 0; // the value will then reset to 0
        }

        // subtracts the timer every frame
        if (runTouchDelay > 0)
        {
            runTouchDelay -= Time.deltaTime * 15;
        }

        // checks every frame if runValue is 2 and sets isRunning
        if (runValue == 2)
        {
			playerController.isRunning = true;
        }
        else
        {
			playerController.isRunning = false;
        }

        // For unity editor

#if (UNITY_EDITOR || UNITY_STANDALONE)
        
        //Running Input
        //Decreasing the value once the touch is pressed

        if (Input.GetMouseButton(0))
        {
            // Go Left and Right Input

			if (playerController.switchingLevel)
            {
				playerController.canMove = false;
            }

            #region OtherMovementInput
            // this piece of code is an optimized version of movement that's not being used
            /*
            Camera camera = mainCamera.GetComponent<Camera>();
            Vector3 screenMousePosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.z));

            if ((screenMousePosition.x >= player.transform.position.x - 0.5) && (screenMousePosition.x <= player.transform.position.x + 0.5))
            {
                // do nothing
            }
            else if (Input.mousePosition.x >= 0 && (screenMousePosition.x <= player.transform.position.x))
            {
                player.GoLeft();
            }
            else if ((Input.mousePosition.x <= Screen.width) && (screenMousePosition.x > player.transform.position.x))
            {
                player.GoRight();
            }
            */
            #endregion

            if ((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2))
            {
                if (!playerController.textRef.isCursorOnActionButton)
                {
                    playerController.GoLeft();
                }
            }

            else if ((Input.mousePosition.x <= Screen.width) && (Input.mousePosition.x > Screen.width / 2))
            {
                if (!playerController.textRef.isCursorOnActionButton)
                {
                    playerController.GoRight();
                }
            }
        }

        if (playerController.textRef.isCursorOnActionButton || !playerController.canMove)
        {
            runValue = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {

            //Running Input

            if (runValue == 0)
            {
                runValue++;

                if ((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2))
                {
                    isTouchingRight = false;
                }
                else if ((Input.mousePosition.x <= Screen.width) && (Input.mousePosition.x > Screen.width / 2))
                {
                    isTouchingRight = true;
                }
            }
            else if (runValue == 1)
            {
                if (runTouchDelay > 0)
                {
                        // this will check runValue value, and add 1 if its value is either 0, or 1
                        // it will only add 1 to runValue = 1 (making it 2) if the runTouchDelay hasn't elapsed
                    if (isTouchingRight)
                    {
                        if (!((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2)))
                        {
                            runValue++;
                        }
                    }
                    else
                    {
                        if (((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2)))
                        {
                            runValue++;
                        }
                    }
                            
                }
            }

            runTouchDelay = runTouchDelayMax;
            // this sets the delay to its max value
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isIdle", true);
            // Running Input
            if (runValue == 2)
            {
                // if at any point the player releases the touch while the value is 2, it resets to 0
                runValue = 0;
            }
        }

// For touch device
#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
        // If user is touching the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            int pointerID = touch.fingerId;
            // only allows one touch input at a time    

            // If user is not touching a button (eg. pause button)
            if (!EventSystem.current.IsPointerOverGameObject(pointerID))
            {
                switch (touch.phase)
                // switching basic touchBegin and End functions    

                {
                    case TouchPhase.Began:

                        playerController.OnUserClick();
                        break;

                    case TouchPhase.Ended:

                        // Insert Running Input Here

                        if (runValue == 2)
                        {
                            runValue = 0;
                        }

                        if (runValue == 0)
                        {
                            runValue++;
                        }
                        else if (runValue == 1)
                        {
                            if (runTouchDelay > 0)
                            {
                                runValue++;
                            }
                        }
                        runTouchDelay = runTouchDelayMax;
                        break;
                }
                
                // If user is touching left of screen
                if (touch.position.x >= 0 && touch.position.x <= Screen.width * 0.5)
                {
					playerController.GoLeft();
                }
                // If user is touching right of screen
                else
                {
					playerController.GoRight();
                }
            }
        }
#endif
    }
}
