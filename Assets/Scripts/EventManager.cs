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
    }

    public void OpenKitchenDoor()
    {
        if (!Game.current.triggeredEvents.Keys.Contains(EventManager.Events.OPEN_KITCHEN_DOOR))
        {
            Game.current.triggeredEvents.Add(EventManager.Events.OPEN_KITCHEN_DOOR, 0);
            Debug.Log("ADDED");
        }
        else Debug.Log("NOT ADDED");

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

    public void TalkToDeer()
    {
        //(int)Events.DEER_STATE = 4;
    }
}
