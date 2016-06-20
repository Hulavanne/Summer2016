﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

public class InventoryItemSlots : MonoBehaviour
{
	public List<GameObject> itemSlots = new List<GameObject>();
	public List<GameObject> itemsInSlots = new List<GameObject>();
	public GameObject itemDescription;
	public bool inspectingItem = false;
	public int inspectedItemIndex = -1;

	List<bool> itemSlotOccupied = new List<bool>();
	List<Vector3> itemSlotPositions = new List<Vector3>();

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
					child.GetComponent<Image>().sprite = null;
					child.GetComponent<Text>().text = "";

					child.gameObject.SetActive(false);
				}
				itemSlots[i].GetComponent<Button>().interactable = false;
			}
		}
	}

	public void AssignItemsToSlots()
	{
		for (int i = 0; i < itemsInSlots.Count; ++i)
		{
			GameObject item = itemsInSlots[i];
			itemSlotOccupied [i] = true;

			if (item.GetComponent<ItemData>() != null)
			{
				ItemData itemData;
				itemData = item.GetComponent<ItemData>();

				foreach (Transform child in itemSlots[i].transform)
				{
					if (child.name == "ItemImage")
					{
						child.GetComponent<Image>().sprite = itemData.itemImage;
					}
					else if (child.name == "ItemNameText")
					{
						GameObject objectPrefab = (GameObject)PrefabUtility.GetPrefabParent(item.gameObject);
						child.GetComponent<Text>().text = item.name;
					}
				}
			}
			else
			{
				Debug.LogError("InventoryItemSlots -> AssignItemToSlots : ItemData not found in object '" + item.name + "'");
			}
		}
	}

	public void ClearItemSlots()
	{
		for (int i = 0; i < itemsInSlots.Count; ++i)
		{
			itemSlotOccupied[i] = false;
		}
	}

	public void InspectItem(int index)
	{
		if (inspectingItem)
		{
			StopInspectingItem();
		}
		else
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

			itemDescription.SetActive(true);
			itemDescription.GetComponentInChildren<Text>().text = itemsInSlots[index].GetComponent<ItemData>().description;
			inspectingItem = true;
			inspectedItemIndex = index;
		}
	}

	public void StopInspectingItem()
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

		itemDescription.SetActive(false);
		inspectingItem = false;
		inspectedItemIndex = -1;
	}

	public void UseItem()
	{
		if (inspectedItemIndex == 0)
		{
			
		}
		else if (inspectedItemIndex == 1)
		{
			
		}
		else if (inspectedItemIndex == 2)
		{
			
		}
	}
}
