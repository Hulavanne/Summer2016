﻿using UnityEngine;
using System.Collections;

public class NavigationTest : MonoBehaviour
{
	public Vector2 startingPosition;

	// Targets
	public Transform target1;
	public Transform target2;
	public Transform target3;

	// Navigation mesh agent
	private NavMeshAgent agent;
	// Destination point
	private Vector3 endPoint;

	// Use this for initialization
	void Awake ()
	{
		if (Game.current.startingPositionX != 0.0f)
		{
			startingPosition = new Vector2(Game.current.startingPositionX, Game.current.startingPositionY);
			transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);
		}

		agent = GetComponentInChildren<NavMeshAgent>();
		agent.updateRotation = false;
	}

	// Update is called once per frame
	void Update ()
	{
		// Check if the screen is touched / clicked   
		if ((Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)) || (Input.GetMouseButton (0)))
		{
			// Declare a variable of RaycastHit struct
			RaycastHit hit;
			// Create a Ray on the tapped / clicked position
			Ray ray;

			// For unity editor
			#if UNITY_EDITOR
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			// For touch device
			#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			#endif

			// Check if the ray hits any collider
			if (Physics.Raycast (ray, out hit))
			{
				// Save the click / tap position
				endPoint = hit.point;
				agent.SetDestination(endPoint);
				//Debug.Log(endPoint);
			}
		}

		if (Input.GetKeyDown (KeyCode.Q))
		{
			//agent.SetDestination(target1.position);
		}
		else if (Input.GetKeyDown (KeyCode.W))
		{
			//agent.SetDestination(target2.position);
		}
		else if (Input.GetKeyDown (KeyCode.E))
		{
			//agent.SetDestination(target3.position);
		}
	}
}
