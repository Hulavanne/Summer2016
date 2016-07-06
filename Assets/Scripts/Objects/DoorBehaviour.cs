using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {

    // This code will activate the question box once it triggers the collider 
    // and de-activate it once it leaves it.

    public LevelManager.Levels thisDoorLevel;
    public float thisDoorLight;

    public bool AutomaticDoor;

	public LevelManager levelManager;
    public PlayerController player;
    public GameObject questionMark;
    
    public GameObject nextDoor;
    public GameObject thisDoor;
    public bool isNextDoorParent;

	void Awake()
	{
        thisDoor = gameObject;

        if (gameObject.transform.FindChild("Door") != null)
        {
            nextDoor = gameObject.transform.FindChild("Door").gameObject;
            isNextDoorParent = false;
        }
        else
        {
            nextDoor = transform.parent.gameObject;
            isNextDoorParent = true;
        }

		if (GameObject.Find("LevelManager") != null)
		{
			levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		}
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			questionMark = player.transform.FindChild("QuestionMark").gameObject;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		if ((AutomaticDoor) && (other.transform.parent.tag == "Player"))
        {
            levelManager.currentDoor = thisDoor;
            levelManager.nextDoor = nextDoor;
            player.switchingLevel = true;
            player.isOverlappingDoor = true;
            player.PlayerAnimStop();
        }

		else if (other.transform.parent.tag == "Player")
        {
            levelManager.currentDoor = thisDoor;
            levelManager.nextDoor = nextDoor;
            player.ActivateSelection(PlayerController.Selection.DOOR);
            player.isOverlappingDoor = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
		if (other.transform.parent.tag == "Player")
        {
            levelManager.currentDoor = thisDoor;
            levelManager.nextDoor = nextDoor;
            player.ActivateSelection(PlayerController.Selection.DOOR);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
		if (other.transform.parent.tag == "Player")
        {
            levelManager.currentDoor = null;
            levelManager.nextDoor = null;
            player.DeactivateSelection();
            player.isOverlappingDoor = false;
        }
    }
}
