using UnityEngine;
using System.Collections;

public class ItemSlideMenu : MonoBehaviour
{
	float startPositionX;
	float goal;

	void Awake()
	{
		
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			startPositionX = Input.mousePosition.x;
			goal = transform.position.x - 900;
		}
		if (Input.GetMouseButton(0))
		{
			//if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width * 0.5)
			//{
			// Sliding left
			if (startPositionX - Input.mousePosition.x > 5)
			{
				Debug.Log("lefting");
				transform.position -= new Vector3(5,0,0);

				//float x = Mathf.Lerp(transform.position.x, goal, 5 * Time.deltaTime);
				//transform.position = new Vector3(x,transform.position.y,transform.position.z);
			}
			//Sliding Right
			if (startPositionX - Input.mousePosition.x < -5)
			{
				Debug.Log("righting");
				transform.position += new Vector3(5,0,0);
			}
			//}
		}
	}
}
