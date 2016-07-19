using UnityEngine;
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

    public void TriggerEvent(EventManager.Events eventAction)
    {
        if (eventAction == EventManager.Events.OPEN_KITCHEN_DOOR)
        {
            OpenKitchenDoor();
        }
        else if (eventAction == EventManager.Events.CHANGE_DEER_STATE)
        {
            //InteractWithDeer(true);
        }
    }

    public void OpenKitchenDoor()
    {
        if (!Game.current.triggeredEvents.Keys.Contains(EventManager.Events.OPEN_KITCHEN_DOOR))
        {
            Game.current.triggeredEvents.Add(EventManager.Events.OPEN_KITCHEN_DOOR, 0);
        }

        GameObject doorKitchen = GameObject.Find("Levels").transform.FindChild("Kitchen").FindChild("Objects").GetChild(1).gameObject;
        NpcBehaviour npcKitchen = GameObject.Find("NPC_Kitchen").GetComponent<NpcBehaviour>();
        GameObject doorKitchenNPC = GameObject.Find("NPC_FrontDoor");

        npcKitchen.textStartLine = 1;
        npcKitchen.textEndLine = 1;

        Destroy(doorKitchenNPC);
        doorKitchen.SetActive(true);

        foreach (EventTrigger trigger in eventTriggers)
        {
            if (trigger.eventAction == EventManager.Events.OPEN_KITCHEN_DOOR)
            {
                //eventTriggers.Remove(trigger);
                Destroy(trigger.gameObject);
            }
        }

        foreach (EventTrigger trigger in eventTriggers)
        {
            Debug.Log(trigger);
        }
    }

    public void InteractWithDeer(bool givingBerries)
    {
        int value = 0;

        if (!Game.current.triggeredEvents.Keys.Contains(EventManager.Events.CHANGE_DEER_STATE))
        {
            Game.current.triggeredEvents.Add(EventManager.Events.CHANGE_DEER_STATE, 0);
        }
        else
        {
            if (Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] < 1)
            {
                Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] += 1;
            }

            value = Game.current.triggeredEvents[Events.CHANGE_DEER_STATE];
        }

        if (givingBerries)
        {
            //do something
            return;
        }

        if (value == 0)
        {
            GameFlowManager.current.ChangeLines(0, 4);
        }
        else if (value == 1)
        {
            GameFlowManager.current.ChangeLines(6, 7);
        }
        else
        {
            Debug.Log("WERKS");
        }

        Debug.Log("display dialogue");
        GameFlowManager.current.ChangeLines(9, 11);
        GameFlowManager.current.npcBehav.behaviour = 2;
        GameFlowManager.current.isNPCAutomatic = true;
    }
}