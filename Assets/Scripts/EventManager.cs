using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

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
                foreach (EventTrigger trigger in eventTriggers)
                {
                    if (Game.current.eventFlags.kitchenDoorOpen)
                    {
                        if (trigger.eventAction == EventTrigger.Events.OPEN_KITCHEN_DOOR)
                        {
                            OpenKitchenDoor(trigger.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void OpenKitchenDoor(GameObject triggerObject)
    {
        Game.current.eventFlags.kitchenDoorOpen = true;

        GameObject doorKitchen = GameObject.Find("Levels").transform.FindChild("Kitchen").FindChild("Objects").GetChild(1).gameObject;
        NpcBehaviour npcKitchen = GameObject.Find("NPC_Kitchen").GetComponent<NpcBehaviour>();
        GameObject doorKitchenNPC = GameObject.Find("NPC_FrontDoor");

        npcKitchen.textStartLine = 1;
        npcKitchen.textEndLine = 1;

        Destroy(doorKitchenNPC);
        doorKitchen.SetActive(true);
        Destroy(triggerObject);
    }
}
