using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<ItemData> itemsData = new List<ItemData>();

	void Update()
	{
        
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
        Item itemScript = item.GetComponent<Item>();
        ItemData itemData = itemScript.itemData;

        items.Add(itemScript);
        itemsData.Add(itemData);
        itemData.collected = true;

        if (item.GetComponent<SpriteRenderer>() != null)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (item.GetComponent<BoxCollider2D>() != null)
        {
            item.GetComponent<BoxCollider2D>().enabled = false;
        }
	}

    public void RemoveItemFromInventory(string itemId)
	{
        Debug.Log("Removed item");
        Debug.Log(itemId);

        List<Item> sceneItems = InventoryManager.current.sceneItems;

        for (int i = 0; i < sceneItems.Count; ++i)
        {
            if (itemId == sceneItems[i].id)
            {
                itemsData.Remove(sceneItems[i].itemData);
                items.Remove(sceneItems[i]);
                break;
            }
        }
	}
}
