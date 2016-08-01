using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour
{
    /*

    public float speed = 1;
    public PlayerController playerControl;
    public LevelManager.Levels thisLevelValue;
    public GameObject mainCamera;
    public float distance;
    public bool startLevel = true;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public float distanceFromCentre;
    public float initPos;
    public float distanceToInitPos;
    public bool isOutOfStartLevel;

    public Transform boundary1;
    public Transform boundary2;

    public float boundaryAverage;

    */

    public float bckgLeft, bckgRight, bckgExtent;
    public Camera mainCamera;
    public float cameraPos, levelPos, initPos1, initPos2;
    public GameObject levelObj;
    public SpriteRenderer spr1, spr2, background;
    public LevelManager.Levels thisLevel;

    void Awake()
    {
        mainCamera = Camera.main;

        initPos1 = transform.FindChild("sprite1").gameObject.transform.position.x;
        initPos2 = transform.FindChild("sprite2").gameObject.transform.position.x;
    }

    void Update()
    {
        if (!GetLevel()) // Check current level, get current level position
        {
            Debug.LogError("Sprite was not found (GetSprites() func)");
        }
        else
        {
            if (!GetSprites()) // Get sprite 1 and 2
            {
                Debug.LogError("Sprite was not found (GetSprites() func)");
            }
            else
            {
                GetCameraComponents(); // Get Camara Position and Size
                CheckLevel(); // Disable Sprites when out of level
            }
        }
    }

    void LateUpdate()
    {
        CheckSpritePosition();
        UpdateSpritePosition();
    }

    void UpdateSpritePosition()
    {
        spr1.transform.position = new Vector3((initPos1 + (levelPos - cameraPos)), 0, 0);
        //spr2.transform.position = new Vector3((levelObj.transform.position.x + Screen.width - cameraPos), 0, 0);
    }

    void CheckSpritePosition()
    {
        if (spr1.transform.position.x > (bckgRight + bckgExtent / 2))
        {
            spr1.transform.position -= new Vector3(bckgExtent * 2, 0, 0);
        }
        if (spr2.transform.position.x > (bckgRight + bckgExtent / 2))
        {
            spr2.transform.position -= new Vector3(bckgExtent * 2, 0, 0);
        }
        if (spr1.transform.position.x < (bckgRight + bckgExtent / 2))
        {
            spr1.transform.position += new Vector3(bckgExtent * 2, 0, 0);
        }
        if (spr2.transform.position.x < (bckgRight + bckgExtent / 2))
        {
            spr2.transform.position += new Vector3(bckgExtent * 2, 0, 0);
        }
    }

    bool GetLevel()
    {
        bool success = false;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Level"))
        {
            if (obj.GetComponent<Level>().levelName == thisLevel)
            {
                Debug.Log(obj.GetComponent<Level>().levelName + " " + thisLevel);
                levelObj = obj;
                success = true;
            }
        }
        return success;
    }

    bool GetSprites()
    {
        bool success = false;
        spr1 = transform.FindChild("sprite1").gameObject.GetComponent<SpriteRenderer>();
        spr2 = transform.FindChild("sprite2").gameObject.GetComponent<SpriteRenderer>();

        if (spr1 != null && spr2 != null)
        {
            success = true;
        }

        return success;
    }

    void GetCameraComponents()
    {
        levelPos = levelObj.transform.position.x;
        cameraPos = mainCamera.transform.position.x;
        background = transform.parent.gameObject.transform.parent.transform.FindChild("Background").gameObject.
            transform.FindChild("BackStatic").gameObject.GetComponent<SpriteRenderer>();
        bckgLeft = background.gameObject.transform.position.x - background.sprite.bounds.extents.x;
        bckgRight = background.gameObject.transform.position.x + background.sprite.bounds.extents.x;
        bckgExtent = background.sprite.bounds.extents.x * 2; 
    }

    void CheckLevel()
    {
        if (LevelManager.current.currentLevel == thisLevel)
        {
            spr1.enabled = true;
            spr2.enabled = true;
        }
        else
        {
            spr1.enabled = false;
            spr2.enabled = false;
        }
    }

    /*

    void Start()
    {
        isOutOfStartLevel = true;
        initPos = transform.position.x;
        sprite1 = transform.FindChild("sprite1").gameObject.GetComponent<SpriteRenderer>();
        sprite2 = transform.FindChild("sprite2").gameObject.GetComponent<SpriteRenderer>();
        boundary1 = transform.parent.gameObject.transform.FindChild("boundary1").gameObject.transform;
        boundary2 = transform.parent.gameObject.transform.FindChild("boundary2").gameObject.transform;
        mainCamera = GameObject.Find("MainCamera");
        playerControl = GameObject.Find("Player").GetComponent<PlayerController>();
        boundaryAverage = (boundary2.transform.position.x - boundary1.transform.position.x) / 2;    }

    void LateUpdate()
    {
        if (LevelManager.current.currentLevel == thisLevelValue)
        {
            if (startLevel) // enables sprites and positions only in the level beggining
            {
                distanceToInitPos = transform.position.x - initPos;

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
            transform.position = new Vector3((distance - mainCamera.transform.position.x
                - distanceToInitPos), transform.position.y, transform.position.z);
        }
        else
        { // disables sprites when out of the level
            //if (isOutOfStartLevel)
            //{
            //    transform.position = new Vector3(initPos, transform.position.y, transform.position.z);
            //}
            startLevel = true;
            sprite1.enabled = false;
            sprite2.enabled = false;
        }
    }
    */
}
