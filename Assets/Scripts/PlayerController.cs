using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public bool canMove = true;

    // some functions & values related to background moving are commented out

    public GameObject bckgPlane;
    // public float bckgSpeed = 2.0f;
    // ^ using this to set background speed (to follow player)

    Vector3 addZPos = new Vector3(0, 0, 1.5f); // Add Y position to player (to change lane)
    Vector3 addXPos = new Vector3(.1f, 0, 0); // Add X position to player (to move < or >)

    Vector3 tempVec; // Vector being used to make background follow (slowly)

    int position = 0; // used to label and switch lanes

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
    void GoUp()
    {
        transform.position += addZPos;
    }

    void GoDown()
    {
        transform.position -= addZPos;
    }

    // Go left or right function (adds X vector to transform.position)

    void GoLeft()
    {
        transform.position -= addXPos;
    }

    void GoRight()
    {
        transform.position += addXPos;
    }
    #endregion

    void Awake () {
        // tempVec = new Vector3 (bckgSpeed*Time.deltaTime,0,0);
        // ^ have to work this out so background follows
	}

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        // tempVec = new Vector3(bckgSpeed * Time.deltaTime, 0, 0);

        #region UserInput

        if (Input.GetKeyDown(KeyCode.W) && (position < 1))
        {
            position++;
            GoUp();
        }

        if (Input.GetKeyDown(KeyCode.S) && (position > -1))
        {
            position--;
            GoDown();
        }

        if (Input.GetKey(KeyCode.A))
        {
            GoLeft();
            //MoveBackground(-tempVec);
        }

        if (Input.GetKey(KeyCode.D))
        {
            GoRight();
            //MoveBackground(tempVec);
        }

        #endregion

    }
}
