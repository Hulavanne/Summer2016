using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour
{
    public float totalOffset;
    public GameObject thisLevelPos;
    public GameObject player;
    public PlayerController playerControl;
    public GameObject mainCamera;
    public float backgroundSpeed;
    public LevelManager.Levels thisLevelValue;
    public bool playerStart = true;
    public bool playerStart1 = true;
    public float distanceFromStart;
    public float distanceBetweenSprites;
    public bool movesAlone;
    public float moveAloneSpeed;
    public SpriteRenderer sprite1, sprite2;
    public GameObject sprite1G, sprite2G;
    public float initialPos;

    void Start()
    {
        if (transform.FindChild("sprite1") != null)
        {
            sprite1 = transform.FindChild("sprite1").GetComponent<SpriteRenderer>();
            sprite1G = transform.FindChild("sprite1").gameObject;
        }

        if (transform.FindChild("sprite2") != null)
        {
            sprite2 = transform.FindChild("sprite2").GetComponent<SpriteRenderer>();
            sprite2G = transform.FindChild("sprite2").gameObject;
        }

        mainCamera = GameObject.Find("MainCamera");
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerController>();
        moveAloneSpeed += 0.05f;
    }

    void LateUpdate()
    {
        if (LevelManager.current.currentLevel == thisLevelValue)
        {
            if (playerStart)
            {
                if (sprite1 != null && sprite2 != null)
                {
                    sprite1.enabled = true;
                    sprite2.enabled = true;
                    initialPos = sprite1.transform.position.x;
                    distanceBetweenSprites = sprite1G.gameObject.transform.position.x -
                        sprite2G.gameObject.transform.position.x;
                    if (distanceBetweenSprites < 0)
                    {
                        distanceBetweenSprites *= -1;
                    }
                }
                transform.position = new Vector3(thisLevelPos.transform.position.x, thisLevelPos.transform.position.y, thisLevelPos.transform.position.z);
                playerStart = false;
            }
            if (movesAlone)
            {
                transform.position = new Vector3(((mainCamera.transform.position.x
                    + distanceFromStart) * backgroundSpeed) + moveAloneSpeed,
                    transform.position.y, transform.position.z);
                moveAloneSpeed += 0.2f * Time.deltaTime;

                if (sprite1 != null && sprite2 != null)
                {
                    if (sprite1.transform.position.x > (initialPos + 2*distanceBetweenSprites))
                    {
                        sprite1.transform.position = new Vector3(initialPos,
                            transform.position.y, transform.position.z);
                    }
                    if (sprite2.transform.position.x > (initialPos + 2 * distanceBetweenSprites))
                    {
                        sprite2.transform.position = new Vector3(initialPos,
                            transform.position.y, transform.position.z);
                    }
                }
            }
            else
            {
                distanceFromStart = thisLevelPos.transform.position.x - mainCamera.transform.position.x;
                totalOffset = mainCamera.transform.position.x + distanceFromStart;
                transform.position = new Vector3((mainCamera.transform.position.x + (distanceFromStart * 0.5f)), transform.position.y, transform.position.z);
            }
        }
        else
        {
            playerStart = true;
            moveAloneSpeed = 0;
            if (sprite1 != null && sprite2 != null)
            {
                sprite1.enabled = false;
                sprite2.enabled = false;
            }
        }
    }
}