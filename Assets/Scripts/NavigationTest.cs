using UnityEngine;
using System.Collections;

public class NavigationTest : MonoBehaviour
{
	public Transform target1;
	public Transform target2;
	public Transform target3;

	private NavMeshAgent agent;

	// Use this for initialization
	void Awake ()
	{
		agent = GetComponentInChildren<NavMeshAgent>();
		agent.updateRotation = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Q))
		{
			agent.SetDestination (target1.position);
		}
		else if (Input.GetKeyDown (KeyCode.W))
		{
			agent.SetDestination (target2.position);
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
			agent.SetDestination (target3.position);
		}
	}
}
