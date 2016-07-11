using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour
{

    public PlayerController playerControl;
    public LevelManager.Levels thisLevelValue;
    public GameObject player;
    public GameObject mainCamera;
    public float distance, distanceBetweenSprites;
    public float sprite1InitPos, sprite2InitPos;
    public bool isStart = true;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    void Start()
    {
        if (transform.FindChild("sprite1") != null)
        {
            sprite1 = transform.FindChild("sprite1").GetComponent<SpriteRenderer>();
            sprite1InitPos = sprite1.transform.position.x;
        }

        if (transform.FindChild("sprite2") != null)
        {
            sprite2 = transform.FindChild("sprite2").GetComponent<SpriteRenderer>();
            sprite2InitPos = sprite2.transform.position.x;
        }

        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("MainCamera");
        playerControl = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerControl.levelManager.currentLevel == thisLevelValue)
        {
            if (isStart)
            {
                if (sprite1 != null && sprite2 != null)
                {
                    sprite1.enabled = true;
                    sprite2.enabled = true;
                    
                    distanceBetweenSprites = sprite1InitPos - sprite2InitPos;
                    if (distanceBetweenSprites < 0)
                    {
                        distanceBetweenSprites *= -1;
                    }
                }
                transform.position = new Vector3(mainCamera.transform.position.x * (-1), transform.position.y, transform.position.z);
                distance = mainCamera.transform.position.x - transform.position.x;
                transform.position = new Vector3(distance, transform.position.y, transform.position.z);
                isStart = false;
            }
            if (sprite1 != null && sprite2 != null)
            {
                if (sprite1.transform.position.x < sprite2.transform.position.x)
                {
                    if (sprite2.transform.position.x < sprite2InitPos - 2 * distanceBetweenSprites)
                    {
                        sprite2.transform.position = new Vector3(sprite2InitPos, transform.position.y, transform.position.z);
                    }

                    if (sprite1.transform.position.x < sprite1InitPos - distanceBetweenSprites)
                    {
                        sprite1.transform.position = new Vector3(sprite2InitPos, transform.position.y, transform.position.z);
                    }
                }
                else if (sprite1.transform.position.x > sprite2.transform.position.x)
                {
                    if (sprite2.transform.position.x > sprite2InitPos)
                    {
                        sprite2.transform.position = new Vector3(sprite2InitPos - 2 * distanceBetweenSprites,
                            transform.position.y, transform.position.z);
                    }

                    if (sprite1.transform.position.x > sprite2InitPos)
                    {
                        sprite1.transform.position = new Vector3(sprite2InitPos - 2 * distanceBetweenSprites,
                            transform.position.y, transform.position.z);
                    }
                }
                transform.position = new Vector3((distance - mainCamera.transform.position.x), transform.position.y, transform.position.z);
            }
        }
        else
        {
            isStart = true;
            if (sprite1 != null && sprite2 != null)
            {
                sprite1.enabled = false;
                sprite2.enabled = false;
                sprite1.transform.position = new Vector3(sprite1InitPos,
                        sprite1.transform.position.y, sprite1.transform.position.z);
                sprite2.transform.position = new Vector3(sprite2InitPos,
                        sprite2.transform.position.y, sprite2.transform.position.z);
            }
        }
    }
}