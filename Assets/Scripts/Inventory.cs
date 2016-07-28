using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public static Inventory current;

    public List<Item> items = new List<Item>();
    public List<ItemData> itemsData = new List<ItemData>();

    void Awake()
    {
        current = this;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Item")
		{
            AddItemToInventory(other.gameObject);
		}
	}

    public void AddItemToInventory(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        ItemData itemData = item.itemData;

        if (!itemData.collected)
        {
            items.Add(item);
            itemsData.Add(itemData);
            itemData.collected = true;

            if (itemObject.GetComponent<SpriteRenderer>() != null)
            {
                itemObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (itemObject.GetComponent<BoxCollider2D>() != null)
            {
                itemObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            Debug.Log("Item '" + itemObject + "' already collected");
        }
    }

    public void AddItemToInventory(Item.Type type)
    {
        foreach (Item item in InventoryManager.current.sceneItems)
        {
            if (type == item.itemType)
            {
                ItemData itemData = item.itemData;

                if (!itemData.collected)
                {
                    items.Add(item);
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
                    return;
                }
                else
                {
                    Debug.Log("Item '" + item.gameObject + "' already collected");
                }
            }
        }
        Debug.Log("No uncollected item of type '" + type + "' found in scene");
    }

    public void RemoveItemFromInventory(string itemId)
	{
        List<Item> sceneItems = InventoryManager.current.sceneItems;

        for (int i = 0; i < sceneItems.Count; ++i)
        {
            if (itemId == sceneItems[i].itemData.id)
            {
                itemsData.Remove(sceneItems[i].itemData);
                items.Remove(sceneItems[i]);
                break;
            }
        }
	}
}
