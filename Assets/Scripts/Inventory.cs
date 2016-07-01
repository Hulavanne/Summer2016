using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
	public List<Item> items = new List<Item>();

	void Awake()
	{
		// Load in the items of the current game
		if (Game.current != null)
		{
			items = Game.current.items;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.S))
		{
			UseItem(items.Count - 1);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Item")
		{
			AddItemToInventory(other.gameObject);
			//other.gameObject.SetActive(false);
		}
	}

	void AddItemToInventory(GameObject item)
	{
		items.Add(item.GetComponent<ItemData>().item);
		Destroy(item.gameObject);
	}

	void UseItem(int index)
	{
		items.Remove(items[index]);
	}
}
