﻿#if UNITY_EDITOR
//using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
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
        #if UNITY_EDITOR

        if (transform.GetComponent<UniqueId>() == null)
        {
            gameObject.AddComponent<UniqueId>();
        }

        #endif
    }

    void Update()
    {
        #if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            return;
        }

        #endif

        charges = itemData.charges;

        if (itemType == type.GLOVES && !itemData.collected)
        {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    public void UseItem()
    {
        if (PlayerController.current.overlappingNpc != null)
        {
            NpcBehaviour npcBehaviour = PlayerController.current.overlappingNpc.GetComponent<NpcBehaviour>();

            if (itemType == npcBehaviour.requiredItemType)
            {
                npcBehaviour.TriggerAction();
            }
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
