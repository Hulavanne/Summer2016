using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    #region declarations

    public static PlayerController current;

    public enum Selection
    {
        DOOR,
        HIDEOBJECT,
        NPC,
        DEFAULT,
    };
    public Selection selection = Selection.DEFAULT;

    public bool isIntro = true;

    public float npcWaitTime = 0.0f;
    public bool canTalkToNPC;
    public GameObject overlappingNpc;

    public string doorName;

    public bool canShowGameOverButtons;
    public bool isOverlappingHideObject;
    public bool isOverlappingNPC;
    public bool talkToNPC;
    public Animator playerAnim;

    public bool hasClickedActionButton;

	public Slider staminaBar;
	public Button yesButton;
	public Button noButton;

	public bool canHide = true; // monsters will set this to false
	public bool isGameOver = false;
	public GameObject gameOverObj;
	public Text gameOverImg;
	public float opacity = 0.0f;
    public GameObject reloadSaveButton;
    public GameObject backToMenuButton;
    public TouchInput_Diogo touchRun;

    GameObject rightBoundary;
    GameObject leftBoundary;

	public int movementDirection = 1;
    //public int canWalkRight = true;
    //public bool canWalkLeft = true;
    //public bool canCameraFollow = true;

    public bool isRoomFixed;
    public bool isHidden;

    public HideBehaviour selectHide;
    public TextBoxManager textRef;

    public GameObject currentDoor;
    public GameObject nextDoor;

    public bool isOverlappingDoor;
    public bool isClickingButton;

    public LevelManager levelManager;

    Camera cameraComponent;
    CameraFollowAndEffects cameraScript; // this is the reference to set the dark screen when changing level

    public bool switchingLevel; // These two variables are to check if it's switching level, and a timer to fading in/out of dark screen
    public float switchingLevelTime; // ^

    public bool isSelectionActive; // using this to know if it's colliding with RayCastHit
    
    public GameObject questionMark;

    public bool canMove = true;
    public bool isRunning;
    
    public GameObject bckgPlane;
 
    Vector3 addXPos = new Vector3(3f, 0, 0); // Add X position to player (to move < or >)
    Vector3 addXRunPos = new Vector3(5f, 0, 0); // same for running
    Vector3 tempVec; // Vector being used to hide/unhide
    
    #endregion

	void Awake ()
	{
        current = this;

		GameObject inGameUI = GameObject.Find("InGameUI").gameObject;
		GameObject gui = inGameUI.transform.FindChild("GUI").gameObject;

        touchRun = GetComponent<TouchInput_Diogo>();
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		playerAnim = transform.GetComponentInChildren<Animator>();
		questionMark = transform.FindChild ("QuestionMark").gameObject;
		gameOverObj = gui.transform.FindChild("GameOver").gameObject;
		gameOverImg = gameOverObj.GetComponent<Text>();
		reloadSaveButton = gameOverObj.transform.FindChild("ReloadSave").gameObject;
		backToMenuButton = gameOverObj.transform.FindChild("BackToMenu").gameObject;
		textRef = inGameUI.GetComponent<TextBoxManager>();
		staminaBar = gui.transform.FindChild("StaminaBar").GetComponent<Slider>();
		yesButton = gui.transform.FindChild("TextBoxNormal").FindChild("ButtonYes").GetComponent<Button>();
		noButton = gui.transform.FindChild("TextBoxNormal").FindChild("ButtonNo").GetComponent<Button>();
		cameraComponent = Camera.main;
		cameraScript = cameraComponent.GetComponent<CameraFollowAndEffects>();

        canMove = true;
        canShowGameOverButtons = true;

		staminaBar.value = 100.0f;
		questionMark.SetActive(false); // Activates once the player reaches a clickable object
        
	}

	void Update()
    {
        CheckNPCWaitTime();

		if ((hasClickedActionButton) && (isSelectionActive))
		{
			if (selection == Selection.HIDEOBJECT)
			{
				tempVec = new Vector3(0, 0, 5);
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
				hasClickedActionButton = false;
			}
			else if (selection == Selection.NPC)
			{
                TalkToNPC();
			}
		}
        
		if (isGameOver)
		{
			GameOverSplash();
			cameraScript.FadeToBlack();
			return;
		}

        #region darkScreen

        if (switchingLevel) // if true, turns the screen dark
        {
            cameraScript.fadeToBlack = true; // this reference (in CameraFollowAndEffects) will turn the screen dark
            switchingLevelTime += 0.8f * Time.deltaTime; // change the 0.8f value to another thing if you want the darkscreen to go faster/slower
        }
        else if (!isIntro)
        {
            cameraScript.fadeToBlack = false; // once this is false, CameraFollowAndEffects script will gradually re-turn the screen visible
        }
        
		if (switchingLevelTime >= 1) // 1 is a timer
		{
			canMove = true;
			levelManager.ChangeLevel();

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
            doorName = other.gameObject.name;
        }

        else if (other.tag == "HideObject")
        {
            ActivateSelection(Selection.HIDEOBJECT);
        }

        else if (other.tag == "NPC")
        {
            overlappingNpc = other.gameObject;

            if (overlappingNpc.GetComponent<NpcBehaviour>() != null)
            {
                overlappingNpc.GetComponent<NpcBehaviour>().SetItemsUsability(true);
            }

            isOverlappingNPC = true;
            ActivateSelection(Selection.NPC);
            PlayerAnimStop();

            if (ActivateTextAtLine.current.requireButtonPress)
            {
                ActivateTextAtLine.current.waitForPress = true;
                return;
            }

            if ((GameFlowManager.current.isNPCAutomatic) && (npcWaitTime <= 0.0f))
            {
                TalkToNPC();
                GameFlowManager.current.isNPCAutomatic = false;
            }
        }

        if (other.tag == "FixedBoundary")
        {
            isRoomFixed = true;
        }
        else if (other.tag == "PlayerBoundary")
        {
            rightBoundary = other.gameObject;

            if (transform.position.x > rightBoundary.transform.position.x)
            {
				if (movementDirection == 1 && !textRef.isTalkingToNPC)
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
				if (movementDirection == -1 && !textRef.isTalkingToNPC)
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

    void OnTriggerExit2D(Collider2D other)
    {
        doorName = "";

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
        isSelectionActive = true;
        questionMark.SetActive(true);
        selection = currentSelection;
    }

    public void DeactivateSelection()
    {
        isSelectionActive = false;
        questionMark.SetActive(false);
        selection = Selection.DEFAULT;
    }

    public void OnActionButtonClick()
    {
        Debug.Log("Player has clicked Action Button.");
        hasClickedActionButton = true;
    }

    void CheckNPCWaitTime()
    {
        if (npcWaitTime > 0.0f)
        {
            canMove = false;
            npcWaitTime -= Time.deltaTime;
            canTalkToNPC = false;
        }
        else
        {
            canTalkToNPC = true;
        }
    }

    public void TalkToNPC()
    {
        talkToNPC = true;
        DeactivateSelection();
        hasClickedActionButton = false;
    }

    void GameOverSplash()
    {
        gameOverObj.SetActive(true);
        opacity += 0.015f;
        gameOverImg.GetComponent<CanvasRenderer>().SetAlpha(opacity);
        
        if((opacity >= 1.0f) && (canShowGameOverButtons))
        {
            canShowGameOverButtons = false;
            reloadSaveButton.SetActive(true);
            backToMenuButton.SetActive(true);
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
