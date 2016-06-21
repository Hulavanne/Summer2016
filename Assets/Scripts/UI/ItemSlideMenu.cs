using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemSlideMenu : MonoBehaviour
{
	public float slideSpeed = 10.0f;
	public float requiredDraggingDistance = 200.0f;
	public List<GameObject> itemSlides = new List<GameObject>();
	public bool canSlide = true;

	List<float> slideStartPositionsX = new List<float>();
	float cursorStartPositionX;
	float cursorDistanceMoved;
	bool dragging = false;
	bool touchReleased = false;
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
			if (!EventSystem.current.IsPointerOverGameObject (pointerID))
			{
				if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary)
				{
					cursorStartPositionX = touch.position.x;
					cursorDistanceMoved = 0;
					inventoryManager.SetSlideVariables();
				}
				if (Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					// Sliding left
					if (cursorStartPositionX - Input.mousePosition.x > 5)
					{
						if (!dragging)
						{
							slidingDirection = "left";
							dragging = true;

							if (canSlide)
							{
								inventoryManager.FillItemSlots(slidingDirection);
							}
						}

						for (int i = 0; i < itemSlides.Count; ++i)
						{
							RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
							slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
						}

						cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

						if (cursorDistanceMoved >= requiredDraggingDistance)
						{
							if (canSlide)
							{
								slidesSliding = true;
							}
							else
							{
								slidesResetting = true;
								touchReleased = true;
							}
							dragging = false;
						}
					}
					// Sliding Right
					if (cursorStartPositionX - Input.mousePosition.x < -5)
					{
						if (!dragging)
						{
							slidingDirection = "right";
							dragging = true;

							if (canSlide)
							{
								inventoryManager.FillItemSlots(slidingDirection);
							}
						}

						for (int i = 0; i < itemSlides.Count; ++i)
						{
							RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
							slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
						}

						cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

						if (cursorDistanceMoved >= requiredDraggingDistance)
						{
							if (canSlide)
							{
								slidesSliding = true;
							}
							else
							{
								slidesResetting = true;
								touchReleased = true;
							}
							dragging = false;
						}
					}
				}
				if (Input.GetTouch(0).phase == TouchPhase.Ended)
				{

				}
			}
		}
	}

	void MouseInput()
	{
		if (Input.GetMouseButtonDown(0) && !slidesSliding)
		{
			cursorStartPositionX = Input.mousePosition.x;
			cursorDistanceMoved = 0;
			inventoryManager.SetSlideVariables();
		}
		if (Input.GetMouseButton(0) && !slidesSliding && !touchReleased && !InventoryItemSlots.inspectingItem)
		{
			// Sliding left
			if (cursorStartPositionX - Input.mousePosition.x > 5)
			{
				if (!dragging)
				{
					slidingDirection = "left";
					dragging = true;

					if (canSlide)
					{
						inventoryManager.FillItemSlots(slidingDirection);
					}
				}

				for (int i = 0; i < itemSlides.Count; ++i)
				{
					RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
					slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
				}

				cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

				if (cursorDistanceMoved >= requiredDraggingDistance)
				{
					if (canSlide)
					{
						slidesSliding = true;
					}
					else
					{
						slidesResetting = true;
						touchReleased = true;
					}
					dragging = false;
				}
			}
			// Sliding Right
			if (cursorStartPositionX - Input.mousePosition.x < -5)
			{
				if (!dragging)
				{
					slidingDirection = "right";
					dragging = true;

					if (canSlide)
					{
						inventoryManager.FillItemSlots(slidingDirection);
					}
				}

				for (int i = 0; i < itemSlides.Count; ++i)
				{
					RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
					slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
				}

				cursorDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

				if (cursorDistanceMoved >= requiredDraggingDistance)
				{
					if (canSlide)
					{
						slidesSliding = true;
					}
					else
					{
						slidesResetting = true;
						touchReleased = true;
					}
					dragging = false;
				}
			}
		}
		if (Input.GetMouseButtonUp(0) && !slidesSliding && !InventoryItemSlots.inspectingItem)
		{
			if (touchReleased)
			{
				slidesResetting = false;
				touchReleased = false;
			}
			else
			{
				if (cursorDistanceMoved > 5)
				{
					slidesResetting = true;
				}
			}
			dragging = false;
		}

		if (slidesSliding)
		{
			SlideSlides(slidingDirection, 900, slideSpeed, true);
			dragging = false;
		}
		if (slidesResetting)
		{
			if (slidingDirection == "left")
			{
				slidingDirection = "right";
			}
			else
			{
				slidingDirection = "left";
			}

			SlideSlides(slidingDirection, 0, slideSpeed, false);
			ClearOutermostSlides();
			dragging = false;
		}
	}

	void SlideSlides(string direction, float distance, float slidingSpeed, bool snapWhenFinished)
	{
		if (direction == "left")
		{
			for (int i = 0; i < itemSlides.Count; ++i)
			{
				RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
				Vector3 targetPosition = new Vector3(slideStartPositionsX[i] - (distance + 5), slide.localPosition.y, slide.localPosition.z);
				slide.localPosition = Vector3.Lerp(slide.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

				if (slide.localPosition.x <= slideStartPositionsX[i] - distance)
				{
					for (int j = 0; j < itemSlides.Count; ++j)
					{
						RectTransform rectTransform = itemSlides[j].GetComponent<RectTransform>();
						rectTransform.localPosition = new Vector3 (slideStartPositionsX[j] - distance, rectTransform.localPosition.y, rectTransform.localPosition.z);

						slidesSliding = false;
						slidesResetting = false;
						i = 100;
					}

					if (snapWhenFinished)
					{
						SnapSlides("left");
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < itemSlides.Count; ++i)
			{
				RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
				Vector3 targetPosition = new Vector3(slideStartPositionsX[i] + (distance + 5), slide.localPosition.y, slide.localPosition.z);
				slide.localPosition = Vector3.Lerp(slide.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

				if (slide.localPosition.x >= slideStartPositionsX[i] + distance)
				{
					for (int j = 0; j < itemSlides.Count; ++j)
					{
						RectTransform rectTransform = itemSlides[j].GetComponent<RectTransform>();
						rectTransform.localPosition = new Vector3 (slideStartPositionsX[j] + distance, rectTransform.localPosition.y, rectTransform.localPosition.z);

						slidesSliding = false;
						slidesResetting = false;
						i = 100;
					}

					if (snapWhenFinished)
					{
						SnapSlides("right");
					}
				}
			}
		}
	}

	void SnapSlides(string direction)
	{
		List<GameObject> tempSlides = new List<GameObject>();

		if (direction == "left")
		{
			tempSlides.Add(itemSlides[1]);
			tempSlides.Add(itemSlides[2]);
			tempSlides.Add(itemSlides[0]);

			itemSlides = tempSlides;

			RectTransform lastSlide = itemSlides[2].GetComponent<RectTransform>();
			lastSlide.localPosition = new Vector3(slideStartPositionsX[2], lastSlide.localPosition.y, lastSlide.localPosition.z);

			++inventoryManager.currentSlideIndex;

			if (inventoryManager.currentSlideIndex > inventoryManager.numberOfSlides - 1)
			{
				inventoryManager.currentSlideIndex = 0;
			}
		}
		else
		{
			tempSlides.Add(itemSlides[2]);
			tempSlides.Add(itemSlides[0]);
			tempSlides.Add(itemSlides[1]);

			itemSlides = tempSlides;

			RectTransform lastSlide = itemSlides[0].GetComponent<RectTransform>();
			lastSlide.localPosition = new Vector3(slideStartPositionsX[0], lastSlide.localPosition.y, lastSlide.localPosition.z);

			--inventoryManager.currentSlideIndex;

			if (inventoryManager.currentSlideIndex < 0)
			{
				inventoryManager.currentSlideIndex = inventoryManager.numberOfSlides - 1;
			}
		}

		ClearOutermostSlides();
	}

	// Clear the outermost slides
	void ClearOutermostSlides()
	{
		itemSlides[0].GetComponent<InventoryItemSlots>().ClearItemSlots();
		itemSlides[2].GetComponent<InventoryItemSlots>().ClearItemSlots();
	}
}
