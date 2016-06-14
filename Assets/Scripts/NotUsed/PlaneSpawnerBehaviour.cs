using UnityEngine;
using System.Collections;

public class PlaneSpawnerBehaviour : MonoBehaviour {

    public GameObject planeObj;
    public float time;
    public float maxTime = 2;

	void Start () {
        time = maxTime;
	}
	
	void Update () {

        time -= 0.1f * Time.deltaTime;

        if (time <= 0)
        {
            Instantiate(planeObj);
            time = maxTime;
        }
	}
}
