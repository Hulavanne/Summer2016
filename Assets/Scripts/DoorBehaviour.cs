using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {

    enum EnemyStance
    {
        Patrolling,
        Suspicious,
        Chasing
    }

    // This code will activate the question box once it triggers the collider 
    // and de-activate it once it leaves it.

    public bool goesDown = true;

    public LevelManager manageLevel;

    public PlayerController Player;
    public GameObject QuestionMark;

    void OnTriggerEnter (Collider other)
    {
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

            Player.isSelectionActive = true;
            QuestionMark.SetActive(true);
        }
    }
    
    void OnTriggerExit (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.isSelectionActive = false;
            QuestionMark.SetActive(false);
        }
    }
    
	void Awake () {

    }
	
	void Update () {
	    
	}
}
