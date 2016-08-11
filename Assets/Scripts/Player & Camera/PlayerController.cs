using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region declarations

    public enum Selection
    {
        DOOR,
        HIDE_OBJECT,
        NPC,
        DEFAULT,
    };

    public Selection selection = Selection.DEFAULT;

    public static PlayerController current;
    public HUDHandler hud;
    public TouchInput touchRun;
    public Animator playerAnim;
    public SpriteRenderer spriteRenderer;

    public GameObject currentHideObject;
    public GameObject currentDoor;
    public GameObject overlappingNpc;

    public int movementDirection = 1;
    public float unhideTimer = -0.5f;
    public float walkingSpeed = 3.0f; // Add X move position to player
    public float runningSpeed = 5.0f; // same for running
    public float switchingLevelTime; // A timer for fading in/out of dark screen
    public bool switchingLevel; // check if is switching level

    public bool canMove = true;
    public bool canHide = true; // monsters will set this to false
    public bool canTalkToNPC;
    public bool canClickActionButton = true;

    public bool isOverlappingHideObject;
    public bool isOverlappingNPC;
    public bool isGameOver = false;
    public bool isHidden;

    [HideInInspector]
    public bool isUnhiding = false; // animation for unhiding
    public bool isFacingRight;
    public bool isRunning;

    bool isNextRight;
    bool canCameraFollow;

    GameObject boundary;
    GameObject leftBoundary;

    Camera cameraComponent;
    
    #endregion

	void Awake ()
	{
        CameraEffects.current.FadeToBlack(false, false); // begin with black screen, fades to scene
        current = this;

        hud = GetComponent<HUDHandler>(); // all hud related components
        touchRun = GetComponent<TouchInput>(); 
		LevelManager.current = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		playerAnim = transform.GetComponentInChildren<Animator>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        cameraComponent = Camera.main;

        canMove = true;
        hud.canShowGameOverButtons = true;

		hud.staminaBar.value = 100.0f;
		hud.questionMark.SetActive(false); // Activates once the player reaches a clickable object
        
	}

	void Update()
    {
        if (isGameOver)
        {
            hud.GameOverSplash();
            AudioManager.current.SwitchMusic(AudioManager.current.gameOverMusic);
            CameraEffects.current.FadeToBlack(true, true); // sets everything to black, then fades GAMEOVER, then buttons show up
            return;
        }

        PlayerFollowObject(canCameraFollow);

        if (unhideTimer >= -0.5f) // unhide is -1 by default (and this if statement won't run)
        {
            if (unhideTimer <= 0.0f) // if unhide is bigger than -0.5f, it means it's running
            {
                PlayerUnhide(true); // unhides after animation
            }
            else
            {
                unhideTimer -= Time.deltaTime;
            }
        }

        if (isFacingRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        CheckNPCWaitTime();

        #region darkScreen

        if (switchingLevel) // if true, turns the screen dark
        {
            CameraEffects.current.fadeToBlack = true; // this will fade the screen to black
            switchingLevelTime += 0.8f * Time.deltaTime; // change 0.8f to another value if to change fade speed.
        }
        
		if (switchingLevelTime >= 1) // 1 is a timer
		{
			canMove = true;
			LevelManager.current.ChangeLevel(); // changes position and enum values
            
			switchingLevel = false; //resetting variables
            switchingLevelTime = 0.0f;
		}
		#endregion
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Door")
        {
            currentDoor = other.gameObject; // get current door
        }

        else if (other.tag == "HideObject")
        {
            currentHideObject = other.gameObject; // get current hide object
        }
        
        else if (other.tag == "NPC")
        {
            overlappingNpc = other.gameObject; // get the gameobject of npc
            CharacterBehaviour npcBehaviour = null; // reset the npcbehaviour script

            if (overlappingNpc.GetComponent<CharacterBehaviour>() != null)
            {
                npcBehaviour = overlappingNpc.GetComponent<CharacterBehaviour>();
                npcBehaviour.SetItemsUsability(true);
            }

            isOverlappingNPC = true;
            ActivateSelection(Selection.NPC);
            PlayerAnimStop();

            if (ActivateTextAtLine.current.requireButtonPress)
            {
                ActivateTextAtLine.current.waitForPress = true;
                return;
            }
        }
        
        else if (other.tag == "PlayerBoundary")
        {
            boundary = other.gameObject;

            if (transform.position.x > boundary.transform.position.x)
            {
				if (movementDirection == 1 && !TextBoxManager.current.isTalkingToNPC)
				{ // checks if the player is at the right of the boundary
					canMove = true;
				}
				else
				{
					canMove = false;
                    PlayerAnimStop();
                }
            }
            else if (transform.position.x < boundary.transform.position.x)
            {
				if (movementDirection == -1 && !TextBoxManager.current.isTalkingToNPC)
                { // checks if the player is at the left of the boundary
                    canMove = true;
				}
				else
				{
					canMove = false;
                    PlayerAnimStop();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            overlappingNpc = other.gameObject;
            CharacterBehaviour npcBehaviour;

            if (overlappingNpc.GetComponent<CharacterBehaviour>() != null)
            {
                npcBehaviour = overlappingNpc.GetComponent<CharacterBehaviour>(); // gets current npcbehav
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    { // resets most of variables
        currentDoor = null;

        if (other.transform.tag == "NPC")
        {
            if (overlappingNpc.GetComponent<CharacterBehaviour>() != null)
            {
                overlappingNpc.GetComponent<CharacterBehaviour>().SetItemsUsability(false);
            }

            overlappingNpc = null;
            isOverlappingNPC = false;
            DeactivateSelection();
        }
    }

    public void ActivateSelection(Selection currentSelection)
    {
        if (!TextBoxManager.current.isTalkingToNPC)
        {
            hud.questionMark.SetActive(true);
        }

        hud.isSelectionActive = true;
        selection = currentSelection;
    }

    public void DeactivateSelection()
    {
        hud.isSelectionActive = false;
        hud.questionMark.SetActive(false);
        selection = Selection.DEFAULT;
        ActivateTextAtLine.current.waitForPress = false;
    }

    public void OnActionButtonClick()
    {
        if (canClickActionButton)
        {
            if (hud.isSelectionActive)
            {
                if (selection == Selection.HIDE_OBJECT)
                {
                    if (isHidden)
                    { // sets the timer and waits for it to reach 0 (while playing animation). After that, unhides.
                        PlayerUnhide(false);
                    }
                    else if (canHide)
                    {
                        PlayerHide(); // hides and plays animation right after
                    }
                }

                else if (selection == Selection.DOOR)
                {
                    switchingLevel = true; // switches door
                }
                else if (selection == Selection.NPC)
                {
                    ActivateTextAtLine.current.TalkToNPC();
                }
            }
        }
        else
        {
            return;
        }
    }

    void CheckNPCWaitTime()
    { // use this script if you want the player to wait for an amount of time (as sort of cut-scene)
        if (overlappingNpc != null)
        {
            if (overlappingNpc.GetComponent<CharacterBehaviour>() != null)
            {
                CharacterBehaviour npcBehaviour = overlappingNpc.GetComponent<CharacterBehaviour>();

                if (npcBehaviour.waitTimer > 0.0f)
                {
                    canMove = false;
                    npcBehaviour.waitTimer -= Time.deltaTime;
                    canTalkToNPC = false;
                }
                else
                {
                    npcBehaviour.waitTimer = 0.0f;
                    canTalkToNPC = true;
                }
            }
        }
    }
    
    public void PlayerAnimStop()
    {
        playerAnim.SetBool("isRunning", false);
        playerAnim.SetBool("isWalking", false);
        playerAnim.SetBool("isIdle", true);
    }

    public void PlayerAnimWalk(bool facingRight)
    {
        if (canMove)
        {
            isFacingRight = facingRight;
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }
    }

    public void GoLeft()
    {
        if (TextBoxManager.current.clickException)
        {
            touchRun.runValue = 0;
            return;
        }

        movementDirection = -1; // -1 stands for left

        PlayerAnimWalk(false);

        if (!canMove)
        {
            return;
        }

        if (isRunning) // reference for this in TouchInput
        {
            playerAnim.SetBool("isRunning", true);
            transform.position -= new Vector3(runningSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(walkingSpeed * Time.deltaTime, 0, 0);
        }
    }

    public void GoRight()
    {
        if (TextBoxManager.current.clickException)
        {
            touchRun.runValue = 0;
            return;
        }

        movementDirection = 1; // 1 stands for right

        PlayerAnimWalk(true);

        if (!canMove)
        {
            return;
        }

        if (isRunning)
        {
            playerAnim.SetBool("isRunning", true);
            transform.position += new Vector3(runningSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += new Vector3(walkingSpeed * Time.deltaTime, 0, 0);
        }
    }

    void PlayerFollowObject(bool canFollow)
    {
        if (currentHideObject == null)
        { // doesn't execute anything if no hideobject is found
            return;
        }

        if (!canFollow) // hideobjectisfound
        { // will keep track of object's position (left or right) until this function is executed with a "true" reference.
            if (currentHideObject.transform.position.x > transform.position.x)
            {
                isNextRight = true;
            }
            else
            {
                isNextRight = false;
            }
        }

        else
        {
            if (isNextRight) // checks if the object (hideobject) is to the player's left or right
            { // object is the player's right
                transform.position += new Vector3(4*Time.deltaTime, 0, 0); // moves
                if (transform.position.x >= currentHideObject.transform.position.x)
                {
                    canCameraFollow = false;
                }
            }
            else
            { // object is to the player's left
                transform.position -= new Vector3(4*Time.deltaTime, 0, 0); // moves
                if (transform.position.x <= currentHideObject.transform.position.x)
                {
                    canCameraFollow = false;
                }
            }
        }
    }

    public void PlayerHide()
    {
        if (!isHidden && !isUnhiding)
        {
            canCameraFollow = true; // in PlayerFollowObject()
            LightBehaviour.current.SetLighting(true, 1.0f); // fade the darkscreen with hole
            playerAnim.SetBool("isHidden", true);
            hud.questionMark.GetComponent<SpriteRenderer>().enabled = false; // disables selection game object
            isHidden = true;
            canMove = false;
            unhideTimer = -1; // deafult value
            canClickActionButton = false;
        }
    }

    public void PlayerUnhide (bool hides)
    {
        if (isHidden)
        {
            if (!hides)
            {
                LightBehaviour.current.SetLighting(false, 0.0f);
                unhideTimer = 0.5f;
                isUnhiding = true;
                canClickActionButton = false;
            }
            else
            {
                if (canHide)
                {
                    isFacingRight = false;
                    hud.questionMark.GetComponent<SpriteRenderer>().enabled = true;
                    playerAnim.SetBool("isHidden", false);
                    isHidden = false;
                    canMove = true;
                    unhideTimer = -1; // reset value
                    isUnhiding = false;
                    canClickActionButton = true;
                }
            }
        }
    }
}
