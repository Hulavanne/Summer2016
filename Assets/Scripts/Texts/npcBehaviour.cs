using UnityEngine;
using System.Collections;

public class NpcBehaviour : MonoBehaviour
{
    public Item.type requiredItemType = Item.type.GLOVES;

    public enum actionType
    {
        DEACTIVATE
    };
    public actionType action = actionType.DEACTIVATE;

    public TextAsset text;
    public int[] buttonsYesNo;
    public int[] buttonsOpt;
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            player.isOverlappingNPC = true;
            player.ActivateSelection(PlayerController.Selection.NPC);

            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isWalking", false);

            SetItemsUsability(true);

            if (textLoader.requireButtonPress)
            {
                textLoader.waitForPress = true;
                return;
            }

            if ((gameflow.isNPCAutomatic) && (player.npcWaitTime <= 0.0f))
            {
                player.TalkToNPC();
                gameflow.isNPCAutomatic = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            SetItemsUsability(false);
            player.isOverlappingNPC = false;
            textLoader.waitForPress = false;
            player.DeactivateSelection();
        }
    }

    public void TriggerAction()
    {
        if (action == actionType.DEACTIVATE)
        {
            Debug.Log("Deactivated");
            gameObject.SetActive(false);
        }
    }

    void SetItemsUsability(bool usable)
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
