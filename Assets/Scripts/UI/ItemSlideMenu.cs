using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemSlideMenu : MonoBehaviour
{
	public float slidingSpeed = 10.0f;
	public float requiredDraggingDistance = 200.0f;

	List<GameObject> itemSlots = new List<GameObject>();
	List<float> slotStartPositionsX = new List<float>();

	float cursorStartPositionX;
	float mouseDistanceMoved;

	bool slotsSliding = false;
	string slidingDirection = "left";

	void Awake()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			itemSlots.Add(transform.GetChild(i).gameObject);
			slotStartPositionsX.Add(0);
		}
		for (int i = 0; i < itemSlots.Count; ++i)
		{
			slotStartPositionsX[i] = itemSlots[i].GetComponent<RectTransform>().localPosition.x;
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !slotsSliding && !EventSystem.current.IsPointerOverGameObject(0))
		{
			cursorStartPositionX = Input.mousePosition.x;
			mouseDistanceMoved = 0;
		}
		if (Input.GetMouseButton(0) && !slotsSliding)
		{
			//if (Input.mousePosition.x >= 0 && Input.mousePosition.x <= Screen.width * 0.5)
			//{
			// Sliding left
			if (cursorStartPositionX - Input.mousePosition.x > 5)
			{
				for (int i = 0; i < itemSlots.Count; ++i)
				{
					RectTransform slot = itemSlots[i].GetComponent<RectTransform>();
					slot.localPosition = new Vector3(slotStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slot.localPosition.y, slot.localPosition.z);
				}

				mouseDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

				if (mouseDistanceMoved >= requiredDraggingDistance)
				{
					slotsSliding = true;
					slidingDirection = "left";
				}
			}
			//Sliding Right
			if (cursorStartPositionX - Input.mousePosition.x < -5)
			{
				for (int i = 0; i < itemSlots.Count; ++i)
				{
					RectTransform slot = itemSlots[i].GetComponent<RectTransform>();
					slot.localPosition = new Vector3(slotStartPositionsX[i] - 1 * (cursorStartPositionX - Input.mousePosition.x), slot.localPosition.y, slot.localPosition.z);
				}

				mouseDistanceMoved = Mathf.Abs(cursorStartPositionX - Input.mousePosition.x);

				if (mouseDistanceMoved >= requiredDraggingDistance)
				{
					slotsSliding = true;
					slidingDirection = "right";
				}
			}
			//}
		}
		if (Input.GetMouseButtonUp(0) && !slotsSliding)
		{
			for (int i = 0; i < itemSlots.Count; ++i)
			{
				RectTransform slot = itemSlots[i].GetComponent<RectTransform>();
				slot.localPosition = new Vector3(slotStartPositionsX[i], slot.localPosition.y, slot.localPosition.z);
			}
		}

		if (slotsSliding)
		{
			SlideSlots(slidingDirection);
		}
	}

	void SlideSlots(string direction)
	{
		if (direction == "left")
		{
			for (int i = 0; i < itemSlots.Count; ++i)
			{
				RectTransform slot = itemSlots[i].GetComponent<RectTransform>();
				Vector3 targetPosition = new Vector3(slotStartPositionsX [i] - 905, slot.localPosition.y, slot.localPosition.z);
				slot.localPosition = Vector3.Lerp(slot.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

				if (slot.localPosition.x <= slotStartPositionsX[i] - 900)
				{
					for (int j = 0; j < itemSlots.Count; ++j)
					{
						RectTransform rectTransform = itemSlots[j].GetComponent<RectTransform>();
						rectTransform.localPosition = new Vector3 (slotStartPositionsX[j] - 900, rectTransform.localPosition.y, rectTransform.localPosition.z);

						slotsSliding = false;
						i = 10;
					}

					SnapSlots("left");
				}
			}
		}
		else
		{
			for (int i = 0; i < itemSlots.Count; ++i)
			{
				RectTransform slot = itemSlots[i].GetComponent<RectTransform>();
				Vector3 targetPosition = new Vector3(slotStartPositionsX [i] + 905, slot.localPosition.y, slot.localPosition.z);
				slot.localPosition = Vector3.Lerp(slot.localPosition, targetPosition, slidingSpeed * Time.unscaledDeltaTime);

				if (slot.localPosition.x >= slotStartPositionsX[i] + 900)
				{
					for (int j = 0; j < itemSlots.Count; ++j)
					{
						RectTransform rectTransform = itemSlots[j].GetComponent<RectTransform>();
						rectTransform.localPosition = new Vector3 (slotStartPositionsX[j] + 900, rectTransform.localPosition.y, rectTransform.localPosition.z);

						slotsSliding = false;
						i = 10;
					}

					SnapSlots("right");
				}
			}
		}
	}

	void SnapSlots(string direction)
	{
		List<GameObject> tempSlots = new List<GameObject>();

		if (direction == "left")
		{
			tempSlots.Add(itemSlots[1]);
			tempSlots.Add(itemSlots[2]);
			tempSlots.Add(itemSlots[0]);

			itemSlots = tempSlots;

			RectTransform lastSlot = itemSlots[2].GetComponent<RectTransform>();
			lastSlot.localPosition = new Vector3(slotStartPositionsX[2], lastSlot.localPosition.y, lastSlot.localPosition.z);
		}
		else
		{
			tempSlots.Add(itemSlots[2]);
			tempSlots.Add(itemSlots[0]);
			tempSlots.Add(itemSlots[1]);

			itemSlots = tempSlots;

			RectTransform lastSlot = itemSlots[0].GetComponent<RectTransform>();
			lastSlot.localPosition = new Vector3(slotStartPositionsX[0], lastSlot.localPosition.y, lastSlot.localPosition.z);
		}
	}
}
