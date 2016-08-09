using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public List<EventTrigger> eventTriggers = new List<EventTrigger>();
    public List<CharacterBehaviour> npcBehaviours = new List<CharacterBehaviour>();
    public List<DoorBehaviour> doorBehaviours = new List<DoorBehaviour>();

	void Awake()
    {
        current = this;

        eventTriggers = GameObject.FindObjectsOfType<EventTrigger>().ToList();
        npcBehaviours = GameObject.FindObjectsOfType<CharacterBehaviour>().ToList();
        doorBehaviours = GameObject.FindObjectsOfType<DoorBehaviour>().ToList();
	}

    void Start()
    {
        if (Game.current != null)
        {
            if (!Game.current.newGame)
            {
                foreach (CharacterBehaviour.Type triggeredEvent in Game.current.triggeredEvents.Keys)
                {
                    TriggerEvent(triggeredEvent);
                }
            }
        }
    }

    public void TriggerEvent(CharacterBehaviour.Type eventType)
    {
        if (eventType == CharacterBehaviour.Type.FRONT_DOOR)
        {
            OpenKitchenDoor();
        }
        if (eventType == CharacterBehaviour.Type.DOOR_PUZZLE)
        {
            OpenDoorPuzzle();
        }
    }

    public void OpenKitchenDoor()
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.FRONT_DOOR);

        // Setup lines for kitchen table and deactivate the NPC at the front door
        foreach (CharacterBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.npcType == CharacterBehaviour.Type.KITCHEN)
            {
                behaviour.ChangeLines(2, 2);
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.FRONT_DOOR)
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
            if (trigger.eventType == CharacterBehaviour.Type.FRONT_DOOR)
            {
                trigger.gameObject.SetActive(false);
            }
        }
    }

    public void InteractWithBelladonna(CharacterBehaviour npcBehaviour, bool usingGloves = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.BELLADONNA);

        // If player is using gloves
        if (usingGloves && Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] < 1)
        {
            // Set state to 2
            Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] = 1;
        }
        else if (usingGloves && Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] >= 1)
        {
            npcBehaviour.ChangeLines(4, 4);
            ActivateTextAtLine.current.TalkToNPC(false);
            return;
        }

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA];

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

    public void InteractWithDeer(CharacterBehaviour npcBehaviour, bool givingBerries = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.DEER);

        // If player is giving berries
        if (givingBerries && Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] < 2)
        {
            // Set deer's state to 2
            Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 2;
        }

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.DEER];

        // Default response
        if (value == 0)
        {
            npcBehaviour.ChangeLines(0, 4);
            Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 1;
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
            Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 3;
        }
        // Last state after 
        else if (value >= 3)
        {
            npcBehaviour.ChangeLines(13, 13);
        }
    }

    public void InteractWithBlocker(CharacterBehaviour npcBehaviour, bool givingDeathCap = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.BLOCKER);

        if (givingDeathCap && Game.current.triggeredEvents[CharacterBehaviour.Type.BLOCKER] < 1)
        {
            Game.current.triggeredEvents[CharacterBehaviour.Type.BLOCKER] = 1;
        }

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.BLOCKER];

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
            Game.current.triggeredEvents[CharacterBehaviour.Type.BLOCKER] = 2;
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
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.LILIES);
        value = Game.current.triggeredEvents[CharacterBehaviour.Type.LILIES];

        if (value == 0)
        {
            Game.current.triggeredEvents[CharacterBehaviour.Type.LILIES] = 1;
            Inventory.current.AddItemToInventory(Item.Type.BERRIES);
        }
        else
        {
            Debug.Log(PlayerController.current.overlappingNpc);
            PlayerController.current.overlappingNpc.GetComponent<CharacterBehaviour>().ChangeLines(1, 1);
        }
    }

    public void EatBerries(int phase)
    {
        CharacterBehaviour behaviour = PlayerController.current.GetComponentInChildren<CharacterBehaviour>();

        if (phase == -1)
        {
            behaviour.ChangeLines(10, 10);
        }
        else if (phase == 0)
        {
            behaviour.ChangeLines(0, 0);
        }
        else if (phase == 1)
        {
            behaviour.ChangeLines(3, 4);
        }
        else if (phase == 2)
        {
            CameraEffects.current.fadeToBlack = true;
            behaviour.ChangeLines(6, 7);
        }
        else if (phase == 3)
        {
            PlayerController.current.isGameOver = true;
            return;
        }

        behaviour.PlayerSelfDialogue();

        PlayerController.current.overlappingNpc = null;
        PlayerController.current.isOverlappingNPC = false;
        PlayerController.current.DeactivateSelection();
    }

    public void OpenDoorPuzzle()
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.DOOR_PUZZLE);

        // Deactivate the puzzle NPC
        foreach (CharacterBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.npcType == CharacterBehaviour.Type.DOOR_PUZZLE)
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
        if (npc == null)
        {
            CharacterBehaviour behaviour = PlayerController.current.GetComponentInChildren<CharacterBehaviour>();

            if (behaviour.isAutomatic)
            {
                EatBerries(3);
            }
            if (TextBoxManager.current.hasClickedYesButton)
            {
                behaviour.isAutomatic = true;
                EatBerries(2);
            }
            if (TextBoxManager.current.hasClickedNoButton)
            {
                EatBerries(-1);
            }
        }
        else
        {
            CharacterBehaviour behaviour = npc.GetComponent<CharacterBehaviour>();

            if (behaviour.npcType == CharacterBehaviour.Type.INTRO)
            {
                if (npc.GetComponent<IsIntro>().introPlaying)
                {
                    npc.GetComponent<IsIntro>().introPlaying = false;
                    behaviour.ChangeLines(3, 4);
                    behaviour.waitTimer = 1.5f;
                    CameraEffects.current.fadeToBlack = false;
                    PlayerController.current.hud.SetHud(false);
                }
            }
        }

        TextBoxManager.current.hasClickedYesButton = false;
        TextBoxManager.current.hasClickedNoButton = false;
    }
}