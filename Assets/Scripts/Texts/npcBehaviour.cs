using UnityEngine;
using System.Collections;

public class NpcBehaviour : MonoBehaviour {

    public PlayerController player;
    public ActivateTextAtLine textLoader;
    public GameFlowManager gameflow;

    public TextAsset text;
    public int[] buttonsYesNo;
    public int[] buttonsOpt;
    public int textStartLine;
    public int textEndLine;
    public string button1Text, button2Text, button3Text, button4Text;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            player.isOverlappingNPC = true;
            player.isSelectionActive = true;
            textLoader.selection.SetActive(true);
            player.selection = PlayerController.Selection.NPC;

            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isWalking", false);

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
            player.isOverlappingNPC = false;
            textLoader.waitForPress = false;
            player.DeactivateSelection();
        }
    }

    void Awake ()
	{
        gameflow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textLoader = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();
	}
}
