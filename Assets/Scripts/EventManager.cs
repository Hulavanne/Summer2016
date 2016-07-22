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
        CHANGE_DEER_STATE,
        CHANGE_LILIES_STATE,
        CHANGE_BLOCKNPC_STATE,
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

    public void InteractWithDeer(NpcBehaviour npcBehaviour, bool givingBerries = false)
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

            GameObject.Find("NPC_Deer").GetComponent<Animator>().SetBool("hasBerries", true);

            // Setup correct lines and start talking to the deer
            npcBehaviour.ChangeLines(9, 11);
            ActivateTextAtLine.current.TalkToNPC();

            // Add death cap to player's inventory and return the function
            Inventory.current.AddItemToInventory(GameObject.Find("DeathCap"));
            return;
        }

        // Deafult response
        if (value == 0)
        {
            npcBehaviour.ChangeLines(0, 4);
            Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] = 1;
        }
        // If spoken once already
        else if (value == 1)
        {
            npcBehaviour.ChangeLines(6, 7);
        }
        // State is upped to 3 right after giving berries
        else if (value == 2)
        {
            Game.current.triggeredEvents[Events.CHANGE_DEER_STATE] = 3;
        }
        // Last state after 
        else if (value >= 3)
        {
            npcBehaviour.ChangeLines(13, 13);
        }
    }

    public void InteractWithLilies()
    {
        int value = 0;

        // Add event to triggeredEvents,
        // if event is already there,
        // set value to the event's value instead
        if (!Game.current.AddToTriggeredEvents(Events.CHANGE_LILIES_STATE))
        {
            value = Game.current.triggeredEvents[Events.CHANGE_LILIES_STATE];
        }

        if (Game.current.triggeredEvents[Events.CHANGE_LILIES_STATE] == 0)
        {
            Game.current.triggeredEvents[Events.CHANGE_LILIES_STATE] = 1;

            foreach (Item item in InventoryManager.current.sceneItems)
            {
                if (item.itemType == Item.type.BERRIES)
                {
                    Inventory.current.AddItemToInventory(item.gameObject);
                    break;
                }
            }
        }

        PlayerController.current.overlappingNpc.GetComponent<NpcBehaviour>().ChangeLines(1, 1);
    }

    public void InteractWithBlockNPC(NpcBehaviour npcBehaviour, bool hasDeathCap = true)
    {
        if (hasDeathCap)
        {
            Game.current.triggeredEvents[Events.CHANGE_BLOCKNPC_STATE] = 2;
            
            npcBehaviour.ChangeLines(3, 4);
            ActivateTextAtLine.current.TalkToNPC();
            npcBehaviour.behaviour++;
            npcBehaviour.transform.FindChild("PlayerBoundary").gameObject.SetActive(false);
        }
    }

    // This gets called after speaking with an NPC
    public void NpcDialogueFinished(GameObject npc)
    {
        if (npc != null)
        {
            NpcBehaviour npcBehaviour = npc.GetComponent<NpcBehaviour>();

            if (npc.name == "NPC_Intro")
            {
                if (npc.GetComponent<IsIntro>().introPlaying)
                {
                    npc.GetComponent<IsIntro>().introPlaying = false;
                    npcBehaviour.ChangeLines(3, 4);
                    npcBehaviour.waitTimer = 2.0f;
                    PlayerController.current.canMove = true;
                    CameraEffects.current.fadeToBlack = false;
                }
            }

            if (npc.name == "NPC_Lilies")
            {
                InteractWithLilies();
                //ActivateTextAtLine.current.TalkToNPC();
            }

            if (npc.name == "NPC_Block")
            {
                npcBehaviour = GameObject.Find("NPC_Block").GetComponent<NpcBehaviour>();
                if (npcBehaviour.behaviour == 1)
                {
                    npcBehaviour.ChangeLines(6, 6);
                    npcBehaviour.behaviour++; 
                }
            }
        }
    }
}