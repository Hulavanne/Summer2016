using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour
{
    public float bckgLeft, bckgRight, bckgExtent, offset1, offset2, speed;
    public bool doesMove;
    public Camera mainCamera;
    public float cameraPos, levelPos, initPos1, initPos2, initPos1Y, initPos2Y;
    public GameObject levelObj;
    public SpriteRenderer spr1, spr2, background;
    public LevelManager.Levels thisLevel;

    void Awake()
    {
        mainCamera = Camera.main;

        initPos1 = transform.FindChild("sprite1").gameObject.transform.position.x;
        initPos1Y = transform.FindChild("sprite1").gameObject.transform.position.y;
        initPos2 = transform.FindChild("sprite2").gameObject.transform.position.x;
        initPos2Y = transform.FindChild("sprite2").gameObject.transform.position.y;

        GetSprites();
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
                GetCameraComponents(); // Get Camera Position and Size
                CheckLevel(); // Disable Sprites when out of level
            }
        }
    }

    void LateUpdate()
    {
        CheckSpritePosition(); // check if sprites are too far out (and teleports it if so)
        UpdateSpritePosition(); // moves sprites according to camara and level position
    }

    void UpdateSpritePosition()
    {
        offset1 += speed * Time.deltaTime;
        offset2 += speed * Time.deltaTime;

        if (doesMove)
        {
            spr1.transform.position = new Vector3((initPos1 + (levelPos - cameraPos)) + offset1, initPos1Y, 0);
            spr2.transform.position = new Vector3((initPos2 + (levelPos - cameraPos)) + offset2, initPos2Y, 0);
        }
        else
        {
            spr1.transform.position = new Vector3((initPos1 + (levelPos - cameraPos)), initPos1Y, 0);
            spr2.transform.position = new Vector3((initPos2 + (levelPos - cameraPos)), initPos2Y, 0);
        }
    }

    void CheckSpritePosition()
    {
        if (spr1.transform.position.x >= (bckgRight + (bckgExtent / 2)))
        {
            offset1 -= (bckgExtent * 2);
        }
        else if (spr1.transform.position.x < (bckgLeft - bckgExtent / 2))
        {
            offset1 += (bckgExtent * 2);
        }

        if (spr2.transform.position.x >= (bckgRight + bckgExtent / 2))
        {
            offset2 -= (bckgExtent * 2);
        }
        else if (spr2.transform.position.x < (bckgLeft - bckgExtent / 2))
        {
            offset2 += (bckgExtent * 2);
        }
    }

    bool GetLevel()
    {
        bool success = false;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Level"))
        {
            if (obj.GetComponent<Level>().levelName == thisLevel)
            {
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
        bckgLeft = transform.parent.transform.parent.transform.FindChild("Boundaries").
            transform.FindChild("CameraBoundary").transform.position.x;
        bckgRight = transform.parent.transform.parent.transform.FindChild("Boundaries").
            transform.FindChild("CameraBoundary").GetChild(0).transform.position.x;
        bckgExtent = bckgRight - bckgLeft; 
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
}
