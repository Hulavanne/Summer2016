using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateTextAtLine : MonoBehaviour {

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

    public bool destroyWhenActivated;
    public TextList textNumber;

    GameFlowManager npcState;

    void Awake()
    {
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        theTextBox = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();
        textNumber = GameObject.Find("TextList").GetComponent<TextList>();
        npcState = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();
    }

	void Update ()
    {
	    if (playerController.talkToNPC)
        {
            if ((playerController.NPCName == "Intro_NPC") && (npcState.npcIntroBehav == 0)) ReloadTextRefScript(textNumber.Text1,
                textNumber.buttonsYesNo1, textNumber.buttonsOpt1, textNumber.textIntroStartLine, textNumber.textIntroEndLine);

            if ((playerController.NPCName == "Intro_NPC") && (npcState.npcIntroBehav == 1)) ReloadTextRefScript(textNumber.Text1,
               textNumber.buttonsYesNo1, textNumber.buttonsOpt1, textNumber.textIntroStartLine, textNumber.textIntroEndLine);

            if ((playerController.NPCName == "NPC_1") && (npcState.npc1Behav == 0)) ReloadTextRefScript(textNumber.Text1,
                textNumber.buttonsYesNo1, textNumber.buttonsOpt1, textNumber.text1StartLine, textNumber.text1EndLine);

            if ((playerController.NPCName == "NPC_1") && (npcState.npc1Behav == 1)) ReloadTextRefScript(textNumber.Text1,
               textNumber.buttonsYesNo1, textNumber.buttonsOpt1, textNumber.text1StartLine, textNumber.text1EndLine);

            if (playerController.NPCName == "NPC_2") ReloadTextRefScript(textNumber.Text2, textNumber.buttonsYesNo2, textNumber.buttonsOpt2,
                textNumber.text2StartLine, textNumber.text2EndLine);

            if (playerController.NPCName == "NPC_3") ReloadTextRefScript(textNumber.Text3, textNumber.buttonsYesNo3, textNumber.buttonsOpt3,
                textNumber.text3StartLine, textNumber.text3EndLine);

            if (playerController.NPCName == "Savepoint") ReloadTextRefScript(textNumber.SaveText, textNumber.buttonsYesNoSave, textNumber.buttonsOptSave,
                textNumber.textSavStartLine, textNumber.textSavEndLine);

            playerController.talkToNPC = false;
        }
	}

    public void ReloadTextRefScript(TextAsset currentText,
        int[] currentYesNoButtons, int[] currentOptButtons,
        int npcStartLine, int npcEndLine)
    {
        playerController.talkToNPC = false;
        playerController.playerAnim.SetBool("isIdle", true);
        playerController.playerAnim.SetBool("isWalking", false);

        theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
            Button1G, Button2G, Button3G, Button4G);

        theTextBox.ReloadScript(currentText, currentYesNoButtons, currentOptButtons,
            GetComponent<ActivateTextAtLine>(), npcStartLine, npcEndLine);       
        
        theTextBox.EnableTextBox();
    }
}
