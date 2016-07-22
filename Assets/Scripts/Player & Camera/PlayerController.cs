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
        HIDEOBJECT,
        NPC,
        DEFAULT,
    };

    public Selection selection = Selection.DEFAULT;

    public static PlayerController current;
    public HUDHandler hud;
    public TouchInput touchRun;
    public Animator playerAnim;
    
    public GameObject currentDoor;
    public GameObject overlappingNpc;
    public GameObject questionMark;

    public int movementDirection = 1;
    public float switchingLevelTime; // A timer for fading in/out of dark screen
    public bool switchingLevel; // check if is switching level

    public bool canMove = true;
    public bool canHide = true; // monsters will set this to false
    public bool canTalkToNPC;

    public bool isOverlappingHideObject;
    public bool isOverlappingNPC;
    public bool isGameOver = false;
    public bool isHidden;
    public bool isRunning;

    GameObject rightBoundary;
    GameObject leftBoundary;

    Camera cameraComponent;

    Vector3 addXPos = new Vector3(3f, 0, 0); // Add X move position to player
    Vector3 addXRunPos = new Vector3(5f, 0, 0); // same for running
    
    #endregion

	void Awake ()
	{
        CameraEffects.current.FadeToBlack(false);
        current = this;

        hud = GetComponent<HUDHandler>();
        touchRun = GetComponent<TouchInput>();
		LevelManager.current = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		playerAnim = transform.GetComponentInChildren<Animator>();
        
        cameraComponent = Camera.main;

        canMove = true;
        hud.canShowGameOverButtons = true;

		hud.staminaBar.value = 100.0f;
		hud.questionMark.SetActive(false); // Activates once the player reaches a clickable object
        
	}

	void Update()
    {
        CheckNPCWaitTime();

		if ((hud.hasClickedActionButton) && (hud.isSelectionActive))
		{
			if (selection == Selection.HIDEOBJECT)
			{
				if (isHidden)
				{
					PlayerUnhide();
				}
				else if (canHide)
				{
					PlayerHide();
				}
			}

			else if (selection == Selection.DOOR)
			{
				switchingLevel = true;
				hud.hasClickedActionButton = false;
			}
			else if (selection == Selection.NPC)
			{
                ActivateTextAtLine.current.TalkToNPC();
			}
		}
        
		if (isGameOver)
		{
			hud.GameOverSplash();
			CameraEffects.current.FadeToBlack(true);
			return;
		}

        #region darkScreen

        if (switchingLevel) // if true, turns the screen dark
        {
            CameraEffects.current.fadeToBlack = true; // this reference (in CameraFollowAndEffects) will turn the screen dark
            switchingLevelTime += 0.8f * Time.deltaTime; // change the 0.8f value to another thing if you want the darkscreen to go faster/slower
        }
        
		if (switchingLevelTime >= 1) // 1 is a timer
		{
			canMove = true;
			LevelManager.current.ChangeLevel();

			//resetting variables
			switchingLevel = false;
			switchingLevelTime = 0.0f;
		}
		#endregion
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Door")
        {
            currentDoor = other.gameObject;
        }

        else if (other.tag == "HideObject")
        {
            ActivateSelection(Selection.HIDEOBJECT);
        }

        else if (other.tag == "NPC")
        {
            overlappingNpc = other.gameObject;
            NpcBehaviour npcBehaviour = null;

            if (overlappingNpc.GetComponent<NpcBehaviour>() != null)
            {
                npcBehaviour = overlappingNpc.GetComponent<NpcBehaviour>();
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

            if (npcBehaviour != null)
            {
                if (npcBehaviour.isAutomatic && npcBehaviour.waitTimer <= 0.0f)
                {
                    //ActivateTextAtLine.current.TalkToNPC();
                    //npcBehaviour.isAutomatic = false;
                    //GameFlowManager.current.isNPCAutomatic = false;
                }
            }
        }
        
        else if (other.tag == "PlayerBoundary")
        {
            rightBoundary = other.gameObject;

            if (transform.position.x > rightBoundary.transform.position.x)
            {
				if (movementDirection == 1 && !TextBoxManager.current.isTalkingToNPC)
				{
					canMove = true;
				}
				else
				{
					canMove = false;
                    PlayerAnimStop();
                }
            }
            else if (transform.position.x < rightBoundary.transform.position.x)
            {
				if (movementDirection == -1 && !TextBoxManager.current.isTalkingToNPC)
				{
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
            NpcBehaviour npcBehaviour;

            if (overlappingNpc.GetComponent<NpcBehaviour>() != null)
            {
                npcBehaviour = overlappingNpc.GetComponent<NpcBehaviour>();

                if (npcBehaviour != null)
                {
                    if (npcBehaviour.isAutomatic && npcBehaviour.waitTimer <= 0.0f)
                    {
                        //ActivateTextAtLine.current.TalkToNPC();
                        //GameFlowManager.current.isNPCAutomatic = false;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentDoor = null;

        if (other.transform.tag == "NPC")
        {
            if (overlappingNpc.GetComponent<NpcBehaviour>() != null)
            {
                overlappingNpc.GetComponent<NpcBehaviour>().SetItemsUsability(false);
            }

            overlappingNpc = null;
            isOverlappingNPC = false;
            DeactivateSelection();

            ActivateTextAtLine.current.waitForPress = false;
        }
    }

    public void ActivateSelection(Selection currentSelection)
    {
        hud.isSelectionActive = true;
        hud.questionMark.SetActive(true);
        selection = currentSelection;
    }

    public void DeactivateSelection()
    {
        hud.isSelectionActive = false;
        hud.questionMark.SetActive(false);
        selection = Selection.DEFAULT;
    }

    public void OnActionButtonClick()
    {
        //Debug.Log("Player has clicked Action Button.");
        hud.hasClickedActionButton = true;
    }

    void CheckNPCWaitTime()
    {
        if (overlappingNpc != null)
        {
            if (overlappingNpc.GetComponent<NpcBehaviour>() != null)
            {
                NpcBehaviour npcBehaviour = overlappingNpc.GetComponent<NpcBehaviour>();

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

    public void PlayerAnimWalk(bool isFacingRight)
    {
        if (canMove)
        {
            playerAnim.SetBool("isFacingRight", isFacingRight);
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }
    }

    public void GoLeft()
    {
		movementDirection = -1;

        PlayerAnimWalk(false);

        if (!canMove)// || (!canWalkLeft)) // This is Enabled/Disabled when a dialogue appears
        {
            return;
        }

        if (isRunning) // reference for this in TouchInput
        {
            playerAnim.SetBool("isRunning", true);
            transform.position -= addXRunPos * Time.deltaTime;
        }
        else
        {
            //playeranim is walking right
            transform.position -= addXPos * Time.deltaTime;
        }
    }

    public void GoRight()
    {
		movementDirection = 1;

        PlayerAnimWalk(true);

        if (!canMove)
        {
            return;
        }

        if (isRunning)
        {
            playerAnim.SetBool("isRunning", true);
            transform.position += addXRunPos * Time.deltaTime;
        }
        else
        {
            transform.position += addXPos * Time.deltaTime;
        }
    }

    public void PlayerHide()
    {
        if (!isHidden)
        {
            playerAnim.SetBool("isHidden", true);
            isHidden = true;
            canMove = false;
        }
    }

    public void PlayerUnhide ()
    {
        if (isHidden)
        {
            playerAnim.SetBool("isHidden", false);
            isHidden = false;
            canMove = true;
        }
    }
}
