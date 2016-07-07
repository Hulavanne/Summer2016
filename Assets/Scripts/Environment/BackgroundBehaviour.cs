using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    public GameObject thisLevelPos;
    public GameObject player;
    public PlayerController playerControl;
    public GameObject mainCamera;
    public float backgroundSpeed;
    public LevelManager.Levels thisLevelValue;
    public float playerInitPos;
    public bool playerStart = true;
    public float distanceFromStart;
    public bool movesAlone;
    public float moveAloneSpeed;

    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera");
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerControl.levelManager.currentLevel == thisLevelValue)
        {
            if (movesAlone)
            {
                moveAloneSpeed += 0.0005f;
                transform.position = new Vector3(transform.position.x + moveAloneSpeed,
                    transform.position.y, transform.position.z);
            }

            else
            {
                if (playerStart)
                {
                    thisLevelPos.transform.position = new Vector3(mainCamera.transform.position.x, thisLevelPos.transform.position.y, thisLevelPos.transform.position.z);
                    playerStart = false;
                }
                transform.position = new Vector3(mainCamera.transform.position.x * backgroundSpeed,
                transform.position.y, transform.position.z);
            }
        }
    }
}