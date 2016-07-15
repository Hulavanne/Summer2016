using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public LevelManager.Levels levelName;
	public bool fixedCamera = false;
	public float levelLightAmount;

    GameObject boundaries;
    GameObject playerBoundaries;
    GameObject cameraBoundaries;

	void Awake()
	{
        // Boundaries parent object
        if (transform.FindChild("Boundaries") != null)
        {
            boundaries = transform.FindChild("Boundaries").gameObject;

            // Player boundaries
            if (boundaries.transform.FindChild("PlayerBoundary") != null)
            {
                playerBoundaries = boundaries.transform.FindChild("PlayerBoundary").gameObject;
            }

            // Camera boundaries
            if (boundaries.transform.FindChild("CameraBoundary") != null)
            {
                cameraBoundaries = boundaries.transform.FindChild("CameraBoundary").gameObject;

                // Set boundaries to the edges of the background
                if (transform.FindChild("Background").transform.FindChild("BackStatic") != null)
                {
                    Sprite background = transform.FindChild("Background").transform.FindChild("BackStatic").GetComponent<SpriteRenderer>().sprite;

                    float leftX = transform.position.x - background.bounds.extents.x;
                    float rightX = transform.position.x + background.bounds.extents.x;

                    cameraBoundaries.transform.position = new Vector3(leftX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
                    cameraBoundaries.transform.GetChild(0).position = new Vector3(rightX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
                }
            }
        }
	}
}
