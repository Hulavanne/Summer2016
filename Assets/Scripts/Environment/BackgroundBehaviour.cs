using UnityEngine;
using System.Collections;

public class BackgroundBehaviour : MonoBehaviour {

    public GameObject player;
    public GameObject mainCamera;
    public float backgroundSpeed = 0.8f;
    public PlayerController playerControl;

    public void JumpToPlayer()
    {
            transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, transform.position.z);        
    }
 
    void Start ()
    {
        JumpToPlayer();
	}
	
	void Update ()
    {
            transform.position = new Vector3(mainCamera.transform.position.x * backgroundSpeed, transform.position.y, transform.position.z);
    }
}
