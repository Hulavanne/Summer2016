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

    public enum Type
    {
        NONE,
        GLOVES,
        NIGHTSHADE,
        BERRIES,
        DEATH_CAP
    };
    public Type itemType = Type.NONE;

    public string displayName;
    public Sprite icon;
    public string description;

    public ItemData itemData = null;

    void Awake()
    {
        #if UNITY_EDITOR

        if (transform.GetComponent<UniqueId>() == null)
        {
            gameObject.AddComponent<UniqueId>();
        }

        #endif
    }

    void Start()
    {
        if (itemType == Type.GLOVES && !itemData.collected && Application.isPlaying)
        {
            Inventory.current.AddItemToInventory(gameObject);
        }
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
    }

    public bool UseItem()
    {
        if (itemType == Type.BERRIES && PlayerController.current.overlappingNpc == null)
        {
            EventManager.current.EatBerries(0);
            return true;
        }

        if (!usable)
        {
            Debug.Log("Not usable");
            return false;
        }

        if (PlayerController.current.overlappingNpc != null)
        {
            CharacterBehaviour npcBehaviour = PlayerController.current.overlappingNpc.GetComponent<CharacterBehaviour>();

            if (itemType == npcBehaviour.requiredItemType)
            {
                npcBehaviour.TriggerAction();
            }
        }
        else
        {
            Debug.Log("No NpcBehaviour found");
            return false;
        }

        RemoveFromInventory();
        return true;
    }

    void RemoveFromInventory()
    {
        --itemData.charges;

        if (itemData.charges == 0)
        {
            // Remove the item from inventory
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
            inventory.RemoveItemFromInventory(itemData.id);
        }
    }
}
