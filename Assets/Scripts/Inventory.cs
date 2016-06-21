using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class Inventory : MonoBehaviour
{
	public List<GameObject> itemsInInventory = new List<GameObject>();

	void Awake()
	{
		
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.S))
		{
			useItem(itemsInInventory.Count - 1);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Item")
		{
			addItemToInventory(other.gameObject);
			other.gameObject.SetActive(false);
		}
	}

	void addItemToInventory(GameObject objectPrefab)
	{
		itemsInInventory.Add(objectPrefab);
	}

	void useItem(int index)
	{
		itemsInInventory.Remove(itemsInInventory[index]);
	}
}
