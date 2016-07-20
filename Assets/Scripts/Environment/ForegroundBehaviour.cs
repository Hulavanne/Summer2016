using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour
{
    public float speed = 1;
    public PlayerController playerControl;
    public LevelManager.Levels thisLevelValue;
    public GameObject mainCamera;
    public float distance;
    public bool startLevel = true;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public float distanceFromCentre;

    public Transform boundary1;
    public Transform boundary2;

    public float boundaryAverage;

    void Start()
    {
        sprite1 = transform.FindChild("sprite1").gameObject.GetComponent<SpriteRenderer>();
        sprite2 = transform.FindChild("sprite2").gameObject.GetComponent<SpriteRenderer>();
        boundary1 = transform.parent.gameObject.transform.FindChild("boundary1").gameObject.transform;
        boundary2 = transform.parent.gameObject.transform.FindChild("boundary2").gameObject.transform;
        mainCamera = GameObject.Find("MainCamera");
        playerControl = GameObject.Find("Player").GetComponent<PlayerController>();
        boundaryAverage = (boundary2.transform.position.x - boundary1.transform.position.x) / 2;

    }

    void Update()
    {
        if (playerControl.levelManager.currentLevel == thisLevelValue)
        {
            if (startLevel) // enables sprites and positions only in the level beggining
            {
                sprite1.enabled = true;
                sprite2.enabled = true;
                startLevel = false;

                // these get the distance of the player to the foreground, and the position of the camera
                transform.position = new Vector3(mainCamera.transform.position.x * (-1), transform.position.y, transform.position.z);
                distance = mainCamera.transform.position.x - transform.position.x;
                transform.position = new Vector3(distance, transform.position.y, transform.position.z);
            }
            // these chunks will teleport the sprite right after it reaches out of the level
            // boundaryAverage --> half of the level size, considering the boundaries' position
            if (sprite1.transform.position.x > (boundary2.position.x + boundaryAverage))
            {
                sprite1.transform.position -= new Vector3(4 * boundaryAverage, 0, 0);
            }
            if (sprite2.transform.position.x > (boundary2.position.x + boundaryAverage))
            {
                sprite2.transform.position -= new Vector3(4 * boundaryAverage, 0, 0);
            }
            if (sprite1.transform.position.x < (boundary1.position.x - boundaryAverage))
            {
                sprite1.transform.position += new Vector3(4 * boundaryAverage, 0, 0);
            }
            if (sprite2.transform.position.x < (boundary1.position.x - boundaryAverage))
            {
                sprite2.transform.position += new Vector3(4 * boundaryAverage, 0, 0);
            }
            // will move the sprites according to the inverse movement of the camera position
            transform.position = new Vector3((distance - mainCamera.transform.position.x), transform.position.y, transform.position.z);    
        }
        else
        { // disables sprites when out of the level
            startLevel = true;
            sprite1.enabled = false;
            sprite2.enabled = false;
        }
    }
}