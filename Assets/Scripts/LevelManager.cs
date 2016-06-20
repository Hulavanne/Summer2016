using UnityEngine;
using System.Collections;

// Will use this LevelManager as alternative to keeping all the changing level stuck into PlayerController and stuff

public class LevelManager : MonoBehaviour {

    public GameObject Player;

    public int currentLevel = 1; // begins at level 1
    public bool goNextLevel = true; // goes up one level if true, goes down one level if false

    public void ChangePlayerPosition()
    {

        Vector3 tempVec = new Vector3(0, 20, 0); // temporary Vector3

        if (goNextLevel)
        {
            Player.transform.position -= tempVec;  // going down means increasing a level
        }
        else
        {
            Player.transform.position += tempVec; // therefore going up means decreasing a level
        }
    }

    public void ChangeLevel()
    {
        if (currentLevel == 1)
        {
            if (goNextLevel)
            {
                currentLevel++;
                ChangePlayerPosition();
            }
            else
            {
                currentLevel--;
                ChangePlayerPosition();
            }
        }

        else if (currentLevel == 2)
        {
            if (goNextLevel)
            {
                currentLevel++;
                ChangePlayerPosition();
            }
            else
            {
                currentLevel--;
                ChangePlayerPosition();
            }
        }
    }

    void Awake () {
	
	}
	
	void Update () {
	
	}
}
