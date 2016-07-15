#if UNITY_EDITOR
//using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public bool usable = false;
    public int charges = 1;

    public enum type
    {
        WHATEVER,
        GLOVES,
    };
    public type itemType = type.WHATEVER;

    public string displayName;
    public Sprite icon;
    public string description;

    public ItemData itemData;

    void Awake()
    {
        if (itemType == type.GLOVES)
        {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    void Update()
    {
        charges = itemData.charges;
    }

    public void UseItem()
    {
        if (PlayerController.current.overlappingNpc != null)
        {
            PlayerController.current.overlappingNpc.GetComponent<NpcBehaviour>().TriggerAction();
        }
        else
        {
            Debug.Log("No NpcBehaviour found");
        }

        --itemData.charges;

        if (itemData.charges == 0)
        {
            // Remove the item from inventory
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
            inventory.RemoveItemFromInventory(itemData.id);
        }
    }
}
