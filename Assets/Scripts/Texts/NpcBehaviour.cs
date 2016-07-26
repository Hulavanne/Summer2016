using UnityEngine;
using System.Collections;

public class NpcBehaviour : MonoBehaviour
{
    public enum Type
    {
        INTRO,
        BED,
        SHELF,
        BOX,
        FRONT_DOOR,
        KITCHEN,
        PARENTS_BED,
        PARENTS_CLOSET,
        OUTHOUSE,
        BELLADONA,
        DEER,
        BLOCK,
        LILIES
    };

    public enum Actions
    {
        DEACTIVATE,
        DIALOGUE,
    };


    public Type npcType = Type.DEER;
    public Item.Type requiredItemType = Item.Type.GLOVES;
    public Actions action = Actions.DEACTIVATE;

    public TextAsset text;
    public bool isAutomatic;
    public float waitTimer;
    public int[] buttonsYesNo;
    public int[] buttonsOpt;
    public int behaviour;
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
        if (name == "NPC_Deer")
        {
            if (Game.current.triggeredEvents.ContainsKey(EventManager.Events.CHANGE_DEER_STATE))
            {
                if ((Game.current.triggeredEvents[EventManager.Events.CHANGE_DEER_STATE] >= 2 && LevelManager.current.currentLevel != LevelManager.Levels.FOREST_DEER) ||
                    Game.current.triggeredEvents[EventManager.Events.CHANGE_DEER_STATE] == 4)
                {
                    Game.current.triggeredEvents[EventManager.Events.CHANGE_DEER_STATE] = 4;
                    gameObject.SetActive(false);
                    //GameObject.Find(name).SetActive(false);
                }
            }
        }
        else if (name == "NPC_Lilies")
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
        else if (name == "NPC_Block")
        {

        }

    }

    public void TriggerAction()
    {
        if (action == Actions.DEACTIVATE)
        {
            Debug.Log("Deactivated");
            gameObject.SetActive(false);
            player.canMove = true;
        }
        if (action == Actions.DIALOGUE)
        {
            if (name == "NPC_Deer")
            {
                EventManager.current.InteractWithDeer(this, true);
            }
            else if (name == "NPC_Block")
            {
                EventManager.current.InteractWithBlockNPC(this, true);
            }
        }
    }

    public void ChangeLines(int startLine, int endLine)
    {
        PlayerController.current.DeactivateSelection();

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
}
