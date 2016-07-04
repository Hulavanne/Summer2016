using UnityEngine;
using System.Collections;

public class npcBehaviour : MonoBehaviour {

    public PlayerController player;
    public ActivateTextAtLine textLoader;
    public GameFlowManager gameflow;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.isOverlappingNPC = true;
            if (other.gameObject.tag == "Player")
            {
                player.isSelectionActive = true;
                textLoader.selection.SetActive(true);
                player.selection = PlayerController.Selection.NPC;
            }

            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isWalking", false);

            if (textLoader.requireButtonPress)
            {
                textLoader.waitForPress = true;
                return;
            }

            if (gameflow.isNPCAutomatic)
            {
                player.TalkToNPC();
                gameflow.isNPCAutomatic = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.isOverlappingNPC = false;
            textLoader.waitForPress = false;
            player.selection = PlayerController.Selection.DEFAULT;
            player.isSelectionActive = false;
            textLoader.selection.SetActive(false);
        }
    }

    void Awake ()
	{
        gameflow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textLoader = GameObject.Find("ActivateText").GetComponent<ActivateTextAtLine>();

        if (gameObject.name == "NPC_1")
        {
            gameflow.isNPCAutomatic = true;
        }
	}
	
	void Update ()
	{
	
	}
}
