using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
	public static CameraEffects current;

    public GameObject darkScreen; // reference for the actual black plane
    public CanvasRenderer darkScreenRenderer; // reference for its renderer
    public Color opacityManager; // a color for (0 > red, 0 > green, 0 > blue, 0~1 > opacity)
    public float opacity = 1.0f; // the changeable opacity variable
    public bool fadeToBlack; // once this is true, the plane gradually turns black

    public float cameraPos = 0.0f, playerPos = 0.0f;
    public float xDistanceToPlayer = 0.0f;
    public GameObject player; // reference so camera follows player
    public PlayerController playerController;
    public TouchInput touchInput;

	Camera cameraComponent;
	BoxCollider2D cameraCollider;

	bool fixedCamera = true;
	bool boundaryColliding = false;
	BoxCollider2D collidingBoundary;

	void Awake()
	{
		current = this;

		darkScreen = GameObject.Find("InGameUI").transform.FindChild("GUI").FindChild("DarkScreen").gameObject;
		darkScreenRenderer = darkScreen.GetComponent<CanvasRenderer>();
		
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
			playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			touchInput = player.GetComponent<TouchInput>();
        }

		cameraComponent = transform.GetComponent<Camera>();
		cameraCollider = transform.GetComponent<BoxCollider2D>();

		AdjustCameraSize();
	}

	void Update ()
	{
		AdjustCameraSize();
		FadingToBlack();
        
		if (!fixedCamera)
        {
			float nextPositionX = player.transform.position.x;

			if (!boundaryColliding)
			{
				transform.position = new Vector3(nextPositionX, transform.position.y, transform.position.z);
			}
			else
			{
				if (playerController.movementDirection == 1 && collidingBoundary != null)
				{
					if (player.transform.position.x >= collidingBoundary.transform.position.x + collidingBoundary.bounds.extents.x + cameraCollider.bounds.extents.x)
					{
						transform.position = new Vector3(nextPositionX, transform.position.y, transform.position.z);
						boundaryColliding = false;
					}
				}
				else if (playerController.movementDirection == -1 && collidingBoundary != null)
				{
					if (player.transform.position.x <= collidingBoundary.transform.position.x - collidingBoundary.bounds.extents.x - cameraCollider.bounds.extents.x)
					{
						transform.position = new Vector3(nextPositionX, transform.position.y, transform.position.z);
						boundaryColliding = false;
					}
				}
			}
        }
    }

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "CameraBoundary")
		{
			boundaryColliding = true;
			collidingBoundary = (BoxCollider2D)other;
		}
	}

	public void AdjustToLevel(GameObject level)
	{
		if (level.GetComponent<Level>().fixedCamera)
		{
			transform.position = new Vector3(level.transform.position.x, transform.position.y, transform.position.z);
			fixedCamera = true;
			return;
		}
		else
		{
			fixedCamera = false;
		}

		Transform boundary = GetNearestBoundary(level);

        //Debug.Log(Mathf.Abs(boundary.position.x - player.transform.position.x) - boundary.GetComponent<BoxCollider2D>().bounds.extents.x);
        //Debug.Log(cameraCollider.bounds.extents.x);

		if (boundary != null)
		{
            if (Mathf.Abs(boundary.position.x - player.transform.position.x) - boundary.GetComponent<BoxCollider2D>().bounds.extents.x < cameraCollider.bounds.extents.x)
			{
                int direction = (int)Mathf.Clamp(player.transform.position.x - boundary.position.x, -1, 1);

                float x = boundary.position.x + direction * (boundary.GetComponent<BoxCollider2D>().bounds.extents.x + cameraCollider.bounds.extents.x);

                transform.position = new Vector3(x, transform.position.y, transform.position.z);

				boundaryColliding = true;
				collidingBoundary = boundary.GetComponent<BoxCollider2D>();

			}
			else
            {
				float x = player.transform.position.x;
				transform.position = new Vector3(x, transform.position.y, transform.position.z);

				boundaryColliding = false;
			}
		}
		else
		{
			float x = player.transform.position.x;
			transform.position = new Vector3(x, transform.position.y, transform.position.z);

			boundaryColliding = false;
		}
	}

	public Transform GetNearestBoundary(GameObject level)
	{
		if (level.transform.FindChild("Boundaries").FindChild("CameraBoundary") != null)
		{
            Transform boundary = level.transform.FindChild("Boundaries").FindChild("CameraBoundary");

            if (Mathf.Abs(boundary.position.x - player.transform.position.x) >= Mathf.Abs(boundary.GetChild(0).position.x - player.transform.position.x))
			{
				boundary = boundary.GetChild(0);
			}

			return boundary;
		}
		else
		{
			Debug.LogError("Boundary null!");
			return null;
		}
	}

	public void FadeToBlack(bool stayBlack)
	{
		fadeToBlack = stayBlack;
		opacity = 1.0f;
	}

	void AdjustCameraSize()
	{
		// Adjusting the size of the camera to fit the current screen resolution
		float ratio = (float)Screen.height / (float)Screen.width;
		float screenWidth = 1920.0f;
		float screenHeight = screenWidth * ratio;
		float size = screenHeight / 150.0f;
		cameraComponent.orthographicSize = size;

		// Making sure the camera is bound to the bottom of the background
		transform.position = new Vector3(transform.position.x, -10.2375f + cameraComponent.orthographicSize, transform.position.z);

		// Adjusting the collider to the camera's size
		float colliderWidth = size * 2 * (float)Screen.width / (float)Screen.height;
		float colliderHeight = size * 2;
		cameraCollider.size = new Vector2(colliderWidth, colliderHeight);
	}

	void FadingToBlack()
	{
		if (fadeToBlack)
		{
			if (opacity <= 1)
			{
				opacity += 1 * Time.deltaTime; // note that this "1" is a timer and isn't changing anything               
            }
		}
		else
		{
			if (opacity >= 0)
			{
				opacity -= 1 * Time.deltaTime; // note that this "1" is a timer and isn't changing anything
			}
		}

        if (opacity >= 1.0f)
        {
            darkScreenRenderer.transform.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            darkScreenRenderer.transform.GetComponent<Image>().raycastTarget = false;
        }

        darkScreenRenderer.SetAlpha(opacity);

		opacityManager = new Color(0.0f, 0.0f, 0.0f, opacity); // checks opacity every frame
		// darkScreenRenderer.material.color = opacityManager; // and puts it in the material
	}
}
