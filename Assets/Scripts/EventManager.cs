using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public List<EventTrigger> eventTriggers = new List<EventTrigger>();
    public List<NpcBehaviour> npcBehaviours = new List<NpcBehaviour>();
    public List<DoorBehaviour> doorBehaviours = new List<DoorBehaviour>();

	void Awake()
    {
        current = this;

        eventTriggers = GameObject.FindObjectsOfType<EventTrigger>().ToList();
        npcBehaviours = GameObject.FindObjectsOfType<NpcBehaviour>().ToList();
        doorBehaviours = GameObject.FindObjectsOfType<DoorBehaviour>().ToList();
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
        if (eventType == NpcBehaviour.Type.DOOR_PUZZLE)
        {
            OpenDoorPuzzle();
        }
    }

    public void OpenKitchenDoor()
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.FRONT_DOOR);

        // Setup lines for kitchen table and deactivate the NPC at the front door
        foreach (NpcBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.npcType == NpcBehaviour.Type.KITCHEN)
            {
                behaviour.ChangeLines(2, 2);
            }
            else if (behaviour.npcType == NpcBehaviour.Type.FRONT_DOOR)
            {
                behaviour.gameObject.SetActive(false);
            }
        }

        // Enable the front door's collider
        foreach (DoorBehaviour behaviour in doorBehaviours)
        {
            if (behaviour.thisDoorLevel == LevelManager.Levels.KITCHEN)
            {
                behaviour.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        // Deactivate the trigger
        foreach (EventTrigger trigger in eventTriggers)
        {
            if (trigger.eventType == NpcBehaviour.Type.FRONT_DOOR)
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }

    public void InteractWithBelladonna(NpcBehaviour npcBehaviour, bool usingGloves = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.BELLADONNA);

        // If player is using gloves
        if (usingGloves && Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONNA] < 1)
        {
            // Set state to 2
            Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONNA] = 1;
        }
        else if (usingGloves && Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONNA] >= 1)
        {
            npcBehaviour.ChangeLines(4, 4);
            ActivateTextAtLine.current.TalkToNPC(false);
            return;
        }

        value = Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONNA];

        if (!usingGloves)
        {
            value = 0;
        }

        // Default response
        if (value == 0)
        {
            npcBehaviour.ChangeLines(0, 0);
            //Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONA] = 1;
        }
        // If using gloves
        else if (value == 1)
        {
            // Setup correct lines and start talking
            npcBehaviour.ChangeLines(2, 2);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Add nightshade to player's inventory
            Inventory.current.AddItemToInventory(Item.Type.NIGHTSHADE);

            // Set state
            //Game.current.triggeredEvents[NpcBehaviour.Type.BELLADONA] = 2;
        }
        // If spoken once already
        else if (value >= 2)
        {
            //npcBehaviour.ChangeLines(4, 4);
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

        // Default response
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

    public void OpenDoorPuzzle()
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(NpcBehaviour.Type.DOOR_PUZZLE);

        // Deactivate the puzzle NPC
        foreach (NpcBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.npcType == NpcBehaviour.Type.DOOR_PUZZLE)
            {
                behaviour.gameObject.SetActive(false);
            }
        }

        // Enable the door's collider
        foreach (DoorBehaviour behaviour in doorBehaviours)
        {
            if (behaviour.thisDoorLevel == LevelManager.Levels.CAVE_PUZZLE)
            {
                behaviour.GetComponent<BoxCollider2D>().enabled = true;
            }
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
                    PlayerController.current.hud.SetHud(false);
                }
            }
        }
    }
}