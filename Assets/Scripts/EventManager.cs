﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public List<EventTrigger> eventTriggers = new List<EventTrigger>();
    public List<CharacterBehaviour> npcBehaviours = new List<CharacterBehaviour>();
    public List<DoorBehaviour> doorBehaviours = new List<DoorBehaviour>();

    public enum Ending
    {
        NORMAL,
        TRUE
    }
    public static Ending ending = Ending.NORMAL;

	void Awake()
    {
        current = this;

        eventTriggers = GameObject.FindObjectsOfType<EventTrigger>().ToList();
        npcBehaviours = GameObject.FindObjectsOfType<CharacterBehaviour>().ToList();
        doorBehaviours = GameObject.FindObjectsOfType<DoorBehaviour>().ToList();

        // Remove the player's CharacterBehaviour from the npcBehaviours list
        foreach (CharacterBehaviour behaviour in npcBehaviours)
        {
            if (behaviour.transform.parent.tag == "Player")
            {
                npcBehaviours.Remove(behaviour);
                break;
            }
        }
	}

    void Start()
    {
        if (Game.current != null)
        {
            if (!Game.current.newGame)
            {
                CharacterBehaviour.Type[] eventTypes = Game.current.triggeredEvents.Keys.ToArray();

                for (int i = 0; i < eventTypes.Count(); ++i)
                {
                    TriggerEvent(eventTypes[i]);
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
        else if (eventType == CharacterBehaviour.Type.DOOR_PUZZLE)
        {
            InteractWithDoorPuzzle();
        }
        else if (eventType == CharacterBehaviour.Type.ENEMY_CHASE)
        {
            foreach (EnemyBehaviour behaviour in LevelManager.current.enemiesList)
            {
                if (behaviour.thisEnemyLevel == LevelManager.current.currentLevel)
                {
                    behaviour.gameObject.SetActive(true);
                    //behaviour.movementDirection = 
                    break;
                }
            }
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

    public void InteractWithBelladonna(CharacterBehaviour behaviour, bool usingGloves = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.BELLADONNA);

        // If player is using gloves
        if (usingGloves && Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] < 1)
        {
            // Set state to 1
            Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] = 1;
        }
        else if (usingGloves && Game.current.triggeredEvents[CharacterBehaviour.Type.BELLADONNA] >= 1)
        {
            behaviour.ChangeLines(4, 4);
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
            behaviour.ChangeLines(0, 0);
        }
        // If using gloves
        else if (value == 1)
        {
            // Setup correct lines and start talking
            behaviour.ChangeLines(2, 2);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Add nightshade to player's inventory
            Inventory.current.AddItemToInventory(Item.Type.NIGHTSHADE);
        }
    }

    public void InteractWithDeer(CharacterBehaviour behaviour, bool givingBerries = false)
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
            behaviour.ChangeLines(0, 4);
            Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 1;
        }
        // If spoken once already
        else if (value == 1)
        {
            behaviour.ChangeLines(6, 7);
        }
        // State is increased to 3 right after giving berries
        else if (value == 2)
        {
            // Setup correct lines and start talking to the deer
            behaviour.ChangeLines(9, 11);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Add death cap to player's inventory
            Inventory.current.AddItemToInventory(Item.Type.DEATH_CAP);
        }
        // Last state after 
        else if (value >= 3)
        {
            behaviour.ChangeLines(20, 20);
        }
    }

    public void InteractWithBear(CharacterBehaviour behaviour, bool givingDeathCap = false, bool givingBerries = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.BEAR);

        if (givingDeathCap && Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] < 2)
        {
            Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] = 2;
        }

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR];

        if (givingBerries && Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] < 2)
        {
            value = -1;
        }

        // If giving berries
        if (value == -1)
        {
            behaviour.ChangeLines(7, 8);
            ActivateTextAtLine.current.TalkToNPC(false);
        }
        // Default response
        else if (value == 0)
        {
            behaviour.ChangeLines(0, 3);
            Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] = 1;
        }
        // If spoken once already
        else if (value == 1)
        {
            behaviour.ChangeLines(5, 5);
        }
        // If giving death cap
        else if (value == 2)
        {
            behaviour.ChangeLines(10, 10);
            ActivateTextAtLine.current.TalkToNPC(false);
        }
    }

    public void InteractWithLilies()
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.LILIES);

        if (Game.current.triggeredEvents[CharacterBehaviour.Type.LILIES] == 1)
        {
            PlayerController.current.overlappingNpc.GetComponent<CharacterBehaviour>().ChangeLines(4, 4);
        }
    }

    public void EatBerries(int phase)
    {
        Debug.Log("dededed");
        int startLine = 0;
        int endLine = 0;

        if (phase == -1)
        {
            startLine = 10;
            endLine = 10;
        }
        else if (phase == 0)
        {
            startLine = 0;
            endLine = 0;
        }
        else if (phase == 1)
        {
            startLine = 3;
            endLine = 4;
            TextBoxManager.current.hasClickedYesNoButton = false;
        }
        else if (phase == 2)
        {
            CameraEffects.current.fadeToBlack = true;
            startLine = 6;
            endLine = 7;
        }
        else if (phase == 3)
        {
            PlayerController.current.isGameOver = true;
            return;
        }

        SelfDialogue(startLine, endLine);
    }

    public void InteractWithDoorPuzzle(bool openDoor = false)
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.DOOR_PUZZLE);

        if (openDoor || Game.current.triggeredEvents[CharacterBehaviour.Type.DOOR_PUZZLE] == 1)
        {
            Game.current.triggeredEvents[CharacterBehaviour.Type.DOOR_PUZZLE] = 1;

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

            // Deactivate the puzzle
            DoorPuzzle puzzle = FindObjectOfType<DoorPuzzle>();
            puzzle.enabled = false;
            puzzle.gameObject.SetActive(false);
        }

        // Activate the enemy in the crevice
        foreach (EnemyBehaviour behaviour in LevelManager.current.enemiesList)
        {
            if (behaviour.thisEnemyLevel == LevelManager.Levels.CAVE_CREVICE)
            {
                behaviour.gameObject.SetActive(true);
            }
        }
        // Activate the rock infront of the cave entrance
        foreach (Level level in LevelManager.current.levelsList)
        {
            if (level.levelName == LevelManager.Levels.CAVE_ENTRANCE)
            {
                level.transform.FindChild("Objects").GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void InteractWithChest(CharacterBehaviour behaviour, bool usingGloves = false)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.CHEST);

        // If player is using gloves
        if (usingGloves && Game.current.triggeredEvents[CharacterBehaviour.Type.CHEST] < 1)
        {
            // Set state to 1
            Game.current.triggeredEvents[CharacterBehaviour.Type.CHEST] = 1;
        }

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.CHEST];

        // Default response
        if (value == 0)
        {
            behaviour.ChangeLines(0, 1);
        }
        // If using gloves
        else if (value == 1)
        {
            // Setup correct lines and start talking
            behaviour.ChangeLines(3, 4);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Add the money box to player's inventory and remove the gloves
            Inventory.current.AddItemToInventory(Item.Type.MONEY_BOX);

            // Set state
            Game.current.triggeredEvents[CharacterBehaviour.Type.CHEST] = 2;
        }
        // After getting the money box
        else if (value == 2)
        {
            behaviour.ChangeLines(6, 6);
        }
    }

    public void UseSlope()
    {
        // Enable the slope door's collider
        foreach (DoorBehaviour behaviour in doorBehaviours)
        {
            if (behaviour.thisDoorLevel == LevelManager.Levels.HELL_BEDROOM)
            {
                behaviour.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void InteractWithWell(CharacterBehaviour behaviour, Item.Type itemType)
    {
        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.WELL);

        if (itemType == Item.Type.NIGHTSHADE)
        {
            // If nothing has yet been given to the well
            if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 0)
            {
                // Mark the nightshade's leaves as given
                Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] = 1;
            }
            // If the money box has already been given to the well
            else if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 2)
            {
                // Mark them both as given
                Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] = 3;
            }
            // Change in the correct line
            behaviour.ChangeLines(2, 2);
        }
        else if (itemType == Item.Type.MONEY_BOX)
        {
            // If nothing has yet been given to the well
            if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 0)
            {
                // Mark the money box as given
                Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] = 2;
            }
            // If the nightshade's leaves have already been given to the well
            else if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 1)
            {
                // Mark them both as given
                Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] = 3;
            }
            // Change in the correct line
            behaviour.ChangeLines(4, 4);
        }

        // Talk to the well
        ActivateTextAtLine.current.TalkToNPC(false);
    }

    public void InteractWithExit(CharacterBehaviour behaviour)
    {
        int value = 0;

        // Add event to triggeredEvents, if it isn't already there
        Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.EXIT);

        value = Game.current.triggeredEvents[CharacterBehaviour.Type.EXIT];

        if (value == 0)
        {
            Game.current.triggeredEvents[CharacterBehaviour.Type.EXIT] = 1;

            if (ending == Ending.NORMAL)
            {
                behaviour.ChangeLines(0, 2);
            }
            else if (ending == Ending.TRUE)
            {
                behaviour.ChangeLines(6, 13);
            }
        }
        else if (value == 1)
        {
            behaviour.ChangeLines(4, 4);
        }
    }

    public void InteractWithCandle(CharacterBehaviour behaviour)
    {
        if (!Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.EXIT))
        {
            behaviour.ChangeLines(0, 0);
        }
        else
        {
            // Add event to triggeredEvents, if it isn't already there
            Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.CANDLE);
            // Setup the correct lines
            behaviour.ChangeLines(2, 2);
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
                IsIntro intro = npc.GetComponent<IsIntro>();

                if (intro.introPlaying)
                {
                    intro.introPlaying = false;
                    behaviour.ChangeLines(3, 4);
                    behaviour.waitTimer = 1.5f;
                    CameraEffects.current.fadeToBlack = false;
                    PlayerController.current.hud.SetHud(false);
                }
                else
                {
                    intro.DestroyIntro();
                }
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.DEER)
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] == 2)
                {
                    // Setup correct lines and start talking to the deer
                    behaviour.ChangeLines(13, 18);
                    ActivateTextAtLine.current.TalkToNPC(false);

                    // Fade the screen to black and back,
                    // while changing the deer's animation in between the fades
                    StartCoroutine(ScreenFadeEvent(behaviour));
                }
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.BEAR)
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] == 2)
                {
                    // Setup correct lines and start talking to the bear
                    behaviour.ChangeLines(12, 12);
                    ActivateTextAtLine.current.TalkToNPC(false);

                    // Fade the screen to black and back,
                    // while deactivating the bear in between the fades
                    StartCoroutine(ScreenFadeEvent(behaviour));
                }
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.DOOR_PUZZLE)
            {
                InteractWithDoorPuzzle();
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.WELL)
            {
                behaviour.ChangeLines(0, 0);
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.EXIT)
            {
                if (ending == Ending.NORMAL)
                {
                    SoundEffectsManager.AddAudio(SoundEffectsManager.current.gameObject, SoundEffectsManager.current.witchBreathingSound, true, true, 1.0f);
                }
                else if (ending == Ending.TRUE)
                {
                    // Play door opening sound
                    SoundEffectsManager.current.PlaySoundEffect(SoundEffectsManager.current.doorCreakSound, SoundEffectsManager.current.actionSource);
                    SoundEffectsManager.current.actionSource.ignoreListenerPause = true;

                    // End the game
                    PlayerController.current.isGameOver = true;
                    PlayerController.current.hasGameEnded = true;
                }
            }
            else if (behaviour.npcType == CharacterBehaviour.Type.CANDLE &&
                     Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.CANDLE))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 0)
                {
                    behaviour.ChangeLines(3, 3);
                    ActivateTextAtLine.current.TalkToNPC(false);
                    Level.lightSpriteRenderer.sprite = Level.lastLevel.lightsForLastLevel[1];
                    Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] = 1;
                }
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 1)
                {
                    behaviour.ChangeLines(4, 4);
                    ActivateTextAtLine.current.TalkToNPC(false);
                    Level.lightSpriteRenderer.sprite = Level.lastLevel.lightsForLastLevel[3];
                    Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] = 2;
                }
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 2)
                {
                    behaviour.ChangeLines(5, 5);
                    ActivateTextAtLine.current.TalkToNPC(false);
                    Level.lightSpriteRenderer.sprite = Level.lastLevel.lightsForLastLevel[4];
                    Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] = 3;
                }
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 3)
                {
                    behaviour.ChangeLines(6, 6);
                    ActivateTextAtLine.current.TalkToNPC(false);
                    //Level.lightSpriteRenderer.sprite = Level.lastLevel.lightsForLastLevel[4];
                    Level.lightSpriteRenderer.transform.GetChild(0).gameObject.SetActive(true);
                    Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] = 4;
                }
                /*else if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 4)
                {
                    behaviour.ChangeLines(7, 7);
                    ActivateTextAtLine.current.TalkToNPC(false);
                    Level.lightSpriteRenderer.transform.GetChild(0).gameObject.SetActive(true);
                    Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] = 5;
                }*/
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.CANDLE] == 4)
                {
                    PlayerController.current.isGameOver = true;
                    PlayerController.current.hasGameEnded = true;
                }
            }
        }

        TextBoxManager.current.hasClickedYesButton = false;
        TextBoxManager.current.hasClickedNoButton = false;
    }

    public void LevelEnteringEvent(LevelManager.Levels enteredLevel)
    {
        if (enteredLevel == LevelManager.Levels.KITCHEN)
        {
            if (Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.TUTORIAL))
            {
                IntroTutorial.doesShow2 = true;
            }
        }
        else if (enteredLevel == LevelManager.Levels.WEIRD_FOREST_BELLADONNA)
        {
            if (Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.ENEMY_CHASE))
            {
                SelfDialogue(12, 13);
            }
        }
        else if (enteredLevel == LevelManager.Levels.HELL_WELL)
        {
            if (Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.SLOPE))
            {
                SelfDialogue(15, 15);
            }
        }
        else if (enteredLevel == LevelManager.Levels.HELL_CANDLE)
        {
            // Ending with nothing in the well
            if (!Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.WELL))
            {
                ending = Ending.NORMAL;
            }
            else
            {
                // Ending with just nightshade in the well
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 1)
                {
                    ending = Ending.NORMAL;
                }
                // Ending with just the money box in the well
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 2)
                {
                    ending = Ending.NORMAL;
                }
                // Ending with both the items in the well
                else if (Game.current.triggeredEvents[CharacterBehaviour.Type.WELL] == 3)
                {
                    ending = Ending.TRUE;
                }
                else
                {
                    ending = Ending.NORMAL;
                }

                return;
            }
        }
    }

    public void SelfDialogue(int startLine, int endLine)
    {
        PlayerController.current.overlappingNpc = null;
        CharacterBehaviour behaviour = PlayerController.current.GetComponentInChildren<CharacterBehaviour>();

        behaviour.ChangeLines(startLine, endLine);
        behaviour.PlayerSelfDialogue();

        PlayerController.current.isOverlappingNPC = false;
        PlayerController.current.DeactivateSelection();
    }

    public IEnumerator ScreenFadeEvent(CharacterBehaviour behaviour)
    {
        // Start the fading
        CameraEffects.current.FadeToBlackAndBack();

        // Wait until the screen is black
        while (CameraEffects.current.opacity < 1.0f)
        {
            yield return null;
        }

        if (behaviour.npcType == CharacterBehaviour.Type.DEER)
        {
            // Set the deer's state to 3 and setup the new animation
            Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 3;
            behaviour.GetComponent<Animator>().SetBool("hasBerries", true);
        }
        else if (behaviour.npcType == CharacterBehaviour.Type.LILIES)
        {
            // Set the state of the lilies to 1 and berries to the inventory
            Game.current.triggeredEvents[CharacterBehaviour.Type.LILIES] = 1;
            Inventory.current.AddItemToInventory(Item.Type.BERRIES);
        }
        else if (behaviour.npcType == CharacterBehaviour.Type.BEAR)
        {
            // Set the bear's state to 3 and deactivate it
            Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] = 3;
            behaviour.gameObject.SetActive(false);
        }
        else if (behaviour.npcType == CharacterBehaviour.Type.MONEY_BOX)
        {
            // Add event to triggeredEvents, if it isn't already there
            Game.current.AddToTriggeredEvents(CharacterBehaviour.Type.MONEY_BOX);

            // Add mortar and pestle to the inventory and deactivate the object
            Inventory.current.AddItemToInventory(Item.Type.MONEY_BOX);
            behaviour.gameObject.SetActive(false);
        }
    }
}