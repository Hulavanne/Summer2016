using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryItemSlots : MonoBehaviour
{
	public List<GameObject> itemSlots = new List<GameObject>();
	public List<GameObject> itemsInSlots = new List<GameObject>();
	public GameObject itemDescription;

	List<bool> itemSlotOccupied = new List<bool>();

	void Awake()
	{
		foreach (Transform child in transform)
		{
			if (child.name != "ItemDescription")
			{
				itemSlots.Add(child.gameObject);
				itemSlotOccupied.Add(false);
			}
			else
			{
				itemDescription = child.gameObject;
			}
		}
	}

	void Update()
	{
		for (int i = 0; i < itemSlots.Count; ++i)
		{
			if (itemSlotOccupied[i])
			{
				foreach (Transform child in itemSlots[i].transform)
				{
					child.gameObject.SetActive(true);
				}
				itemSlots[i].GetComponent<Button>().interactable = true;
			}
			else
			{
				foreach (Transform child in itemSlots[i].transform)
				{
					child.gameObject.SetActive(false);
				}
				itemSlots[i].GetComponent<Button>().interactable = false;
			}
		}
	}

	public void AssignItemsToSlots()
	{
		for (int i = 0; i < itemSlots.Count; ++i)
		{
			foreach (Transform child in itemSlots[i].transform)
			{
				if (child.name == "ItemImage")
				{
					//child.GetComponent<Image>().sprite = itemSlots[i].;
				}
				else if (child.name == "ItemNameText")
				{

				}
			}
		}
	}
}
