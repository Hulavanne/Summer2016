using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// To Do :
// > either merge this script with Pyry's or re-organize it other way
// > remember to cap fps so time.deltaTime remains constant (40 min?)
// > set other time-related options related to time.deltaTime (25 min)

public class TouchInput_Diogo : MonoBehaviour
{
    public Slider staminaBar;

    public PlayerController Player;
	Vector3 addXPos = new Vector3(.1f, 0, 0);

    public int runValue = 0;
    public float runTouchDelay = 0;
    float runTouchDelayMax = 2;

    bool isTouching;
    /*
    To run, the player must double tap and hold within the second tap.  
    So, we have a runValue that can have of value 0, 1 and 2.
    0 for no touch, 1 when the player touches once, and 2 for when the player touches twice.
    
    Releasing the touch at value 1, the program waits for a while for the second touch before turning back to 0.
    The player starts running once the value reaches 2.
    (Check GetMouseButtonDown(0) and GetMouseButtonUp(0) and TouchPhase events.)
    */

    public float minSwipeDistY; // minimum swipe distance to prevent accidental touch input

    private Vector2 startPos; // create a starting position Vector2 when the player clicks / touches

	void Awake ()
	{
		
	}

	void Update ()
	{
        if ((staminaBar.value < 100) && (runValue != 2))
        {
            staminaBar.value += Time.deltaTime * 0.02f;
        }

        if (runValue == 2)
        {
            staminaBar.value -= Time.deltaTime * 0.33f;
        }

        // Debug.Log(staminaBar.value);

        if ((staminaBar.value <= .01f) && (Input.GetMouseButton(0)))
        {
            staminaBar.value = 0.0f;
        }

        if (staminaBar.value <= 0)
        {
            runValue = 0;
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
            Player.isRunning = true;
        }
        else
        {
            Player.isRunning = false;
        }

// For unity editor

#if UNITY_EDITOR

        //Running Input
        //Decreasing the value once the touch is pressed

        if (Input.GetMouseButton(0))
        {

            // Go Left and Right Input
            
                if (Player.switchingLevel)
                {
                    Player.canMove = false;
                }
            
            if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width * 0.5)
            {
                Player.GoLeft();
            }
            else
            {
                Player.GoRight();
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;

            

            //Running Input

            if (runValue == 0)
            {
                runValue++;
            }
            else if (runValue == 1)
            {
                if (runTouchDelay > 0)
                {
                    // this will check runValue value, and add 1 if its value is either 0, or 1
                    // it will only add 1 to runValue = 1 (making it 2) if the runTouchDelay hasn't elapsed
                    runValue++;
                }
            }

            runTouchDelay = runTouchDelayMax;
            // this sets the delay to its max value
        }


        if (Input.GetMouseButtonUp(0))
        {
            // Running Input

            if(runValue == 2)
            {
                // if at any point the player releases the touch while the value is 2, it resets to 0
                runValue = 0;
            }


            float swipeDistVertical = (new Vector3(0, Input.mousePosition.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
            // gets a single value to see if a minimum swipe distance is bigger than the vector created
            // ^ this way little swipes won't be considered by the program

            if (swipeDistVertical > minSwipeDistY)
            {
                float swipeValue = Mathf.Sign(Input.mousePosition.y - startPos.y);
                // creates a value by subtracting both positions
            }

        }


        // For touch device

        // Still have to implement the running system for touch (takes about 20~30 minutes)

#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
		// If user is touching the screen
		if (Input.touchCount > 0)
		{
            Touch touch = Input.touches[0];
            // only allows one touch input at a time    

            switch (touch.phase)
            // switching basic touchBegin and End functions    

            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                    // Insert Running Input Here


                case TouchPhase.Ended:
                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;
                    // gets a single value to see if a minimum swipe distance is bigger than the vector created
                    // ^ this way little swipes won't be considered by the program
                

                    // Insert Running Input Here

                    if(runValue == 2)
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

                    }

                    if (swipeDistVertical > minSwipeDistY)
                    {
                        float swipeValue = Mathf.Sign (touch.position.y - startPos.y);
                        // creates a value by subtracting both positions

                        if (swipeValue > 0) // up swipe
                        {
                            Player.GoUp();
                        }
                        else if (swipeValue < 0) // down swipe
                        {
                            Player.GoDown();
                        }

                    break;
                    }
        
            }

			// Check first touch (prevent issues with multi touch)
			Touch touch = Input.touches[0];
			int pointerID = touch.fingerId;

			// If user is not touching a button (eg. pause button)
			if (!EventSystem.current.IsPointerOverGameObject(pointerID))
			{
				// If user is touching left of screen
				if (touch.position.x >= 0 && touch.position.x <= Screen.width * 0.5)
				{
					GoLeft();
				}
				// If user is touching right of screen
				else
				{
					GoRight();
				}
			}
		}
#endif

        if (Input.GetKey("a"))
		{
			GoLeft();
		}
		if (Input.GetKey("d"))
		{
			GoRight();
		}
	}

	void GoLeft()
	{
		transform.position -= addXPos;
	}

	void GoRight()
	{
		transform.position += addXPos;
	}
}
