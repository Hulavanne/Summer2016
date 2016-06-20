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
		if (Input.GetMouseButtonDown(0) && !slidesSliding && !EventSystem.current.IsPointerOverGameObject(0))
		{
			cursorStartPositionX = Input.mousePosition.x;
			cursorDistanceMoved = 0;
			inventoryManager.canFillItemSlots = true;
		}
		if (Input.GetMouseButton(0) && !slidesSliding && !touchReleased)
		{
			// Sliding left
			if (cursorStartPositionX - Input.mousePosition.x > 5)
			{
				for (int i = 0; i < itemSlides.Count; ++i)
				{
					RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
					slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
				}

				slidingDirection = "left";
				inventoryManager.FillItemSlots(slidingDirection);

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
				}
			}
			//Sliding Right
			if (cursorStartPositionX - Input.mousePosition.x < -5)
			{
				for (int i = 0; i < itemSlides.Count; ++i)
				{
					RectTransform slide = itemSlides[i].GetComponent<RectTransform>();
					slide.localPosition = new Vector3(slideStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slide.localPosition.y, slide.localPosition.z);
				}

				slidingDirection = "right";
				inventoryManager.FillItemSlots(slidingDirection);

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
				}
			}
		}
		if (Input.GetMouseButtonUp(0) && !slidesSliding)
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
		}

		if (slidesSliding)
		{
			SlideSlides(slidingDirection, 900, slideSpeed, true);
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
						SnapSlides ("left");
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
						SnapSlides ("right");
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

			if (inventoryManager.currentSlideIndex > inventoryManager.numberOfSlides)
			{
				inventoryManager.currentSlideIndex = 1;
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

			if (inventoryManager.currentSlideIndex < inventoryManager.numberOfSlides)
			{
				inventoryManager.currentSlideIndex = inventoryManager.numberOfSlides;
			}
		}
	}
}
