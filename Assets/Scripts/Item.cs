using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent(typeof(UniqueId))]
public class Item : MonoBehaviour
{
    public bool usable = false;

    public enum type
    {
        WHATEVER,
        GLOVES,
    }
    public type itemType = type.WHATEVER;

    public string id;
    public string displayName;
    public Sprite icon;
    public string description;

    public ItemData itemData;

    void Start()
    {
#if UNITY_EDITOR

        id = transform.GetComponent<UniqueId>().uniqueId;

#endif

        if (itemType == type.GLOVES)
        {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    void Update()
    {
#if UNITY_EDITOR

        id = transform.GetComponent<UniqueId>().uniqueId;

#endif
    }

    /*public void SetupData(int indexNumber)
    {
        index = indexNumber;
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();

        bool done = false;

        if (Game.current != null)
        {
            // If current game is a loaded game
            if (!Game.current.newGame)
            {
                for (int i = 0; i < Game.current.itemsDataScene.Count; ++i)
                {
                    if (index == Game.current.itemsDataScene[i].index)
                    {
                        itemData = Game.current.itemsDataScene[i];
                        
                        break;
                    }
                }

                // Search for saved item data
                for (int i = 0; i < Game.current.itemsDataInventory.Count; ++i)
                {
                    // If saved data is found, set itemData to what was found and deactivate gameObject
                    if (index == Game.current.itemsDataInventory[i].index)
                    {
                        itemData = Game.current.itemsDataInventory[i];
                        done = true;
                        break;
                    }
                }
            }
        }

        // If object has already been collected before, deactivate it
        if (itemData.collected)
        {
            gameObject.SetActive(false);
        }

        if (done)
        {
            
        }
        // If no saved data was found, set itemData as a new class
        else
        {
            //itemData = new ItemData(index, gameObject.name, false);
        }
    }*/

    public void UseItem()
    {

    }
}
