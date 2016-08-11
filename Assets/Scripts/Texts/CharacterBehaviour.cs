﻿using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour
{
    public enum Type
    {
        PASSIVE,
        INTRO,
        BED,
        SHELF,
        BOX,
        FRONT_DOOR,
        KITCHEN,
        PARENTS_BED,
        PARENTS_CLOSET,
        OUTHOUSE,
        BELLADONNA,
        DEER,
        BLOCKER,
        LILIES,
        DOOR_PUZZLE
    };
    public Type npcType = Type.PASSIVE;
    public Item.Type requiredItemType = Item.Type.NONE;

    public TextAsset text;
    public bool isAutomatic;
    public float waitTimer;
    public int[] buttonsYesNo;
    public int[] buttonsOpt;
    public int textStartLine;
    public int textEndLine;
    public string button1Text, button2Text, button3Text, button4Text;

    PlayerController player;
    ActivateTextAtLine textLoader;
    Inventory inventory;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textLoader = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        inventory = player.GetComponentInChildren<Inventory>();
    }

    void Update()
    {
        if (npcType == Type.DEER)
        {
            if (Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.DEER))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 2)
                {
                    // Setup new animation
                    transform.GetComponent<Animator>().SetBool("hasBerries", true);
                }
                if ((Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 2 && LevelManager.current.currentLevel != LevelManager.Levels.FOREST_DEER) ||
                    Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] == 4)
                {
                    Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 4;
                    gameObject.SetActive(false);
                    //GameObject.Find(name).SetActive(false);
                }
            }
        }
        else if (npcType == Type.BLOCKER)
        {
            if (Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.BLOCKER))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.BLOCKER] >= 2)
                {
                    transform.FindChild("PlayerBoundary").gameObject.SetActive(false);
                }
            }
        }
        else if (npcType == Type.LILIES)
        {
        /*
            if (Game.current.triggeredEvents.ContainsKey(EventManager.Events.CHANGE_LILIES_STATE))
            {
                Debug.Log("working1");
                if ((Game.current.triggeredEvents[EventManager.Events.CHANGE_LILIES_STATE] >= 2 && LevelManager.current.currentLevel != LevelManager.Levels.FOREST_LILIES) ||
                    Game.current.triggeredEvents[EventManager.Events.CHANGE_LILIES_STATE] == 4)
                {
                    Debug.Log("working2");
                    Game.current.triggeredEvents[EventManager.Events.CHANGE_LILIES_STATE] = 4;
                    gameObject.SetActive(false);
                    //GameObject.Find(name).SetActive(false);
                }
            }
        */
        }
    }

    public void TriggerAction()
    {
        if (npcType == Type.BELLADONNA)
        {
            EventManager.current.InteractWithBelladonna(this, true);
        }
        else if (npcType == Type.DEER)
        {
            EventManager.current.InteractWithDeer(this, true);
        }
        else if (npcType == Type.BLOCKER)
        {
            EventManager.current.InteractWithBlocker(this, true);
        }
    }

    public void ChangeLines(int startLine, int endLine)
    {
        textStartLine = startLine;
        textEndLine = endLine;
    }

    public void SetItemsUsability(bool usable)
    {
        for (int i = 0; i < inventory.items.Count; ++i)
        {
            if (inventory.items[i].itemType == requiredItemType)
            {
                inventory.items[i].usable = usable;
            }
        }
    }

    public void PlayerSelfDialogue()
    {
        PlayerController.current.canMove = false;
        PlayerController.current.isOverlappingNPC = true;
        PlayerController.current.canTalkToNPC = true;

        PlayerController.current.ActivateSelection(PlayerController.Selection.NPC);
        PlayerController.current.PlayerAnimStop();

        ActivateTextAtLine.current.TalkToNPC();
    }
}