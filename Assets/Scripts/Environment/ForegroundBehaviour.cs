using UnityEngine;
using System.Collections;

public class ForegroundBehaviour : MonoBehaviour {

    public GameObject player;
    public GameObject mainCamera;

    public void JumpToPlayer()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, transform.position.z);
    }

    void Start()
    {
        JumpToPlayer();
    }

    void Update()
    {
        transform.position = new Vector3(mainCamera.transform.position.x * -5.0f, transform.position.y, transform.position.z);
    }
}
