using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemSlideMenu : MonoBehaviour
{
	public bool canSlide = true;
	public float slideSpeed = 10.0f;
	public float requiredDraggingDistance = 200.0f;
	public float slideSlidingDistance = 900.0f;
	public List<GameObject> itemSlides = new List<GameObject>();

	public List<float> slideStartPositionsX = new List<float>();
	float cursorStartPositionX = 0.0f;
	float cursorDistanceMoved = 0.0f;
	bool dragging = false;
	bool inputLocked = false;
	bool slidesSliding = false;
	bool slidesResetting = false;
	string slidingDirection = "left";

	InventoryManager inventoryManager;

	void Awake()
	{
		for (int i = 0; i < transform.childCount; ++i)
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

	void InputBegan(string inputType)
	{
		// Set the starting position of the finger / cursor
		if (inputType == "touch")
		{
			cursorStartPositionX = Input.touches[0].position.x;
		}
		else if (inputType == "mouse")
		{
			cursorStartPositionX = Input.mousePosition.x;
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

				// Slides are no loger being dragged
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

				// Slides are no loger being dragged
				dragging = true;
			}
		}
		else
		{
			// Loop through all the slides
			for (int i = 0; i < itemSlides.Count; ++i)
			{
				// Update the local position of the slide so that it follows the finger / cursor
				RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
				slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * cursorDistanceToStartPosition, slide.localPosition.y, slide.localPosition.z);
			}

			// Set the distance that the finger / cursor has moved since input began
			cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - cursorCurrentPositionX);

			// If the distance moved is long enough:
			if (cursorDistanceMoved >= requiredDraggingDistance)
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
					inputLocked = true;
				}

				// Slides are no loger being dragged
				dragging = false;
			}
		}
	}

	void InputEnded()
	{
		// If input is locked:
		if (inputLocked)
		{
			// Reset the slides back to their original position and release the input lock
			slidesResetting = false;
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

		// Slides are no loger being dragged
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
				if (Input.GetTouch(0).phase == TouchPhase.Moved && !slidesSliding && !InventoryItemSlots.inspectingItem && !inputLocked)
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
		if (Input.GetMouseButton(0) && !slidesSliding && !InventoryItemSlots.inspectingItem && !inputLocked)
		{
			InputMoving("mouse");
		}
		// If the left mouse button was just released:
		if (Input.GetMouseButtonUp(0) && !slidesSliding && !InventoryItemSlots.inspectingItem)
		{
			InputEnded();
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

		// For each item slide:
		for (int i = 0; i < itemSlides.Count; ++i)
		{
			// Lerp the slide's local position towards the target distance
			RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
			Vector3 targetPosition = new Vector3(slideStartPositionsX[i] + directionMultiplier * (distance + 5), slide.localPosition.y, slide.localPosition.z);
			slide.localPosition = Vector3.Lerp(slide.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

			bool destinationReached = false;

			if (direction == "left")
			{
				// If the slide has moved the wanted distance or more:
				if (slide.localPosition.x <= slideStartPositionsX[i] + directionMultiplier * distance)
				{
					destinationReached = true;
				}
			}
			else
			{
				// If the slide has moved the wanted distance or more:
				if (slide.localPosition.x >= slideStartPositionsX[i] + directionMultiplier * distance)
				{
					destinationReached = true;
				}
			}

			if (destinationReached)
			{
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
