using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateTextAtLine : MonoBehaviour
{
    public static ActivateTextAtLine current;

    public string NPCName;

    public PlayerController playerController;
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
    public bool waitForPress;

    public GameObject currentNPC;
    public NpcBehaviour npcBehav;

    public bool destroyWhenActivated;

    GameFlowManager npcState;

    void Awake()
    {
        current = this;

        if (GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>() != null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();
        }
        else
        {
            playerController = GameObject.FindGameObjectWithTag("Player").transform.parent.GetComponentInChildren<PlayerController>();
        }

        theTextBox = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
        npcState = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
        selection = GameObject.Find("Player").transform.FindChild("QuestionMark").gameObject;
    }

    void Update()
    {
        /*if (playerController.talkToNPC)
        {
            TalkToNPC();
        }*/
    }

    public void TalkToNPC()
    {
        PlayerController.current.DeactivateSelection();
        PlayerController.current.hasClickedActionButton = false;

        currentNPC = PlayerController.current.overlappingNpc;
        npcBehav = currentNPC.GetComponent<NpcBehaviour>();

        if (npcBehav.transform.name == "NPC_Deer")
        {
            EventManager.current.InteractWithDeer();
        }

        ReloadTextRefScript(npcBehav.text, npcBehav.buttonsYesNo, npcBehav.buttonsOpt,
            npcBehav.textStartLine, npcBehav.textEndLine);

        //playerController.talkToNPC = false;
    }

    public void ReloadTextRefScript(TextAsset currentText,
        int[] currentYesNoButtons, int[] currentOptButtons,
        int npcStartLine, int npcEndLine)
    {
        playerController.PlayerAnimStop();

        theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
            Button1G, Button2G, Button3G, Button4G);

        theTextBox.ReloadScript(currentText, currentYesNoButtons, currentOptButtons,
            GetComponent<ActivateTextAtLine>(), npcStartLine, npcEndLine);       
        
        theTextBox.EnableTextBox();
        Debug.Log(currentNPC);
    }
}
