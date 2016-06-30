using UnityEngine;
using System.Collections;
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

    public string NPCName;

    public bool canShowGameOverButtons;
    public bool isOverlappingHideObject;
    public bool isOverlappingNPC;
    public bool talkToNPC;
    public Animator playerAnim;

    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    public GameObject rightBoundary;
    public GameObject leftBoundary;

    public bool canWalkRight = true;
    public bool canWalkLeft = true;
    public bool canCameraFollow = true;

    public bool canHide = true; // monsters will set this to false
    public bool isGameOver = false;
    public GameObject gameOverObj;
    public Text gameOverImg;
    public float opacity = 0.0f;

    public bool isHidden;

    public HideBehaviour selectHide;
    public TextBoxManager textRef;

    public Slider staminaBar;
    public Button yesButton;
    public Button noButton;
    public GameObject yesButtonG;
    public GameObject noButtonG;

    public bool isOverlappingDoor;
    public bool isClickingButton;

    public LevelManager manageLevel;

    public CameraFollowAndEffects cameraReference; // this is the reference to set the dark screen when changing level
    public bool switchingLevel; // These two variables are to check if it's switching level, and a timer to fading in/out of dark screen
    public float switchingLevelTime; // ^

    public bool isSelectionActive; // using this to know if it's colliding with RayCastHit
    
    public GameObject QuestionMark;

    // Selecting Declaration
    public GameObject[] doors;
    public Collider2D[] collidedDoors;

    public GameObject[] hideObjects;
    public Collider2D[] collidedHideObjects;

    public GameObject[] npcs;
    public Collider2D[] collidedNPCs;

    public bool canMove = true;
    public bool isRunning;
    
    public GameObject bckgPlane;
 
    Vector3 addXPos = new Vector3(2f, 0, 0); // Add X position to player (to move < or >)
    Vector3 addXRunPos = new Vector3(4f, 0, 0); // same for running
    Vector3 tempVec; // Vector being used to hide/unhide
    
    #endregion

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            NPCName = other.gameObject.name;
        }

        if (other.tag == "RightBoundary")
        {
            canCameraFollow = false;
            if (transform.position.x > rightBoundary.transform.position.x)
            {
                canWalkRight = false;
                canCameraFollow = false;
            }
        }
        else if (other.tag == "LeftBoundary")
        {
            canCameraFollow = false;
            if (transform.position.x < leftBoundary.transform.position.x)
            {
                canWalkLeft = false;
                canCameraFollow = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        NPCName = "";

        if (other.tag == "RightBoundary")
        {
            canWalkRight = true;
            canCameraFollow = true;
        }
        else if (other.tag == "LeftBoundary")
        {
            canWalkLeft = true;
            canCameraFollow = true;
        }
    }

    void GameOverSplash()
    {
        gameOverObj.SetActive(true);
        opacity += 0.015f;
        gameOverImg.GetComponent<CanvasRenderer>().SetAlpha(opacity);
        
        if((opacity >= 1.0f) && (canShowGameOverButtons))
        {
            canShowGameOverButtons = false;
            ReloadSaveButton.SetActive(true);
            BackToMenuButton.SetActive(true);
        }
    }

    public void GoLeft()
    {
        if (canMove)
        {
            playerAnim.SetBool("isFacingRight", false);
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }

        if (canCameraFollow)
        cameraReference.JumpToPlayer();

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
        if (canMove)
        {
            playerAnim.SetBool("isFacingRight", true);
            playerAnim.SetBool("isWalking", true);
            playerAnim.SetBool("isIdle", false);
        }

        if (canCameraFollow)
        cameraReference.JumpToPlayer();

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

    void Awake () {
        canShowGameOverButtons = true;
        ReloadSaveButton.SetActive(false);
        BackToMenuButton.SetActive(false);
        gameOverObj.SetActive(false);

        staminaBar.value = 100.0f;
        QuestionMark.SetActive(false); // Activates once the player reaches a clickable object 
        
        doors = GameObject.FindGameObjectsWithTag("Door");
        hideObjects = GameObject.FindGameObjectsWithTag("HideObject");
        npcs = GameObject.FindGameObjectsWithTag("NPC");
        

        int b = 0;

        foreach (GameObject doorNum in doors)
        {
            collidedDoors[b] = doorNum.GetComponent<Collider2D>();
            b++;
        }
        b = 0;

        foreach(GameObject npcNum in npcs)
        {
            collidedNPCs[b] = npcNum.GetComponent<Collider2D>();
            b++;
        }
        b = 0;

        yesButtonG.SetActive(true);
        noButtonG.SetActive(true);

        foreach(GameObject hideObjectNum in hideObjects)
        {
            collidedHideObjects[b] = hideObjectNum.GetComponent<Collider2D>();
            b++;
        }
        b = 0;
    }

    public void OnUserClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // this will make a mouse/touch input RayCast to select and use the Door
        RaycastHit2D hit = Physics2D.Raycast(mouseposition, Input.mousePosition);

        foreach (Collider2D col in collidedDoors)
        {
            if ((hit.collider && hit.collider.tag == "Door") && (isSelectionActive) && (selection == Selection.DOOR))
            {
                switchingLevel = true;
            }
            else if (((hit.collider && hit.collider.tag == "Player") && (isSelectionActive) && (selection == Selection.DOOR))
                && isOverlappingDoor)
            {
                switchingLevel = true;
            }
        }

        foreach (Collider2D col in collidedNPCs)
        {
            if ((hit.collider && hit.collider.tag == "NPC") && (isSelectionActive) && (selection == Selection.NPC))
            {
                talkToNPC = true;
                QuestionMark.SetActive(false);
                isSelectionActive = false;
                selection = Selection.DEFAULT;
            }
            else if (((hit.collider && hit.collider.tag == "Player") && (isSelectionActive) && (selection == Selection.NPC))
                && isOverlappingNPC)
            {
                talkToNPC = true;
                QuestionMark.SetActive(false);
                isSelectionActive = false;
                selection = Selection.DEFAULT;
            }
        }

        foreach (Collider2D col in collidedHideObjects)
        {

            if (((hit.collider && hit.collider.tag == "HideObject") && (isSelectionActive) && (selection == Selection.HIDEOBJECT))
                || ((((hit.collider && hit.collider.tag == "Player") && (isSelectionActive) && (selection == Selection.HIDEOBJECT))
                && isOverlappingHideObject)))
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
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            GameOverSplash();
            cameraReference.turnBlack = true;
            cameraReference.opacity = 1.0f;
            return;
        }
        
        #region darkScreen

        if (switchingLevel) // if true, turns the screen dark
        {
            cameraReference.turnBlack = true; // this reference (in CameraFollowAndEffects) will turn the screen dark
            switchingLevelTime += 0.8f * Time.deltaTime; // change the 0.8f value to another thing if you want the darkscreen to go faster/slower
        }
        else
        {
            cameraReference.turnBlack = false; // once this is false, CameraFollowAndEffects script will gradually re-turn the screen visible
        }
        if (switchingLevelTime >= 1) // note that this 1 is a timer
        {
            cameraReference.StartJoinPlayer();
            canMove = true;
            manageLevel.ChangeLevel();

            //resetting these variables
            switchingLevel = false;
            switchingLevelTime = 0.0f;
        }
        #endregion
    }
}
