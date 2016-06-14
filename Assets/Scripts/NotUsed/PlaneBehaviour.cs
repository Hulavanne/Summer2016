using UnityEngine;
using System.Collections;

public class PlaneBehaviour : MonoBehaviour {

    Vector3 tempVec = new Vector3(-0.1f, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += tempVec;
	}
}
