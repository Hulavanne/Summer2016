using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To comment this properly, I still have to go through this code so I know how it really works
// ^ I followed a youtube tutorial, so I'm still kind of unsure of what some things do.

public class ActivateTextAtLine : MonoBehaviour {

    public PlayerController player;
    public GameObject selection;

    public int[] activateYesNoButtonsAtLines;
    public int[] activateOptButtonsAtLines;

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

	// Use this for initialization
	void Awake ()
    {
        // theTextBox = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (player.talkToNPC)
        {
            Debug.Log("talktoNPC working");

            player.talkToNPC = false;
            player.playerAnim.SetBool("isIdle", true);
            player.playerAnim.SetBool("isWalking", false);

            theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
                Button1G, Button2G, Button3G, Button4G);
            theTextBox.ReloadScript(theText, activateYesNoButtonsAtLines, activateOptButtonsAtLines,
                GetComponent<ActivateTextAtLine>());
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

        }
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (player.talkToNPC)
        {
            /*
            Debug.Log("talktoNPC working");
            theTextBox.ReloadScript(theText, activateYesNoButtonsAtLines, activateOptButtonsAtLines,
                GetComponent<ActivateTextAtLine>());
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
                Button1G, Button2G, Button3G, Button4G);
                */
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
