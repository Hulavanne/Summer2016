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

    public bool isIntro = true;

    public float npcWaitTime = 0.0f;
    public bool canTalkToNPC;
    public string NPCName;

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

    GameObject rightBoundary;
    GameObject leftBoundary;

	public int movingDirection = 1;
    public bool canWalkRight = true;
    public bool canWalkLeft = true;
    //public bool canCameraFollow = true;

    public bool isRoomFixed;
    public bool isHidden;

    public HideBehaviour selectHide;
    public TextBoxManager textRef;

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
 
    Vector3 addXPos = new Vector3(2f, 0, 0); // Add X position to player (to move < or >)
    Vector3 addXRunPos = new Vector3(4f, 0, 0); // same for running
    Vector3 tempVec; // Vector being used to hide/unhide
    
    #endregion

	void Awake ()
	{
		if (Game.current == null)
		{
			Game game = new Game ();
			Game.current = game;
			Game.currentIndex = -1;
		}

		GameObject inGameUI = GameObject.Find("InGameUI").gameObject;
		GameObject gui = inGameUI.transform.FindChild("GUI").gameObject;

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
        
        if (Game.current != null)
		{
			if (Game.current.playerStartPositionX != 0.0f)
			{
				Vector2 startingPosition = transform.position;
				startingPosition = new Vector2(Game.current.playerStartPositionX, Game.current.playerStartPositionY);
				transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);
			}
		}

        canMove = true;
        canShowGameOverButtons = true;

		staminaBar.value = 100.0f;
		questionMark.SetActive(false); // Activates once the player reaches a clickable object
        
	}

    public void TalkToNPC()
    {
        talkToNPC = true;
        questionMark.SetActive(false);
        isSelectionActive = false;
        selection = Selection.DEFAULT;
        hasClickedActionButton = false;
    }

	void Update()
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
			cameraScript.FadeToBlack();// turnBlack = true;
			//cameraScript.opacity = 1.0f;
			return;
		}

        #region darkScreen

        if (switchingLevel) // if true, turns the screen dark
        {
            cameraScript.fadeToBlack = true; // this reference (in CameraFollowAndEffects) will turn the screen dark
            switchingLevelTime += 0.8f * Time.deltaTime; // change the 0.8f value to another thing if you want the darkscreen to go faster/slower
        }
        else
        {
            if (!isIntro)
            {
                cameraScript.fadeToBlack = false; // once this is false, CameraFollowAndEffects script will gradually re-turn the screen visible
            }
        }
		if (switchingLevelTime >= 1) // note that this 1 is a timer
		{
			//cameraScript.JoinPlayer();
			canMove = true;
			levelManager.ChangeLevel();

			//resetting these variables
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

        if (other.tag == "NPC")
        {
            NPCName = other.gameObject.name;
        }

        if (other.tag == "FixedBoundary")
        {
            //canCameraFollow = false;
            isRoomFixed = true;
        }

        else if (other.tag == "RightBoundary")
        {
            rightBoundary = other.gameObject;
            //canCameraFollow = false;
            if (transform.position.x > rightBoundary.transform.position.x)
            {
                canWalkRight = false;
                //canCameraFollow = false;
            }
            else if (transform.position.x <= rightBoundary.transform.position.x)
            {
                canWalkRight = true;
            }
        }
        else if (other.tag == "LeftBoundary")
        {
            leftBoundary = other.gameObject;
            //canCameraFollow = false;
            if (transform.position.x < leftBoundary.transform.position.x)
            {
                canWalkLeft = false;
                //canCameraFollow = false;
            }
            else if (transform.position.x >= leftBoundary.transform.position.x)
            {
                canWalkLeft = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        doorName = "";
        NPCName = "";

        if (other.tag == "FixedBoundary")
        {
            //canCameraFollow = true;
            isRoomFixed = false;
        }

        if (other.tag == "RightBoundary")
        {
            canWalkRight = true;
            if (!isRoomFixed)
            {
                //canCameraFollow = true;
            }
        }
        else if (other.tag == "LeftBoundary")
        {
            canWalkLeft = true;
            if (!isRoomFixed)
            {
                //canCameraFollow = true;
            }
        }
    }

    public void OnActionButtonClick()
    {
        Debug.Log("Player has clicked Action Button.");
        hasClickedActionButton = true;
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

    public void GoLeft()
    {
        if(textRef.isCursorOnActionButton)
        {
            return;
        }

        if (canMove)
		{
			movingDirection = -1;
            playerAnim.SetBool("isFacingRight", false);
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }

        if ((!canMove) || (!canWalkLeft)) // This is Enabled/Disabled when a dialogue appears (maybe later also GameOver?)
        {
            return;
        }

        if (isRunning) // reference for this in TouchInput
        {
            playerAnim.SetBool("isRunning", true);
            //playeranim is running left
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
        if (textRef.isCursorOnActionButton)
        {
            return;
        }

        if (canMove)
        {
			movingDirection = 1;
            playerAnim.SetBool("isFacingRight", true);
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }

        if ((!canMove)||(!canWalkRight)) // This is Enabled/Disabled when a dialogue appears (maybe later also GameOver?)
        {
            return;
        }

        if (isRunning) // reference for this in TouchInput
        {
            playerAnim.SetBool("isRunning", true);
            //playeranim is running right
            transform.position += addXRunPos * Time.deltaTime;
        }
        else
        {
            //playeranim is walking right
            transform.position += addXPos * Time.deltaTime;
        }
    }

    public void PlayerHide()
    {
        if (!isHidden)
        {
            playerAnim.SetBool("isHidden", true);
            transform.position += tempVec;
            isHidden = true;
            canMove = false;
        }
    }

    public void PlayerUnhide ()
    {
        if (isHidden)
        {
            playerAnim.SetBool("isHidden", false);
            transform.position -= tempVec;
            isHidden = false;
            canMove = true;
        }
    }

}
