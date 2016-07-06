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

    void Start ()
    {
        mainCamera = GameObject.Find("MainCamera");
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        Debug.Log("Working0");
        if (playerControl.levelManager.currentLevel == thisLevelValue)
        {
            Debug.Log("Working1");

            if (playerStart)
            {
                thisLevelPos.transform.position = new Vector3 (thisLevelPos.transform.position.x, thisLevelPos.transform.position.y, thisLevelPos.transform.position.z);
                playerStart = false;
            }

            transform.position = new Vector3(thisLevelPos.transform.position.x + 
            (player.transform.position.x - thisLevelPos.transform.position.x) * backgroundSpeed,
            transform.position.y, transform.position.z);
        }
    }
}
