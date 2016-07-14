using UnityEngine;
using System;
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
    };
    public type itemType = type.WHATEVER;

    public string id;
    public string displayName;
    public Sprite icon;
    public string description;

    public ItemData itemData;

    void Awake()
    {
        #if UNITY_EDITOR

        if (transform.GetComponent<UniqueId>() != null)
        {
            id = transform.GetComponent<UniqueId>().uniqueId;
        }

        #endif

        if (itemType == type.GLOVES)
        {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    void Update()
    {
        #if UNITY_EDITOR

        if (transform.GetComponent<UniqueId>() != null)
        {
            id = transform.GetComponent<UniqueId>().uniqueId;
        }

        #endif
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
    }
}
