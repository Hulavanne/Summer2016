using UnityEngine;
using System.Collections;

public class Boundary_Behaviour : MonoBehaviour {

    public float boundaryScale;
    public float screenWidth;
    public float screenHeight;
    
	void Awake () {
        if (tag == "FixedBoundary")
        {
            return;
        }

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        boundaryScale = (1 / (screenHeight / screenWidth));
        if (boundaryScale > 1.8f) boundaryScale = 1.2f * boundaryScale;
        else if (boundaryScale > 1.0f) boundaryScale = 1.0f * boundaryScale;
        // boundaryScale = (screenHeight / screenWidth);

        transform.localScale = new Vector3(boundaryScale, transform.localScale.y, transform.localScale.z);
        transform.localScale = new Vector3(boundaryScale, transform.localScale.y, transform.localScale.z);

    }
    
}
