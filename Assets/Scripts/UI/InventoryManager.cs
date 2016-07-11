using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager current;

	public int currentSlideIndex = 0;
	public int numberOfSlides;

    public List<Item> sceneItems = new List<Item>(); // List of all item data in the scene

	Inventory inventory;
    ItemSlideMenu itemSlideMenu;
    List<Item> itemsInInventory = new List<Item>();
    List<List<Item>> itemsInSlides = new List<List<Item>>();

	void Awake()
	{
        current = this;

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        itemSlideMenu = transform.GetComponent<ItemSlideMenu>();

        // Get all items in the scene into a list
        if (GameObject.Find("Items") != null)
        {
            Transform itemsParent = GameObject.Find("Items").transform;

            for (int i = 0; i < itemsParent.childCount; ++i)
            {
                sceneItems.Add(itemsParent.GetChild(i).GetComponent<Item>());
            }
        }

        // Load in the items of the current game
        if (Game.current != null)
        {
            inventory.itemsData = Game.current.itemsInInventory;

            // If current game is a loaded game
            if (!Game.current.newGame)
            {
                // Search for saved scene items
                for (int i = 0; i < Game.current.itemsInScene.Count; ++i)
                {
                    // If saved items are found, set itemData to what was found and deactivate gameObject
                    if (Game.current.itemsInScene[i].index == sceneItems[i].index)
                    {
                        sceneItems[i].itemData = Game.current.itemsInScene[i];
                    }
                }
            }
        }

        // Run setup for all items in the scene
        if (GameObject.Find("Items") != null)
        {
            Transform itemsParent = GameObject.Find("Items").transform;

            for (int i = 0; i < itemsParent.childCount; ++i)
            {
                sceneItems[i].GetComponent<Item>().SetupData(i);
            }
        }
	}

    public List<Item> GetInventoryItemsData()
    {
        List<Item> data = new List<Item>();

        for (int i = 0; i < inventory.itemsData.Count; ++i)
        {
            data.Add(sceneItems[inventory.itemsData[i].index]);
        }

        return data;
    }

	public void SetSlideVariables()
	{
        // Clear item slots in item slides
        for (int i = 0; i < itemSlideMenu.itemSlides.Count; ++i)
        {
            itemSlideMenu.itemSlides[i].GetComponent<InventoryItemSlots>().ClearItemSlots();
        }

        // Get itemsData from inventory and convert the data to references for matching items in scene
        itemsInInventory = GetInventoryItemsData();

        // Determine the number of slides
		if (itemsInInventory.Count > 0)
		{
			numberOfSlides = Mathf.CeilToInt(itemsInInventory.Count / 3.0f);
		}
		else
		{
			numberOfSlides = 1;
		}

        // If there are 4 or more items in inventory, the slides can slide
		if (itemsInInventory.Count > 3)
		{
			itemSlideMenu.canSlide = true;
		}
		else
		{
			itemSlideMenu.canSlide = false;
		}

        // Clear the itemsInSlides list
		itemsInSlides.Clear();

        // And then fill it
		for (int i = 0; i < numberOfSlides; ++i)
		{
            List<Item> tempItemsList = new List<Item>();

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
	}

	public void FillItemSlots(string slidingDirection = "noDirection")
	{
        int itemsInSlidesIndex = 0;

		if (slidingDirection == "left")
		{
			itemsInSlidesIndex = currentSlideIndex + 1;

			if (itemsInSlidesIndex > numberOfSlides - 1)
			{
				itemsInSlidesIndex = 0;
			}

			SetItemsInSlots(2, itemsInSlidesIndex);
		}
		else if (slidingDirection == "right")
		{
			itemsInSlidesIndex = currentSlideIndex - 1;

			if (itemsInSlidesIndex < 0)
			{
				itemsInSlidesIndex = numberOfSlides - 1;
			}

			SetItemsInSlots(0, itemsInSlidesIndex);
		}
		else
		{
			SetItemsInSlots(1, itemsInSlidesIndex);
		}
	}

	void SetItemsInSlots(int nextSlideIndex, int itemsInSlidesIndex)
	{
		InventoryItemSlots itemSlide = itemSlideMenu.itemSlides[nextSlideIndex].GetComponent<InventoryItemSlots>();
		itemSlide.itemsInSlots.Clear();

		// Fill next slide
		if (itemsInSlides[itemsInSlidesIndex].Count > 0)
		{
			itemSlide.itemsInSlots.Add(itemsInSlides[itemsInSlidesIndex][0]);
		}
		if (itemsInSlides[itemsInSlidesIndex].Count > 1)
		{
			itemSlide.itemsInSlots.Add(itemsInSlides[itemsInSlidesIndex][1]);
		}
		if (itemsInSlides[itemsInSlidesIndex].Count > 2)
		{
			itemSlide.itemsInSlots.Add(itemsInSlides[itemsInSlidesIndex][2]);
		}

		itemSlide.AssignItemsToSlots();
	}
}
