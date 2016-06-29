using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {

    // This code will activate the question box once it triggers the collider 
    // and de-activate it once it leaves it.

    public bool goesDown = true;
    public bool AutomaticDoor;

    public LevelManager manageLevel;

    public PlayerController player;
    public GameObject QuestionMark;

    void OnTriggerEnter2D (Collider2D other)
    {
        if ((AutomaticDoor) && (other.gameObject.tag == "Player"))
        {
            Debug.Log("hue?");

            player.switchingLevel = true;
            if (goesDown) manageLevel.goNextLevel = true;
            else manageLevel.goNextLevel = false;
            player.isOverlappingDoor = true;
            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isRunning", false);
            player.playerAnim.SetBool("isWalking", false);
        }

        else if (other.gameObject.tag == "Player")
        {
            if (goesDown)
            {
                manageLevel.goNextLevel = true;
            }
            else
            {
                manageLevel.goNextLevel = false;
            }

            player.isSelectionActive = true;
            QuestionMark.SetActive(true);
            player.selection = PlayerController.Selection.DOOR;
            player.isOverlappingDoor = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!AutomaticDoor)
        {
            player.isSelectionActive = true;
            QuestionMark.SetActive(true);
            player.isOverlappingDoor = true;
            player.selection = PlayerController.Selection.DOOR;
        }
    }


    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.isSelectionActive = false;
            QuestionMark.SetActive(false);
            player.isOverlappingDoor = false;
            player.selection = PlayerController.Selection.DEFAULT;
        }
    }
    
	void Awake () {

    }
	
	void Update () {
	    
	}
}
