using UnityEngine;
using System.Collections;

public class NpcBehaviour : MonoBehaviour
{
    public Item.type requiredItemType = Item.type.GLOVES;

    public enum Actions
    {
        DEACTIVATE,
        DIALOGUE,
    };
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
    GameFlowManager gameflow;
    Inventory inventory;

    void Awake()
    {
        gameflow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textLoader = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
        inventory = player.GetComponentInChildren<Inventory>();
    }

    void Update()
    {
        if (name == "Intro_NPC")
        {
            if (waitTimer <= 0.0f && isAutomatic && !PlayerController.current.isIntro)
            {
                gameObject.GetComponent<IsIntro>().ContinueIntro();
            }
        }
        else if (name == "NPC_Deer")
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
                EventManager.current.InteractWithDeer(true);
            }
        }
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
