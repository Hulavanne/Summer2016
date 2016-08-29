using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour
{
    public float totalOffset;
    public GameObject thisLevelPos;
    public GameObject player;
    public PlayerController playerControl;
    public GameObject mainCamera;
    public float offset;
    public LevelManager.Levels thisLevelValue;
    public bool playerStart = true;
    public bool playerStart1 = true;
    public float distanceFromStart;
    public float distanceBetweenSprites;
    public bool movesAlone;
    public float moveAloneSpeed;
    public float speed = 0.35f;
    public SpriteRenderer sprite1, sprite2;
    public GameObject sprite1G, sprite2G;
    public float initialPos;
    public bool doesNotFollow;

    void Start()
    {
        if (transform.FindChild("sprite1") != null)
        { // get sprite 1 spriteRenderer and gameObject
            sprite1 = transform.FindChild("sprite1").GetComponent<SpriteRenderer>();
            sprite1G = transform.FindChild("sprite1").gameObject;
        }

        if (transform.FindChild("sprite2") != null)
        { // get sprite 2 spriteRenderer and gameObject
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

            if (doesNotFollow)
            {
                sprite1.transform.position += new Vector3(moveAloneSpeed * Time.deltaTime, 0, 0);
                sprite2.transform.position += new Vector3(moveAloneSpeed * Time.deltaTime, 0, 0);   

                PositionCheck();
            }

            else
            {
                if (!movesAlone)
                { // checks camera position, preset offset, distance from start, and executes a funciton out of those values
                    distanceFromStart = thisLevelPos.transform.position.x - mainCamera.transform.position.x;
                    totalOffset = mainCamera.transform.position.x + distanceFromStart;
                    transform.position = new Vector3((mainCamera.transform.position.x + (distanceFromStart * 0.5f)), transform.position.y, transform.position.z);
                }
                else
                {   // first gets a moveAlone value, and then adds it to a function that mixes those from above with it
                    distanceFromStart = thisLevelPos.transform.position.x - mainCamera.transform.position.x;
                    totalOffset = mainCamera.transform.position.x + distanceFromStart;

                    transform.position = new Vector3(((mainCamera.transform.position.x - (distanceFromStart * 0.75f)
                        + distanceFromStart) * offset) + moveAloneSpeed,
                        transform.position.y, transform.position.z);
                    moveAloneSpeed += speed * Time.deltaTime;

                    PositionCheck();
                }
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

    void PositionCheck()
    {
        if (sprite1 != null && sprite2 != null)
        {
            if (sprite1.transform.position.x > (initialPos + 2 * distanceBetweenSprites))
            { // resets position if it reaches too far out of the initial place
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
}