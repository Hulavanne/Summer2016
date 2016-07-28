using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour
{
	public PlayerController player;
    
    public int runValue = 0;
    public float runTouchDelay = 0;
    public float runTouchDelayMax = 5.0f;
    
    public bool isPressing;
    public bool isTouchingRight;
    bool isTouching;

    /*
    To run, the player must double tap and hold within the second tap.  
    So, we have a runValue that can have of value 0, 1 and 2.
    0 for no touch, 1 when the player touches once, and 2 for when the player touches twice.
    
    Releasing the touch at value 1, the program waits for a while for the second touch before turning back to 0.
    The player starts running once the value reaches 2.
    */

	void Awake()
	{
		player = transform.GetComponent<PlayerController>();
	}
    
    void Update()
    {
        if (MenuController.gamePaused)
        {
            return;
        }

        UpdateStamina();
        RunCheck();

        if (!player.canMove)
        {
            runValue = 0;
        }

        if (player.switchingLevel)
        {
            player.canMove = false;
        }

        isPressing = false;

#if (UNITY_EDITOR || UNITY_STANDALONE)

        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            if (Input.GetMouseButton(0))
            {
                isPressing = true;

                if ((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2))
                {
                    player.GoLeft();
                }

                else if ((Input.mousePosition.x <= Screen.width) && (Input.mousePosition.x > Screen.width / 2))
                {
                    player.GoRight();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (runValue == 0)
                {
                    if ((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2))
                    {
                        isTouchingRight = false;
                        runValue++;
                    }
                    else if ((Input.mousePosition.x <= Screen.width) && (Input.mousePosition.x > Screen.width / 2))
                    {
                        isTouchingRight = true;
                        runValue++;
                    }
                }
                else if (runValue == 1)
                {
                    if (runTouchDelay > 0)
                    {
                        // this will check runValue value, and add 1 if its value is either 0, or 1
                        if (((Input.mousePosition.x >= 0) && (Input.mousePosition.x > Screen.width / 2)))
                        {
                            if (isTouchingRight)
                            {
                                runValue++;
                                runTouchDelay = 0;
                                isTouchingRight = true;
                            }
                            else
                            {
                                runValue = 1;
                                runTouchDelay = runTouchDelayMax;
                                isTouchingRight = true;
                            }
                        }
                        else
                        {
                            if (!isTouchingRight)
                            {
                                runValue++;
                                runTouchDelay = 0;
                                isTouchingRight = false;
                            }
                            else
                            {
                                runValue = 1;
                                runTouchDelay = runTouchDelayMax;
                                isTouchingRight = false;
                            }
                        }
                    }
                }

                else
                {
                    if (((Input.mousePosition.x >= 0) && (Input.mousePosition.x < Screen.width / 2)))
                    {
                        runValue++;
                        runTouchDelay = 0;
                        isTouchingRight = false;
                    }
                    else
                    {
                        runValue = 1;
                        runTouchDelay = runTouchDelayMax;
                        isTouchingRight = true;
                    }
                }
                runTouchDelay = runTouchDelayMax;
            }
        }
        else
        {
            player.PlayerAnimStop();
        }

        if (Input.GetMouseButtonUp(0))
        {
            player.PlayerAnimStop();
            if (runValue == 2)
            {
                runTouchDelay = runTouchDelayMax;
                runValue = 1; // if at any point the player releases the touch while the value is 2, it resets to 0
            }
        }

        // For touch device
//#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)

        // If user is touching the screen
        if (Input.touchCount > 0)
        {
            // Only allows for one touch input at a time
            Touch touch = Input.touches[0];
            //Touch touch = Input.GetTouch(0);
            int pointerID = touch.fingerId;

            // If user is not touching a button (eg. pause button)
            if (EventSystem.current.IsPointerOverGameObject(pointerID))
            {
                player.PlayerAnimStop();
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        isPressing = true;

                        if ((touch.position.x >= 0) && (touch.position.x < Screen.width / 2))
                        {
                            player.GoLeft();
                        }

                        else if ((touch.position.x <= Screen.width) && (touch.position.x > Screen.width / 2))
                        {
                            player.GoRight();
                        }
                    }

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (runValue == 0)
                        {
                            if ((touch.position.x >= 0) && (touch.position.x < Screen.width / 2))
                            {
                                isTouchingRight = false;
                                runValue++;
                            }
                            else if ((touch.position.x <= Screen.width) && (touch.position.x > Screen.width / 2))
                            {
                                isTouchingRight = true;
                                runValue++;
                            }
                        }
                        else if (runValue == 1)
                        {
                            if (runTouchDelay > 0)
                            {
                                // this will check runValue value, and add 1 if its value is either 0, or 1
                                if (((Input.mousePosition.x <= Screen.width) && (Input.mousePosition.x > Screen.width / 2)))
                                {
                                    if (isTouchingRight)
                                    {
                                        runValue++;
                                        runTouchDelay = 0;
                                    }
                                    else
                                    {
                                        runTouchDelay = runTouchDelayMax;
                                    }
                                }
                                else
                                {
                                    if (!isTouchingRight)
                                    {
                                        runValue++;
                                        runTouchDelay = 0;
                                    }
                                    else
                                    {
                                        runTouchDelay = runTouchDelayMax;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (((touch.position.x >= 0) && (touch.position.x < Screen.width / 2)))
                            {
                                if (!isTouchingRight)
                                {
                                    runValue++;
                                    runTouchDelay = 0;
                                }
                            }
                            else
                            {
                                if (isTouchingRight)
                                {
                                    runValue = 1;
                                    runTouchDelay = runTouchDelayMax;
                                }
                            }
                        }
                    }
                }
                else
                {
                    player.PlayerAnimStop();
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    player.PlayerAnimStop();
                    if (runValue == 2)
                    {
                        runTouchDelay = runTouchDelayMax;
                        runValue = 1; // if at any point the player releases the touch while the value is 2, it resets to 0    
                    }
                }
            }
        }
#endif
    }

    void UpdateStamina()
    {
        if ((player.hud.staminaBar.value < 100) && (runValue != 2))
        {
            player.hud.staminaBar.value += Time.deltaTime * 0.05f;
        }

        if (runValue == 2)
        {
            player.hud.staminaBar.value -= Time.deltaTime * 0.1f;
        }

        if ((player.hud.staminaBar.value <= .01f && Input.GetMouseButton(0))
            || (player.hud.staminaBar.value <= 0.01f && runValue > 0))
        {
            runValue = 0;
            player.PlayerAnimStop();
        }
    }

    void RunCheck()
    {
        // keeps checking if player is touching the first time
        if (runValue == 1 && (isPressing))
        {
            runTouchDelay = runTouchDelayMax;
        }

        // checks if the player hasn't touched for a while after the first touched
        else if (runValue == 1 && runTouchDelay <= 0)
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
            player.isRunning = true;
        }
        else
        {
            player.isRunning = false;
        }
    }
}
