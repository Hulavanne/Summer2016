using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
	public int currentSlideIndex = 0;
	public int numberOfSlides;

	Inventory inventory;
	List<GameObject> itemsInInventory = new List<GameObject>();
	List<List<GameObject>> itemsInSlides = new List<List<GameObject>>();
	ItemSlideMenu itemSlideMenu;

	void Awake()
	{
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
		itemSlideMenu = transform.GetComponent<ItemSlideMenu>();
		SetSlideVariables();
	}

	public void SetSlideVariables()
	{
		itemsInInventory = inventory.itemsInInventory;

		if (itemsInInventory.Count > 0)
		{
			numberOfSlides = Mathf.CeilToInt(itemsInInventory.Count / 3.0f);
		}
		else
		{
			numberOfSlides = 1;
		}

		if (itemsInInventory.Count > 3)
		{
			itemSlideMenu.canSlide = true;
		}
		else
		{
			itemSlideMenu.canSlide = false;
		}
	}

	public void FillItemSlots(string slidingDirection = "noDirection")
	{
		int nextSlideIndex = 0;

		if (slidingDirection == "left")
		{
			nextSlideIndex = currentSlideIndex + 1;

			if (nextSlideIndex > numberOfSlides - 1)
			{
				nextSlideIndex = 0;
			}

			SetItemsInSlots(2, nextSlideIndex);
		}
		else if (slidingDirection == "right")
		{
			nextSlideIndex = currentSlideIndex - 1;

			if (nextSlideIndex < 0)
			{
				nextSlideIndex = numberOfSlides - 1;
			}

			SetItemsInSlots(0, nextSlideIndex);
		}
		else
		{
			SetItemsInSlots(1, nextSlideIndex);
		}
	}

	void SetItemsInSlots(int nextSlideIndex, int itemsInSlidesIndex)
	{
		itemsInSlides.Clear();

		for (int i = 0; i < numberOfSlides; ++i)
		{
			List<GameObject> tempItemsList = new List<GameObject>();

			if (itemsInInventory.Count > i * 3)
			{
				tempItemsList.Add(itemsInInventory[i * 3]);
			}
			if (itemsInInventory.Count > i * 3 + 1)
			{
				tempItemsList.Add(itemsInInventory[i * 3 + 1]);
			}
			if (itemsInInventory.Count > i * 3 + 2)
			{
				tempItemsList.Add(itemsInInventory[i * 3 + 2]);
			}

			itemsInSlides.Add(tempItemsList);
		}

		InventoryItemSlots itemSlide = itemSlideMenu.itemSlides[nextSlideIndex].GetComponent<InventoryItemSlots>();
		itemSlide.itemsInSlots.Clear();

		// Fill next slide
		if (itemsInSlides[itemsInSlidesIndex].Count > 0)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[0]);
		}
		if (itemsInSlides[itemsInSlidesIndex].Count > 1)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[1]);
		}
		if (itemsInSlides[itemsInSlidesIndex].Count > 2)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[2]);
		}

		itemSlide.AssignItemsToSlots();
	}
}
