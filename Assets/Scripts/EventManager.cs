using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    /*public enum Events
    {
        OPEN_KITCHEN_DOOR,
        CHANGE_DEER_STATE,
        CHANGE_LILIES_STATE,
        CHANGE_BLOCKER_STATE,
    }*/

    public List<NpcBehaviour> npcBehaviours = new List<NpcBehaviour>();
    public List<EventTrigger> eventTriggers = new List<EventTrigger>();

	void Awake()
    {
        current = this;

        npcBehaviours = GameObject.FindObjectsOfType<NpcBehaviour>().ToList();
        eventTriggers = GameObject.FindObjectsOfType<EventTrigger>().ToList();
	}

    void Start()
    {
        if (Game.current != null)
        {
            if (!Game.current.newGame)
            {
                foreach (NpcBehaviour.Type triggeredEvent in Game.current.triggeredEvents.Keys)
                {
                    TriggerEvent(triggeredEvent);
                }
            }
        }
    }

    public void TriggerEvent(NpcBehaviour.Type eventType)
    {
        if (eventType == NpcBehaviour.Type.FRONT_DOOR)
        {
            OpenKitchenDoor();
        }
    }

    public void OpenKitchenDoor()
    {
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.FRONT_DOOR);

        foreach (NpcBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.npcType == NpcBehaviour.Type.KITCHEN)
            {
                behaviour.textStartLine = 1;
                behaviour.textEndLine = 1;
            }
            else if (behaviour.npcType == NpcBehaviour.Type.FRONT_DOOR)
            {
                behaviour.gameObject.SetActive(false);
            }
        }

        GameObject doorKitchen = GameObject.Find("Levels").transform.FindChild("Kitchen").FindChild("Objects").GetChild(1).gameObject;
        doorKitchen.SetActive(true);

        foreach (EventTrigger trigger in eventTriggers)
        {
            if (trigger.eventType == NpcBehaviour.Type.FRONT_DOOR)
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }

    public void InteractWithDeer(NpcBehaviour npcBehaviour, bool givingBerries = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.DEER);

        // If player is giving berries
        if (givingBerries && Game.current.triggeredEvents[NpcBehaviour.Type.DEER] < 2)
        {
            // Set deer's state to 2
            Game.current.triggeredEvents[NpcBehaviour.Type.DEER] = 2;
        }

        value = Game.current.triggeredEvents[NpcBehaviour.Type.DEER];

        // Deafult response
        if (value == 0)
        {
            npcBehaviour.ChangeLines(0, 4);
            Game.current.triggeredEvents[NpcBehaviour.Type.DEER] = 1;
        }
        // If spoken once already
        else if (value == 1)
        {
            npcBehaviour.ChangeLines(6, 7);
        }
        // State is upped to 3 right after giving berries
        else if (value == 2)
        {
            // Setup correct lines and start talking to the deer
            npcBehaviour.ChangeLines(9, 11);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Add death cap to player's inventory
            Inventory.current.AddItemToInventory(Item.Type.DEATH_CAP);

            // Set state to 3
            Game.current.triggeredEvents[NpcBehaviour.Type.DEER] = 3;
        }
        // Last state after 
        else if (value >= 3)
        {
            npcBehaviour.ChangeLines(13, 13);
        }
    }

    public void InteractWithBlocker(NpcBehaviour npcBehaviour, bool givingDeathCap = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.BLOCKER);

        if (givingDeathCap && Game.current.triggeredEvents[NpcBehaviour.Type.BLOCKER] < 1)
        {
            Game.current.triggeredEvents[NpcBehaviour.Type.BLOCKER] = 1;
        }

        value = Game.current.triggeredEvents[NpcBehaviour.Type.BLOCKER];

        // Default response
        if (value == 0)
        {
            npcBehaviour.ChangeLines(0, 1);
        }
        // If giving death cap
        if (value == 1)
        {
            npcBehaviour.ChangeLines(3, 4);
            ActivateTextAtLine.current.TalkToNPC(false);
            Game.current.triggeredEvents[NpcBehaviour.Type.BLOCKER] = 2;
        }
        // If NPC is dead
        else if (value >= 2)
        {
            npcBehaviour.ChangeLines(6, 6);
        }
    }

    public void InteractWithLilies()
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.LILIES);
        value = Game.current.triggeredEvents[NpcBehaviour.Type.LILIES];

        if (value == 0)
        {
            Game.current.triggeredEvents[NpcBehaviour.Type.LILIES] = 1;
            Inventory.current.AddItemToInventory(Item.Type.BERRIES);
        }
        else
        {
            PlayerController.current.overlappingNpc.GetComponent<NpcBehaviour>().ChangeLines(1, 1);
        }
    }

    // This gets called after speaking with an NPC
    public void NpcDialogueFinished(GameObject npc)
    {
        if (npc != null)
        {
            NpcBehaviour npcBehaviour = npc.GetComponent<NpcBehaviour>();

            if (npcBehaviour.npcType == NpcBehaviour.Type.INTRO)
            {
                if (npc.GetComponent<IsIntro>().introPlaying)
                {
                    npc.GetComponent<IsIntro>().introPlaying = false;
                    npcBehaviour.ChangeLines(3, 4);
                    npcBehaviour.waitTimer = 1.5f;
                    CameraEffects.current.fadeToBlack = false;
                }
            }
        }
    }
}