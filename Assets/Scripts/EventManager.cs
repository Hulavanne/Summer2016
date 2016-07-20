﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public enum Events
    {
        OPEN_KITCHEN_DOOR,
        CHANGE_DEER_STATE
    }

    public List<EventTrigger> eventTriggers = new List<EventTrigger>();

	void Awake()
    {
        current = this;
        eventTriggers = GameObject.FindObjectsOfType<EventTrigger>().ToList();
	}

    void Start()
    {
        if (Game.current != null)
        {
            if (!Game.current.newGame)
            {
                foreach (EventManager.Events triggeredEvent in Game.current.triggeredEvents.Keys)
                {
                    foreach (EventTrigger trigger in eventTriggers)
                    {
                        if (triggeredEvent == trigger.eventAction)
                        {
                            TriggerEvent(trigger.eventAction);
                        }
                    }
                }
            }
        }
    }

    public void TriggerEvent(Events eventAction)
    {
        if (eventAction == Events.OPEN_KITCHEN_DOOR)
        {
            OpenKitchenDoor();
        }
    }

    public void OpenKitchenDoor()
    {
        Game.current.AddToTriggeredEvents(Events.OPEN_KITCHEN_DOOR);

        GameObject doorKitchen = GameObject.Find("Levels").transform.FindChild("Kitchen").FindChild("Objects").GetChild(1).gameObject;
        NpcBehaviour npcKitchen = GameObject.Find("NPC_Kitchen").GetComponent<NpcBehaviour>();
        GameObject doorKitchenNPC = GameObject.Find("NPC_FrontDoor");

        npcKitchen.textStartLine = 1;
        npcKitchen.textEndLine = 1;

        Destroy(doorKitchenNPC);
        doorKitchen.SetActive(true);

        foreach (EventTrigger trigger in eventTriggers)
        {
            if (trigger.eventAction == Events.OPEN_KITCHEN_DOOR)
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }

    public void InteractWithDeer(bool givingBerries = false)
    {
        int value = 0;

        // Add event to triggeredEvents,
        // if event is already there,
        // set value to the event's value instead
        if (!Game.current.AddToTriggeredEvents(Events.CHANGE_DEER_STATE))
        {
            value = Game.current.triggeredEvents[Events.CHANGE_DEER_STATE];
        }

        // If player is giving berries
        if (givingBerries && Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] < 2)
        {
            // Set deer's state to 2
            Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] = 2;

            // Setup correct lines and start talking to the deer
            GameFlowManager.current.ChangeLines(9, 11);
            ActivateTextAtLine.current.TalkToNPC();

            // Add death cap to player's inventory and return the function
            Inventory.current.AddItemToInventory(GameObject.Find("DeathCap"));
            return;
        }

        // Deafult response
        if (value == 0)
        {
            GameFlowManager.current.ChangeLines(0, 4);
            Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] = 1;
        }
        // If spoken once already
        else if (value == 1)
        {
            GameFlowManager.current.ChangeLines(6, 7);
        }
        // State is upped to 3 right after giving berries
        else if (value == 2)
        {
            Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] = 3;
        }
        // Last state after 
        else if (value >= 3)
        {
            GameFlowManager.current.ChangeLines(13, 13);
        }
    }
}