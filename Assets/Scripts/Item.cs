#if UNITY_EDITOR
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
        DEATH_CAP,
        MORTAR_AND_PESTLE
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
        int chargesToRemove = 1;

        // Exceptions for when using berries
        if (itemType == Type.BERRIES)
        {
            if (PlayerController.current.overlappingNpc == null)
            {
                EventManager.current.EatBerries(0);
                return true;
            }
            else if (PlayerController.current.overlappingNpc.GetComponent<CharacterBehaviour>().npcType == CharacterBehaviour.Type.BEAR)
            {
                chargesToRemove = 0;
            }
        }

        if (!usable)
        {
            Debug.Log("Not usable");
            return false;
        }

        if (PlayerController.current.overlappingNpc != null)
        {
            CharacterBehaviour npcBehaviour = PlayerController.current.overlappingNpc.GetComponent<CharacterBehaviour>();

            if (npcBehaviour.requiredItemTypes.Contains(itemType))
            {
                npcBehaviour.TriggerAction(itemType);
            }
        }
        else
        {
            Debug.Log("No NpcBehaviour found");
            return false;
        }

        itemData.charges -= chargesToRemove;
        RemoveFromInventory();

        return true;
    }

    void RemoveFromInventory()
    {
        if (itemData.charges == 0)
        {
            // Remove the item from inventory
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
            inventory.RemoveItemFromInventory(itemData.id);
        }
    }
}
