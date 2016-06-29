using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateTextAtLine : MonoBehaviour {

    public string NPCName;

    public PlayerController player;
    public GameObject selection;

    public bool showYesNoButtons;
    public bool showOptButtons;

    public Button yesButton;
    public Button noButton;
    public GameObject yesButtonG;
    public GameObject noButtonG;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;

    public GameObject Button1G;
    public GameObject Button2G;
    public GameObject Button3G;
    public GameObject Button4G;

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextBox;

    public bool requireButtonPress;
    private bool waitForPress;

    public bool destroyWhenActivated;
    public TextList textnumber;
    
	void Update ()
    {
	    if (player.talkToNPC)
        {
            if (player.NPCName == "NPC_1") ReloadTextRefScript(textnumber.Text1, textnumber.buttonsYesNo1, textnumber.buttonsOpt1);
            if (player.NPCName == "NPC_2") ReloadTextRefScript(textnumber.Text2, textnumber.buttonsYesNo2, textnumber.buttonsOpt2);
            if (player.NPCName == "NPC_3") ReloadTextRefScript(textnumber.Text3, textnumber.buttonsYesNo3, textnumber.buttonsOpt3);

            player.talkToNPC = false;
        }
	}

    public void ReloadTextRefScript(TextAsset currentText,
        int[] currentYesNoButtons, int[] currentOptButtons)
    {
        player.talkToNPC = false;
        player.playerAnim.SetBool("isIdle", true);
        player.playerAnim.SetBool("isWalking", false);

        theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
            Button1G, Button2G, Button3G, Button4G);

        theTextBox.ReloadScript(currentText, currentYesNoButtons, currentOptButtons,
            GetComponent<ActivateTextAtLine>());       

        theTextBox.currentLine = startLine;
        theTextBox.endAtLine = endLine;
        theTextBox.EnableTextBox();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            player.isOverlappingNPC = true;
            if (other.gameObject.tag == "Player")
            {
                player.isSelectionActive = true;
                selection.SetActive(true);
                player.selection = PlayerController.Selection.NPC;
            }

            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isWalking", false);

            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            player.isOverlappingNPC = false;
            waitForPress = false;
            player.selection = PlayerController.Selection.DEFAULT;
            player.isSelectionActive = false;
            selection.SetActive(false);
        }
    }
}
