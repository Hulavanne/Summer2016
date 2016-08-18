using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager current;

	public int currentSlideIndex = 0;
	public int numberOfSlides = 0;

    public List<Item> sceneItems = new List<Item>(); // List of all items in the scene

	Inventory inventory;
    ItemSlideMenu itemSlideMenu;
    List<Item> itemsInInventory = new List<Item>();
    List<List<Item>> itemsInSlides = new List<List<Item>>();
    public List<ItemData> savedData = new List<ItemData>();

    void Awake()
    {
        current = this;
    }

    public void Setup()
	{
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        itemSlideMenu = transform.GetComponent<ItemSlideMenu>();

        sceneItems = GameObject.FindObjectsOfType<Item>().ToList();

        if (Game.current != null)
        {
            // Load in the items of the current game
            savedData = Game.current.itemsDataScene;
        }

        for (int i = 0; i < savedData.Count; ++i)
        {
            bool dataFound = false;

            if (Game.current != null)
            {
                // If current game is a loaded game
                if (!Game.current.newGame)
                {
                    // If saved items are found, set their data and deactivate their renderer and collider
                    for (int j = 0; j < sceneItems.Count; ++j)
                    {
                        if (savedData[i].id == sceneItems[j].GetComponent<UniqueId>().uniqueId)
                        {
                            sceneItems[j].itemData = savedData[i];

                            if (sceneItems[j].itemData.collected)
                            {
                                if (sceneItems[j].gameObject.GetComponent<SpriteRenderer>() != null)
                                {
                                    sceneItems[j].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                                }
                                if (sceneItems[j].gameObject.GetComponent<BoxCollider2D>() != null)
                                {
                                    sceneItems[j].gameObject.GetComponent<BoxCollider2D>().enabled = false;
                                }
                            }

                            dataFound = true;
                            break;
                        }
                    }
                }
            }

            // If the saved item wasn't found in the scene, print an error
            if (!dataFound)
            {
                Debug.LogError("No item found in scene for ID '" + savedData[i].id + "' | " + i);
            }
        }

        // Goes through all items in the scene and checks their item data, if nothing is found, new item data is attached
        for (int i = 0; i < sceneItems.Count; ++i)
        {
            if (string.IsNullOrEmpty(sceneItems[i].itemData.id))
            {
                //Debug.Log("No saved data found for " + sceneItems[i] + ", list index " + i);
                sceneItems[i].itemData = new ItemData(sceneItems[i].GetComponent<UniqueId>().uniqueId, sceneItems[i].charges);
            }
        }

        if (Game.current != null)
        {
            // If current game is a loaded game
            if (!Game.current.newGame)
            {
                // Adding saved items' data to inventory
                inventory.itemsData = Game.current.itemsDataInventory;

                // Adding saved items to inventory
                for (int i = 0; i < sceneItems.Count; ++i)
                {
                    for (int j = 0; j < inventory.itemsData.Count; ++j)
                    {
                        if (sceneItems[i].itemData.id == inventory.itemsData[j].id)
                        {
                            inventory.items.Add(sceneItems[i]);
                        }
                    }
                }
            }
        }
	}

    public List<Item> GetInventoryItemsData()
    {
        List<Item> data = new List<Item>();

        for (int i = 0; i < inventory.itemsData.Count; ++i)
        {
            for (int j = 0; j < sceneItems.Count; ++j)
            {
                if (inventory.itemsData[i].id == sceneItems[j].itemData.id)
                {
                    data.Add(sceneItems[j]);
                }
            }
        }

        return data;
    }

	public void SetSlideVariables()
	{
        for (int i = 0; i < itemSlideMenu.itemSlides.Count; ++i)
        {
            GameObject itemSlide = itemSlideMenu.itemSlides[i];

            // Clear item slots in item slides
            itemSlide.GetComponent<InventoryItemSlots>().ClearItemSlots();
            // Reset item slots' positions
            itemSlide.transform.localPosition = new Vector3(itemSlideMenu.slideStartPositionsX[i],
                itemSlide.transform.localPosition.y,
                itemSlide.transform.localPosition.z);
        }

        // Get itemsData from inventory and convert the data to references for matching items in scene
        itemsInInventory = GetInventoryItemsData();
        // Reset currentSlideIndex
        currentSlideIndex = 0;

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

	public void FillItemSlots(int slidingDirection = 0)
	{
        int itemsInSlidesIndex = 0;

        // Sliding to the left, fill the slide on the right
		if (slidingDirection == -1)
		{
			itemsInSlidesIndex = currentSlideIndex + 1;

			if (itemsInSlidesIndex > numberOfSlides - 1)
			{
				itemsInSlidesIndex = 0;
			}

			SetItemsInSlots(2, itemsInSlidesIndex);
        }
        // Sliding to the right, fill the slide on the left
		else if (slidingDirection == 1)
		{
			itemsInSlidesIndex = currentSlideIndex - 1;

			if (itemsInSlidesIndex < 0)
			{
				itemsInSlidesIndex = numberOfSlides - 1;
			}

			SetItemsInSlots(0, itemsInSlidesIndex);
		}
        // No sliding direction, fill the slide in the middle
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
