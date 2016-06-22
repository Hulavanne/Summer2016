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
        if (AutomaticDoor)
        {
            player.switchingLevel = true;
        }

        if (other.gameObject.tag == "Player")
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

            player.isOverlappingDoor = true;
        }
    }
    
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.isSelectionActive = false;
            QuestionMark.SetActive(false);
            player.isOverlappingDoor = true;
        }
    }
    
	void Awake () {

    }
	
	void Update () {
	    
	}
}
