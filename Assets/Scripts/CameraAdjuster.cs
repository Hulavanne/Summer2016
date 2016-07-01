using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour
{
	public float orthographicSize = 5;
	public float aspect = 1.42222f;
	public float width = 1;
	public float height = 1;

	GameObject player;

	void Update()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		//aspect = width / height;

		/*Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
			-orthographicSize, orthographicSize,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);*/

		Camera camera = Camera.main;
		Transform cameraTransform = camera.transform;

		//camera.orthographicSize = Screen.width / 100;
		cameraTransform.position = new Vector3(cameraTransform.position.x, player.transform.position.y + camera.orthographicSize, cameraTransform.position.z);
	}
}
