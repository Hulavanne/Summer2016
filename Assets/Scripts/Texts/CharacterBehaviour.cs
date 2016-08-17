using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        BEAR,
        LILIES,
        DOOR_PUZZLE
    };
    public Type npcType = Type.PASSIVE;
    public List<Item.Type> requiredItemTypes = new List<Item.Type>();

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

    void Start()
    {
        if (npcType == Type.DEER)
        {
            if (Game.current != null && Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.DEER))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 2)
                {
                    // Setup the new animation
                    transform.GetComponent<Animator>().SetBool("hasBerries", true);
                }
            }
        }
    }

    void Update()
    {
        if (npcType == Type.DEER)
        {
            if (Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.DEER))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 2)
                {
                    // Activate the enemy in the forest
                    foreach (EnemyBehaviour enemy in LevelManager.current.enemiesList)
                    {
                        if (enemy.thisEnemyLevel == LevelManager.Levels.FOREST_ENEMY)
                        {
                            enemy.gameObject.SetActive(true);
                        }
                    }

                    if (PlayerController.current.switchingLevel || Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] >= 4)
                    {
                        Game.current.triggeredEvents[CharacterBehaviour.Type.DEER] = 4;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (npcType == Type.BEAR)
        {
            if (Game.current.triggeredEvents.ContainsKey(CharacterBehaviour.Type.BEAR))
            {
                if (Game.current.triggeredEvents[CharacterBehaviour.Type.BEAR] >= 2)
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

    public void TriggerAction(Item.Type itemType)
    {
        if (npcType == Type.BELLADONNA)
        {
            EventManager.current.InteractWithBelladonna(this, true);
        }
        else if (npcType == Type.DEER)
        {
            EventManager.current.InteractWithDeer(this, true);
        }
        else if (npcType == Type.BEAR)
        {
            if (itemType == Item.Type.DEATH_CAP)
            {
                EventManager.current.InteractWithBear(this, true, false);
            }
            else if (itemType == Item.Type.BERRIES)
            {
                EventManager.current.InteractWithBear(this, false, true);
            }
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
            if (requiredItemTypes.Contains(inventory.items[i].itemType))
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
