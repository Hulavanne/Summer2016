using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<ItemData> itemsData = new List<ItemData>();

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
		{
            //RemoveItemFromInventory(itemIndices.Count - 1);
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
        itemsData.Add(item.GetComponent<Item>().itemData);
        item.SetActive(false);
	}

	public void RemoveItemFromInventory(int itemIndex)
	{
        Debug.Log("Removed item");
        Debug.Log(itemIndex);

        List<Item> sceneItems = InventoryManager.current.sceneItems;

        for (int i = 0; i < sceneItems.Count; ++i)
        {
            if (itemIndex == sceneItems[i].index)
            {
                itemsData.Remove(sceneItems[i].itemData);
                break;
            }
        }
	}
}
