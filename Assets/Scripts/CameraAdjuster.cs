using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour
{
	public float orthographicSize = 5;
	public float aspect = 1.42222f;
	public float width = 1;
	public float height = 1;

	void Start()
	{
		//aspect = width / height;

		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
			-orthographicSize, orthographicSize,
			Camera.main.nearClipPlane, Camera.main.farClipPlane);
	}
}
