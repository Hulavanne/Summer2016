using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    // this script makes the camera follow the player on the X axis

    public GameObject player;

	void Update () {
        transform.position = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
    }
}
