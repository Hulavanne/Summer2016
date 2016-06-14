using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MovementTest : MonoBehaviour
{
	Vector3 addXPos = new Vector3(.1f, 0, 0);

	void Awake ()
	{
		
	}

	void Update ()
	{
		// For unity editor
		#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width * 0.5)
			{
				GoLeft();
			}
			else
			{
				GoRight();
			}
		}
		// For touch device
		#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
		// If user is touching the screen
		if (Input.touchCount > 0)
		{
			// Check first touch (prevent issues with multi touch)
			Touch touch = Input.touches[0];
			int pointerID = touch.fingerId;

			// If user is not touching a button (eg. pause button)
			if (!EventSystem.current.IsPointerOverGameObject(pointerID))
			{
				// If user is touching left of screen
				if (touch.position.x >= 0 && touch.position.x <= Screen.width * 0.5)
				{
					GoLeft();
				}
				// If user is touching right of screen
				else
				{
					GoRight();
				}
			}
		}
		#endif

		if (Input.GetKey("a"))
		{
			GoLeft();
		}
		if (Input.GetKey("d"))
		{
			GoRight();
		}
	}

	void GoLeft()
	{
		transform.position -= addXPos;
	}

	void GoRight()
	{
		transform.position += addXPos;
	}
}
