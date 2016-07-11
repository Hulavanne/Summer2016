using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public bool usable = false;

    public int index = -1;
	public string displayName;
	public Sprite icon;
	public string description;

    public ItemData itemData;

    public void SetupData(int indexNumber)
    {
        index = indexNumber;
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();

        bool done = false;

        if (Game.current != null)
        {
            // If current game is a loaded game
            if (!Game.current.newGame)
            {
                // Search for saved item data
                for (int i = 0; i < inventory.itemsData.Count; ++i)
                {
                    // If saved data is found, set itemData to what was found and deactivate gameObject
                    if (inventory.itemsData[i].index == index)
                    {
                        itemData = Game.current.itemsInInventory[i];
                        gameObject.SetActive(false);
                        done = true;
                        break;
                    }
                }
            }
        }

        // If no saved data was found, set itemData as a new class
        if (!done)
        {
            itemData = new ItemData(index, false);
        }

        // If object is already been used before, deactivate it
        if (itemData.used)
        {
            gameObject.SetActive(false);
        }
    }
}
