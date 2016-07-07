using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour
{

    public GameObject thisLevelPos;
    public GameObject player;
    public PlayerController playerControl;
    public GameObject mainCamera;
    public float backgroundSpeed;
    public LevelManager.Levels thisLevelValue;
    public bool playerStart = true;
    public float distanceFromStart;
    public bool movesAlone;
    public float moveAloneSpeed;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerController>();
        moveAloneSpeed += 0.05f;
    }

    void Update()
    {
        if (playerControl.levelManager.currentLevel == thisLevelValue)
        {
            if (playerStart)
            {
                transform.position = new Vector3(thisLevelPos.transform.position.x, thisLevelPos.transform.position.y, thisLevelPos.transform.position.z);
                playerStart = false;
                distanceFromStart = thisLevelPos.transform.position.x - mainCamera.transform.position.x;
            }
            if (movesAlone)
            {
                transform.position = new Vector3(((mainCamera.transform.position.x
                    + distanceFromStart) * backgroundSpeed) + moveAloneSpeed,
                    transform.position.y, transform.position.z);
                moveAloneSpeed += 0.2f * Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3((mainCamera.transform.position.x
                    + distanceFromStart) * backgroundSpeed,
                    transform.position.y, transform.position.z);
            }
        }
        else
        {
            playerStart = true;
            moveAloneSpeed = 0;
        }
    }
}