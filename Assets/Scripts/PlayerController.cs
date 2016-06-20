using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To Do:
// > Create touch input for RayCastHit for the Selecting door thing ( 30 min )

public class PlayerController : MonoBehaviour {

    public bool isHidden;

    public HideBehaviour selectHide;
    public ActivateTextAtLine textRef;

    public Slider staminaBar;
    public Button yesButton;
    public Button noButton;
    public GameObject yesButtonG;
    public GameObject noButtonG;
    
    public bool isClickingButton;
    public GameObject[] buttons; // am I using this?
    public Collider[] collidedButtons;

    public LevelManager manageLevel;

    public CameraFollowAndEffects cameraReference; // this is the reference to set the dark screen when changing level
    public bool switchingLevel; // These two variables are to check if it's switching level, and a timer to fading in/out of dark screen
    public float switchingLevelTime; // ^

    public bool isSelectionActive; // using this to know if it's colliding with RayCastHit

    public GameObject SelectDoor;
    public GameObject QuestionMark;

    // Selecting Declaration
    public GameObject[] doors;
    public Collider[] collidedDoors;

    public GameObject[] hideObjects;
    public Collider[] collidedHideObjects;

    public bool canMove = true;
    public bool isRunning;

    // some functions & values related to background moving are commented out

    public GameObject bckgPlane;
    // public float bckgSpeed = 2.0f;
    // ^ using this to set background speed (to follow player)

    

    Vector3 addZPos = new Vector3(0, 0, 0); // Add Y position to player (to change lane)
    // laning system disabled - add 1.5f to Z value to switch back on



    Vector3 addXPos = new Vector3(.08f, 0, 0); // Add X position to player (to move < or >)

    //running value
    Vector3 addXRunPos = new Vector3(.14f, 0, 0); 

    Vector3 tempVec; // Vector being used to make background follow (slowly)

    // int position = 0; // used to label and switch lanes --> delete this after disabling up/down funcs.

    #region MoveBackground
    /*
    void MoveBackground(Vector3 addPosition)
    {
        bckgPlane.transform.position += addPosition ;
    }
    */
    #endregion

    // Go up or down function (adds Y vector to transform.position)

    #region MovePlayer

    // Go left or right function (adds X vector to transform.position)

    public void GoLeft()
    {

        if (!canMove) // This is Enabled/Disabled when a dialogue appears (maybe later also GameOver?)
        {
            return;
        }

        if (isRunning) // reference for this in TouchInput
        {
            transform.position -= addXRunPos;
        }
        else
        {
            transform.position -= addXPos;
        }
    }

    public void GoRight()
    {

        if (!canMove) // This is Enabled/Disabled when a dialogue appears (maybe later also GameOver?)
        {
            return;
        }


        if (isRunning) // reference for this in TouchInput
        {
            transform.position += addXRunPos;
        }
        else
        {
            transform.position += addXPos;
        }
    }
    #endregion

    void Awake () {

        Application.targetFrameRate = 30;

        staminaBar.value = 100.0f;

        // tempVec = new Vector3 (bckgSpeed*Time.deltaTime,0,0);
        // ^ have to work this out so background follows

        QuestionMark.SetActive(false); // Activates once the player reaches a clickable object 
        
        doors = GameObject.FindGameObjectsWithTag("Door");
        hideObjects = GameObject.FindGameObjectsWithTag("HideObject");
        
        int b = 0;

        foreach (GameObject doorNum in doors)
        {
            collidedDoors[b] = doorNum.GetComponent<Collider>();
            b++;
        }

        b = 0;

        yesButtonG.SetActive(true);
        noButtonG.SetActive(true);

        buttons = GameObject.FindGameObjectsWithTag("Button");

        if (textRef.showYesNoButtons == false)
        {
            yesButtonG.SetActive(false);
            noButtonG.SetActive(false);
        }

        b = 0;

        // collidedDoors = .FindGameObjectsWithTag("Door");
        // SelectDoor.GetComponent<Collider>(); // ^ this collider is for RayCastHit into the Door

        foreach(GameObject hideObjectNum in hideObjects)
        {
            collidedHideObjects[b] = hideObjectNum.GetComponent<Collider>();
            b++;
        }

        b = 0;
    }

    void Update()
    {
        //if (isHidden) cameraReference.opacity = 0.5f;
        //else cameraReference.opacity = 0.0f;

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
            canMove = true;

            manageLevel.ChangeLevel();

            //resetting these variables
            switchingLevel = false;
            switchingLevelTime = 0.0f;
        }

        #endregion
        
        if (Input.GetMouseButtonDown(0))
        {
            // this will make a mouse/touch input RayCast to select and use the Door
            // _collided is a collider reference for the Door Object

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            foreach (Collider col in collidedDoors)
            {
                if (col.Raycast(ray, out hit, 100.0f) && (isSelectionActive))
                {
                    switchingLevel = true;
                }
            }

            foreach (Collider col in collidedHideObjects)
            {
                if (col.Raycast(ray, out hit, 100.0f) && (isSelectionActive))
                {
                    Vector3 tempVec = new Vector3(0, 0, 5);
                    if (isHidden)
                    {
                        transform.position -= tempVec;
                        isHidden = false;
                        canMove = true;
                    }
                    else
                    {
                        transform.position += tempVec;
                        isHidden = true;
                        canMove = false;
                    }
                    Debug.Log("working");
                }
            }
        }
        


        // tempVec = new Vector3(bckgSpeed * Time.deltaTime, 0, 0);

        #region UserInput

        // Comment these soon - they're for debugging with A/D keys (side movement)

        if (Input.GetKey(KeyCode.A))
        {
            GoLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            GoRight();
        }

        #endregion

    }
}
