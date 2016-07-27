using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItemSlots : MonoBehaviour
{
    public AudioClip useItemSuccessSound;
    public AudioClip useItemFailureSound;

	public List<GameObject> itemSlots = new List<GameObject>();
    public List<Item> itemsInSlots = new List<Item>();
	public GameObject itemDescriptionObject;

	public static bool inspectingItem = false;
    public static int inspectedItemIndex = -1;

	List<bool> itemSlotOccupied = new List<bool>();
	List<Vector3> itemSlotPositions = new List<Vector3>();
	ItemSlideMenu itemSlideMenu;

	void Awake()
	{
        foreach (Transform child in transform)
		{
			if (child.name != "ItemDescription")
			{
				itemSlots.Add(child.gameObject);
				itemSlotOccupied.Add(false);
				itemSlotPositions.Add(child.transform.localPosition);
			}
			else
			{
				itemDescriptionObject = child.gameObject;
				itemDescriptionObject.SetActive(false);
			}
		}

		for (int i = 0; i < itemSlots.Count; ++i)
		{
			DeactivateItemSlot(i);
		}

		itemSlideMenu = transform.GetComponentInParent<ItemSlideMenu>();
	}

	public void AssignItemsToSlots()
	{
		for (int i = 0; i < itemsInSlots.Count; ++i)
		{
            Item item = itemsInSlots[i];

			itemSlotOccupied[i] = true;
			ActivateItemSlot(i);

			foreach (Transform child in itemSlots[i].transform)
			{
				if (child.name == "ItemImage")
				{
                    child.GetComponent<Image>().sprite = item.icon;
				}
				else if (child.name == "ItemNameText")
				{
					child.GetComponent<Text>().text = item.name;
				}
			}
		}

		for (int i = 0; i < itemSlots.Count; ++i)
		{
			DeactivateItemSlot(i);
		}
		itemDescriptionObject.SetActive(false);
	}

	public void ActivateItemSlot(int index)
	{
		if (itemSlotOccupied[index])
		{
			foreach (Transform child in itemSlots[index].transform)
			{
				child.gameObject.SetActive(true);
			}
			itemSlots[index].GetComponent<Button>().interactable = true;
		}
	}

    public void ClearItemSlots()
    {
        for (int i = 0; i < itemsInSlots.Count; ++i)
        {
            itemSlotOccupied[i] = false;
            DeactivateItemSlot(i);
        }
    }

	public void DeactivateItemSlot(int index)
	{
		if (!itemSlotOccupied[index])
		{
			foreach (Transform child in itemSlots[index].transform)
			{
				if (child.name == "ItemImage")
				{
					child.GetComponent<Image>().sprite = null;
				}
				else if (child.name == "ItemNameText")
				{
					child.GetComponent<Text>().text = "";
				}
				child.gameObject.SetActive(false);
			}
			itemSlots[index].GetComponent<Button>().interactable = false;
		}
	}

	public void InspectItem(int index)
	{
        StopInspectingItem();

		if (!inspectingItem)
		{
			if (index == 0)
			{
				itemSlots[1].SetActive(false);
				itemSlots[2].SetActive(false);
			}
			else if (index == 1)
			{
				itemSlots[0].SetActive(false);
				itemSlots[2].SetActive(false);
			}
			else if (index == 2)
			{
				itemSlots[0].SetActive(false);
				itemSlots[1].SetActive(false);
			}
			itemSlots[index].transform.localPosition = itemSlots[0].transform.localPosition;

			itemDescriptionObject.SetActive(true);
			itemDescriptionObject.GetComponentInChildren<Text> ().text = itemsInSlots [index].description;// GetComponent<ItemData>().description;
			inspectingItem = true;
			inspectedItemIndex = index;
		}
	}

	public void StopInspectingItem()
	{
        if (inspectingItem)
        {
            if (inspectedItemIndex == 0)
            {
                itemSlots[1].SetActive(true);
                itemSlots[2].SetActive(true);
            }
            else if (inspectedItemIndex == 1)
            {
                itemSlots[0].SetActive(true);
                itemSlots[2].SetActive(true);
            }
            else if (inspectedItemIndex == 2)
            {
                itemSlots[0].SetActive(true);
                itemSlots[1].SetActive(true);
            }
            itemSlots[inspectedItemIndex].transform.localPosition = itemSlotPositions[inspectedItemIndex];

            itemDescriptionObject.SetActive(false);
            inspectingItem = false;
            inspectedItemIndex = -1;
        }
	}

	public void UseItem()
	{
        // If the item can be used
        if (itemsInSlots[inspectedItemIndex].usable)
        {
            // Use the item
            itemsInSlots[inspectedItemIndex].GetComponent<Item>().UseItem();

            // Stop the inspection and then resume game
            StopInspectingItem();
            transform.parent.parent.parent.GetComponent<MenuController>().ResumeGame();

            // Play sound effect for successfully using the item
            AudioManager.current.PlayRandomizedSoundEffect(useItemSuccessSound);
        }
        // If the item can't be used
        else
        {
            // Play sound effect for failing to use the item
            AudioManager.current.PlayRandomizedSoundEffect(useItemFailureSound);
        }
	}
}
