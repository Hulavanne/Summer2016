using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    public GameObject player;
    public GameObject mainCamera;
    public float backgroundSpeed = 0.8f;
    public PlayerController playerControl;
    
    void Start ()
    {
        // jumps to player
        transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, transform.position.z);
    }
	
	void Update ()
    {
        transform.position = new Vector3(mainCamera.transform.position.x * backgroundSpeed, transform.position.y, transform.position.z);
    }
}
