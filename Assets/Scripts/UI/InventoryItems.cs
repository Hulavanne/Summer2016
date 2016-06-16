using UnityEngine;
using System.Collections.Generic;

public class InventoryItems : MonoBehaviour
{
	public GameObject itemInSlot;

	Inventory inventory;
	List<GameObject> itemsInInventory = new List<GameObject>();

	void Awake ()
	{
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
	}

	void Update ()
	{
		itemsInInventory = inventory.itemsInInventory;

		for (int i = 0; i < itemsInInventory.Count; ++i)
		{

		}
	}
}
