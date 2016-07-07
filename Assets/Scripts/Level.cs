using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
	public bool fixedCamera = false;
	public float levelLightAmount;

    public GameObject playerBoundaries;
    public GameObject cameraBoundaries;

    GameObject boundaries;

	void Awake()
	{
        // Boundaries parent object
        if (transform.FindChild("Boundaries") != null)
        {
            boundaries = transform.FindChild("Boundaries").gameObject;
        }
        else
        {
            boundaries = new GameObject("Boundaries");
            boundaries.transform.parent = transform;
        }

        // Player boundaries
        if (boundaries.transform.FindChild("PlayerBoundary") != null)
        {
            playerBoundaries = boundaries.transform.FindChild("PlayerBoundary").gameObject;
        }
        else
        {
            playerBoundaries = (GameObject)Instantiate(playerBoundaries, transform.position, transform.rotation);
            playerBoundaries.transform.parent = boundaries.transform;
        }

        // Camera boundaries
        if (boundaries.transform.FindChild("CameraBoundary") != null)
        {
            cameraBoundaries = boundaries.transform.FindChild("CameraBoundary").gameObject;
        }
        else
        {
            cameraBoundaries = (GameObject)Instantiate(cameraBoundaries, transform.position, transform.rotation);
            cameraBoundaries.transform.parent = boundaries.transform;
        }

        if (transform.FindChild("Background").transform.FindChild("BackStatic") != null)
        {
            Sprite background = transform.FindChild("Background").transform.FindChild("BackStatic").GetComponent<SpriteRenderer>().sprite;

            float leftX = transform.position.x - background.bounds.extents.x;
            float rightX = transform.position.x + background.bounds.extents.x;

            cameraBoundaries.transform.position = new Vector3(leftX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
            cameraBoundaries.transform.GetChild(0).position = new Vector3(rightX, cameraBoundaries.transform.position.y, cameraBoundaries.transform.position.z);
        }
	}

	void Update()
	{
		
	}
}
