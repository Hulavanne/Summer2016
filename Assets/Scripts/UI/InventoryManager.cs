using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
	public int currentSlideNumber = 1;
	public int numberOfSlides;
	public GameObject itemInSlot;
	public List<GameObject> itemSlides = new List<GameObject>();
	public bool canFillItemSlots = true;

	Inventory inventory;
	List<GameObject> itemsInInventory = new List<GameObject>();
	ItemSlideMenu itemSlideMenu;

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
		itemsInInventory = inventory.itemsInInventory;

		if (itemsInInventory.Count > 0)
		{
			numberOfSlides = (int)Mathf.Ceil(itemsInInventory.Count / 3);
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

		for (int i = 0; i < itemsInInventory.Count; ++i)
		{
			
		}
	}

	public void fillItemSlots(string slidingDirection)
	{
		if (canFillItemSlots)
		{
			if (slidingDirection == "left")
			{
				
			}
			else if (slidingDirection == "right")
			{

			}
			else
			{
				InventoryItemSlots itemSlide = itemSlides[1].GetComponent<InventoryItemSlots>();
				itemSlide.itemsInSlots.Clear();

				if (itemsInInventory [0] != null)
				{
					itemSlide.itemsInSlots.Add(itemsInInventory[0]);
				}
				if (itemsInInventory [1] != null)
				{
					itemSlide.itemsInSlots.Add(itemsInInventory[1]);
				}
				if (itemsInInventory [2] != null)
				{
					itemSlide.itemsInSlots.Add(itemsInInventory[2]);
				}

				itemSlide.AssignItemsToSlots();
			}

			canFillItemSlots = false;
		}
	}
}
