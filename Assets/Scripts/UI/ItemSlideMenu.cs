using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemSlideMenu : MonoBehaviour
{
	public bool canSlide = true;
	public float slideSpeed = 10.0f;
	public float slideDraggingThreshold = 10.0f; // When dragging the screen if this threshold is reached the sliding will be triggered
	public float emptyDraggingThreshold = 100.0f; // Replaces the above value when canSlide is false
	public float slideSlidingDistance = 1000.0f;
	public List<GameObject> itemSlides = new List<GameObject>();

	List<float> slideStartPositionsX = new List<float>();
	public GameObject background;
	float cursorStartPositionX = 0.0f; // Position of the finger / cursor at the start of input
	float cursorDistanceMoved = 0.0f; // Distance the finger / cursor has moved since the input started
	bool dragging = false; // Is the player dragging the slides
	bool inputLocked = false; // Insures that only a single slide occurs
	bool slidesSliding = false;
	bool slidesResetting = false;
	string slidingDirection = "left";

	InventoryManager inventoryManager;

	void Awake()
	{
		background = transform.GetChild(0).gameObject;

		for (int i = 1; i < transform.childCount; ++i)
		{
			itemSlides.Add(transform.GetChild(i).gameObject);
			slideStartPositionsX.Add(0);
		}
		for (int i = 0; i < itemSlides.Count; ++i)
		{
			slideStartPositionsX[i] = itemSlides[i].GetComponent<RectTransform>().localPosition.x;
		}

		inventoryManager = transform.GetComponent<InventoryManager>();
	}

	void Update()
	{
		// For unity editor and computers
		#if (UNITY_EDITOR || UNITY_STANDALONE)

		MouseInput();

		// For touch devices
		#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)

		TouchInput();

		#endif

		// Updating slides
		UpdateSlides();
	}

	void TouchInput()
	{
		// If user is touching the screen
		if (Input.touchCount > 0)
		{
			// Check first touch (prevent issues with multi touch)
			Touch touch = Input.touches[0];
			int pointerID = touch.fingerId;

			// If user is not touching a button (eg. pause button)
			if (!EventSystem.current.IsPointerOverGameObject(pointerID))
			{
				// If user just began touching the screen
				//if ((Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary) && !slidesSliding)
				if (Input.GetTouch(0).phase == TouchPhase.Began && !slidesSliding && !InventoryItemSlots.inspectingItem)
				{
					InputBegan("touch");
				}
				// If input is ongoing and the finger is moving
				if (Input.GetTouch(0).phase == TouchPhase.Moved && !slidesSliding && !InventoryItemSlots.inspectingItem)
				{
					InputMoving("touch");
				}
				// If input just ended
				if (Input.GetTouch(0).phase == TouchPhase.Ended && !slidesSliding && !InventoryItemSlots.inspectingItem)
				{
					InputEnded();
				}
			}
		}
	}

	void MouseInput()
	{
		// If the left mouse button was just clicked:
		if (Input.GetMouseButtonDown(0) && !slidesSliding && !InventoryItemSlots.inspectingItem)
		{
			InputBegan("mouse");
		}
		// If the left mouse button is still pressed down:
		if (Input.GetMouseButton(0) && !slidesSliding && !InventoryItemSlots.inspectingItem)
		{
			InputMoving("mouse");
		}
		// If the left mouse button was just released:
		if (Input.GetMouseButtonUp(0) && !slidesSliding && !InventoryItemSlots.inspectingItem)
		{
			InputEnded();
		}
	}

	void InputBegan(string inputType)
	{
		// Set the starting position of the finger / cursor
		if (inputType == "touch")
		{
			cursorStartPositionX = Input.touches[0].position.x;
			inputLocked = false;
		}
		else if (inputType == "mouse")
		{
			cursorStartPositionX = Input.mousePosition.x;
			inputLocked = false;
		}
		else
		{
			// Error message
			cursorStartPositionX = 0.0f;
			Debug.LogError("ItemSlideMenu -> InputBegan : Invalid inputType: '" + inputType + "'");
		}

		// Reset cursorDistanceMoved and set variables for the slides of the inventory
		cursorDistanceMoved = 0.0f;
		inventoryManager.SetSlideVariables();
	}

	void InputMoving(string inputType)
	{
		if (!inputLocked)
		{
			// Set the current position of the finger / cursor
			float cursorCurrentPositionX = 0.0f;

			if (inputType == "touch")
			{
				cursorCurrentPositionX = Input.touches[0].position.x;
			}
			else if (inputType == "mouse")
			{
				cursorCurrentPositionX = Input.mousePosition.x;
			}
			else
			{
				// Error message
				cursorCurrentPositionX = 0.0f;
				Debug.LogError("ItemSlideMenu -> InputMoving : Invalid inputType: '" + inputType + "'");
			}

			// Set the distance of the finger / cursor to the start position
			float cursorDistanceToStartPosition = cursorStartPositionX - cursorCurrentPositionX;

			// If the slides are not yet being dragged:
			if (!dragging)
			{
				// If dragging left
				if (cursorDistanceToStartPosition > 5)
				{
					// Set direction
					slidingDirection = "left";

					// If the slides can be moved:
					if (canSlide)
					{
						// Fill the item slots of the slides
						inventoryManager.FillItemSlots(slidingDirection);
					}

					// Slides are being dragged
					dragging = true;
				}
				// If dragging right
				else if (cursorDistanceToStartPosition < -5)
				{
					slidingDirection = "right";

					// If the slides can be moved:
					if (canSlide)
					{
						// Fill the item slots of the slides
						inventoryManager.FillItemSlots(slidingDirection);
					}

					// Slides are being dragged
					dragging = true;
				}
			}
			else
			{
				RectTransform backgroundTransform = background.GetComponent<RectTransform>();
				backgroundTransform.localPosition = new Vector3(slideStartPositionsX[1] - 1 * cursorDistanceToStartPosition, backgroundTransform.localPosition.y, backgroundTransform.localPosition.z);

				// Loop through all the slides
				for (int i = 0; i < itemSlides.Count; ++i)
				{
					// Update the local position of the slide so that it follows the finger / cursor
					RectTransform slideTranform = itemSlides[i].GetComponent<RectTransform>();
					slideTranform.localPosition = new Vector3(slideStartPositionsX[i] - 1 * cursorDistanceToStartPosition, slideTranform.localPosition.y, slideTranform.localPosition.z);
				}

				// Set the distance that the finger / cursor has moved since input began
				cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - cursorCurrentPositionX);
				// When dragging the screen if this threshold is reached the sliding will be triggered
				float threshold = slideDraggingThreshold;

				// If the slides can't slide, use the secondary threshold instead
				if (!canSlide)
				{
					threshold = emptyDraggingThreshold;
				}

				// If the distance moved is long enough:
				if (cursorDistanceMoved >= threshold)
				{
					// If the slides can be moved:
					if (canSlide)
					{
						// Start sliding the slides
						slidesSliding = true;
					}
					// If the slides can't be moved:
					else
					{
						// Reset the slides back to their original position and lock input
						slidesResetting = true;
					}

					// Slides are no longer being dragged
					dragging = false;
					// Lock input to make sure only a single slide occurs
					inputLocked = true;
				}
			}
		}
	}

	void InputEnded()
	{
		// If input is locked:
		if (inputLocked)
		{
			// Release the lock
			inputLocked = false;
		}
		// If input isn't locked:
		else
		{
			// If the distance moved is long enough:
			if (cursorDistanceMoved > 5)
			{
				// Reset the slides back to their original position
				slidesResetting = true;
			}
		}

		// Slides are no longer being dragged
		dragging = false;
	}

	void UpdateSlides()
	{
		// If slides are sliding:
		if (slidesSliding)
		{
			// Update the position of the slides
			SlideSlides(slidingDirection, slideSlidingDistance, slideSpeed, true);
			// Slides are no longer being dragged
			dragging = false;
		}

		// If slides are resetting:
		if (slidesResetting)
		{
			// Reverse the sliding direction
			if (slidingDirection == "left")
			{
				slidingDirection = "right";
			}
			else
			{
				slidingDirection = "left";
			}

			// Update the position of the slides
			SlideSlides(slidingDirection, 0, slideSpeed, false);
			// Clear the outermost slides
			ClearOutermostSlides();
			// Slides are no longer being dragged
			dragging = false;
		}
	}

	void SlideSlides(string direction, float distance, float slidingSpeed, bool snapWhenFinished)
	{
		// Multiplies certain values depending on the direction
		int directionMultiplier = 1;

		if (direction == "left")
		{
			directionMultiplier *= -1;
		}

		RectTransform backgroundTransform = background.GetComponent<RectTransform>();
		Vector3 targetPosition = new Vector3(slideStartPositionsX[1] + directionMultiplier * (distance + 5), backgroundTransform.localPosition.y, backgroundTransform.localPosition.z);
		backgroundTransform.localPosition = Vector3.Lerp(backgroundTransform.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

		// For each item slide:
		for (int i = 0; i < itemSlides.Count; ++i)
		{
			// Lerp the slide's local position towards the target distance
			RectTransform slideTransform = itemSlides[i].GetComponent<RectTransform>();
			targetPosition = new Vector3(slideStartPositionsX[i] + directionMultiplier * (distance + 5), slideTransform.localPosition.y, slideTransform.localPosition.z);
			slideTransform.localPosition = Vector3.Lerp(slideTransform.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

			bool destinationReached = false;

			if (direction == "left")
			{
				// If the slide has moved the wanted distance or more:
				if (slideTransform.localPosition.x <= slideStartPositionsX[i] + directionMultiplier * distance)
				{
					destinationReached = true;
				}
			}
			else
			{
				// If the slide has moved the wanted distance or more:
				if (slideTransform.localPosition.x >= slideStartPositionsX[i] + directionMultiplier * distance)
				{
					destinationReached = true;
				}
			}

			if (destinationReached)
			{
				backgroundTransform.localPosition = new Vector3 (slideStartPositionsX[1], backgroundTransform.localPosition.y, backgroundTransform.localPosition.z);

				// For each item slide:
				for (int j = 0; j < itemSlides.Count; ++j)
				{
					// Set the slide's local position to the target distance
					RectTransform rectTransform = itemSlides[j].GetComponent<RectTransform>();
					rectTransform.localPosition = new Vector3 (slideStartPositionsX[j] + directionMultiplier * distance, rectTransform.localPosition.y, rectTransform.localPosition.z);

					// Set related booleans to false
					slidesSliding = false;
					slidesResetting = false;
					// Make sure the first for loop finishes
					i = 100;
				}

				if (snapWhenFinished)
				{
					SnapSlides(direction);
				}
			}
		}
	}

	void SnapSlides(string direction)
	{
		List<GameObject> tempSlides = new List<GameObject>();

		// Sliding left
		if (direction == "left")
		{
			// Take slides into a temporary and reorganize them
			tempSlides.Add(itemSlides[1]);
			tempSlides.Add(itemSlides[2]);
			tempSlides.Add(itemSlides[0]);

			// Replace current slide order with the new one
			itemSlides = tempSlides;

			// Set the local position of the last slide in the new order to the position of the previously last slide of the list
			RectTransform lastSlide = itemSlides[2].GetComponent<RectTransform>();
			lastSlide.localPosition = new Vector3(slideStartPositionsX[2], lastSlide.localPosition.y, lastSlide.localPosition.z);

			// Update currentSlideIndex in inventoryManager by adding 1
			++inventoryManager.currentSlideIndex;

			// If this was the last slide in the list:
			if (inventoryManager.currentSlideIndex > inventoryManager.numberOfSlides - 1)
			{
				inventoryManager.currentSlideIndex = 0;
			}
		}
		// Sliding right
		else
		{
			// Take slides into a temporary and reorganize them
			tempSlides.Add(itemSlides[2]);
			tempSlides.Add(itemSlides[0]);
			tempSlides.Add(itemSlides[1]);

			// Replace current slide order with the new one
			itemSlides = tempSlides;

			// Set the local position of the last slide in the new order to the position of the previously last slide of the list
			RectTransform lastSlide = itemSlides[0].GetComponent<RectTransform>();
			lastSlide.localPosition = new Vector3(slideStartPositionsX[0], lastSlide.localPosition.y, lastSlide.localPosition.z);

			// Update currentSlideIndex in inventoryManager by subtracting 1
			--inventoryManager.currentSlideIndex;

			// If this was the first slide in the list:
			if (inventoryManager.currentSlideIndex < 0)
			{
				inventoryManager.currentSlideIndex = inventoryManager.numberOfSlides - 1;
			}
		}

		ClearOutermostSlides();
	}

	void ClearOutermostSlides()
	{
		// Clear the outermost slides of items
		itemSlides[0].GetComponent<InventoryItemSlots>().ClearItemSlots();
		itemSlides[2].GetComponent<InventoryItemSlots>().ClearItemSlots();
	}
}
