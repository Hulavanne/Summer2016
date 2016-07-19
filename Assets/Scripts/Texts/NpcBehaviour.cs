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
        if (name == "NPC_Deer")
        {
            if (GameObject.Find(name).GetComponent<NpcBehaviour>().behaviour == 3)
            {
                if (LevelManager.current.currentLevel != LevelManager.Levels.FOREST_DEER)
                {
                    GameObject.Find(name).SetActive(false);
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
