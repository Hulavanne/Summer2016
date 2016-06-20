using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
	public int currentSlideIndex = 0;
	public int numberOfSlides;
	public List<GameObject> itemSlides = new List<GameObject>();
	public bool canFillItemSlots = true;

	Inventory inventory;
	List<GameObject> itemsInInventory = new List<GameObject>();
	ItemSlideMenu itemSlideMenu;

	List<List<GameObject>> itemsInSlides = new List<List<GameObject>>();

	void Awake()
	{
		foreach (Transform child in transform)
		{
			itemSlides.Add(child.gameObject);
		}

		inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
		itemSlideMenu = transform.GetComponent<ItemSlideMenu>();
	}

	void Update()
	{
		
	}

	public void FillItemSlots(string slidingDirection = "noDirection")
	{
		SetSlideVariables();

		int nextSlideIndex = 0;

		if (slidingDirection == "left")
		{
			nextSlideIndex = currentSlideIndex - 1;

			if (nextSlideIndex < 0)
			{
				nextSlideIndex = numberOfSlides - 1;
			}

			SetItemsInSlots(2, nextSlideIndex);
		}
		else if (slidingDirection == "right")
		{
			nextSlideIndex = currentSlideIndex + 1;

			if (nextSlideIndex > numberOfSlides - 1)
			{
				nextSlideIndex = 0;
			}

			SetItemsInSlots(0, nextSlideIndex);
		}
		else
		{
			SetItemsInSlots(1, nextSlideIndex);
		}
	}

	void SetSlideVariables()
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

		for (int i = 0; i < itemsInSlides.Count; ++i)
		{
			for (int j = 0; j < itemsInSlides[i].Count; ++j)
			{
				Debug.Log(i);
				//Debug.Log(itemsInSlides[i][j]);
			}
		}
	}

	void SetItemsInSlots(int slideIndex, int itemsIndex)
	{
		InventoryItemSlots itemSlide = itemSlides[slideIndex].GetComponent<InventoryItemSlots>();
		itemSlide.itemsInSlots.Clear();

		// Fill next slide
		if (itemsInSlides[itemsIndex].Count > 0)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[0]);
		}
		if (itemsInSlides[itemsIndex].Count > 1)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[1]);
		}
		if (itemsInSlides[itemsIndex].Count > 2)
		{
			itemSlide.itemsInSlots.Add(itemsInInventory[2]);
		}

		// Clear the other slides
		if (slideIndex == 0)
		{
			itemSlides[1].GetComponent<InventoryItemSlots>().ClearItemSlots();
			itemSlides[2].GetComponent<InventoryItemSlots>().ClearItemSlots();
		}
		else if (slideIndex == 1)
		{
			itemSlides[0].GetComponent<InventoryItemSlots>().ClearItemSlots();
			itemSlides[2].GetComponent<InventoryItemSlots>().ClearItemSlots();
		}
		else if (slideIndex == 2)
		{
			itemSlides[0].GetComponent<InventoryItemSlots>().ClearItemSlots();
			itemSlides[1].GetComponent<InventoryItemSlots>().ClearItemSlots();
		}

		itemSlide.AssignItemsToSlots();
	}
}
