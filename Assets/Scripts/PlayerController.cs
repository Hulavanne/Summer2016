using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject bckgPlane;
    public float bckgSpeed = 2.0f;


    Vector3 currentPos;
    Vector3 addZPos = new Vector3(0, 0, 1.5f);
    Vector3 addXPos = new Vector3(.1f, 0, 0);

    Vector3 tempVec;

    int position = 0;

    void MoveBackground(Vector3 addPosition)
    {
        bckgPlane.transform.position += addPosition ;
    }
    
    void GoUp()
    {
        transform.position += addZPos;
    }

    void GoDown()
    {
        transform.position -= addZPos;
    }

    void GoLeft()
    {
        transform.position -= addXPos;
    }

    void GoRight()
    {
        transform.position += addXPos;
    }

    void Awake () {
        //tempVec = new Vector3 (bckgSpeed*Time.deltaTime,0,0);
        currentPos = transform.position;
	}
	
	void Update () {
        tempVec = new Vector3(bckgSpeed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown("w") && (position < 1))
        {
            position++;
            GoUp();
        }

        if (Input.GetKeyDown("s") && (position > -1))
        {
            position--;
            GoDown();
        }
      

        if (Input.GetKey("a"))
        {
            GoLeft();
            MoveBackground(-tempVec);
        }

        if (Input.GetKey("d"))
        {
            GoRight();
            MoveBackground(tempVec);
        }
    }
}
