using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour
{
	public float orthographicSize = 5.0f;
	public float targetAspect = 1.0f;
	public float windowAspect = 1.0f;
	public float width = 1.0f;
	public float height = 1.0f;

	GameObject player;

	// Use this for initialization
	void Start () 
	{
		
	}

	void Update()
	{
		//player = GameObject.FindGameObjectWithTag("Player");

		orthographicSize = Camera.main.orthographicSize;

		//width = Screen.width;
		//height = Screen.height;

		//aspect = height / width;
		targetAspect = width / height;

		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * targetAspect, orthographicSize * targetAspect,
			-orthographicSize, orthographicSize,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);

		//Camera camera = Camera.main;
		//Transform cameraTransform = camera.transform;

		//camera.orthographicSize = Screen.width / 100;
		//cameraTransform.position = new Vector3(cameraTransform.position.x, player.transform.position.y + camera.orthographicSize, cameraTransform.position.z);

		//float ratio = Screen.width / Screen.height;
		//cameraComponent.orthographicSize = Screen.width * ratio / 100.0f;

		//Debug.Log(cameraComponent.orthographicSize);





		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		/*float targetaspect = 1920.0f / 1080.0f;

		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;

		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;

			Rect rect = camera.rect;

			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}*/
	}
}
