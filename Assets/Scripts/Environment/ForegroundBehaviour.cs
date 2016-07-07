using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour {

    public PlayerController playerControl;
    public LevelManager.Levels thisLevelValue;
    public GameObject player;
    public GameObject mainCamera;
    public float distance;
    public bool isStart = true;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    void Start()
    {
        sprite1 = transform.FindChild("sprite1").GetComponent<SpriteRenderer>();
        sprite2 = transform.FindChild("sprite2").GetComponent<SpriteRenderer>();
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
                sprite1.enabled = true;
                sprite2.enabled = true;
                transform.position = new Vector3(mainCamera.transform.position.x * (-1), transform.position.y, transform.position.z);
                distance = mainCamera.transform.position.x - transform.position.x;
                transform.position = new Vector3(distance, transform.position.y, transform.position.z);
                isStart = false;
            }
            transform.position = new Vector3((distance - mainCamera.transform.position.x),transform.position.y,transform.position.z);
        }
        else
        {
            isStart = true;
            sprite1.enabled = false;
            sprite2.enabled = false;
        }
    }
}