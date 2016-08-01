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
    public NpcBehaviour npcBehaviour;

    public bool destroyWhenActivated;

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
        selection = GameObject.Find("Player").transform.FindChild("QuestionMark").gameObject;
    }

    public void TalkToNPC(bool interact = true)
    {
        PlayerController.current.DeactivateSelection();

        currentNPC = PlayerController.current.overlappingNpc;
        npcBehaviour = currentNPC.GetComponent<NpcBehaviour>();

        if (interact)
        {
            if (npcBehaviour.npcType == NpcBehaviour.Type.BELLADONNA)
            {
                EventManager.current.InteractWithBelladonna(npcBehaviour);
            }
            if (npcBehaviour.npcType == NpcBehaviour.Type.DEER)
            {
                EventManager.current.InteractWithDeer(npcBehaviour);
            }
            if (npcBehaviour.npcType == NpcBehaviour.Type.BLOCKER)
            {
                EventManager.current.InteractWithBlocker(npcBehaviour);
            }
            if (npcBehaviour.npcType == NpcBehaviour.Type.LILIES)
            {
                EventManager.current.InteractWithLilies();
            }
        }

        ReloadTextRefScript(npcBehaviour.text, npcBehaviour.buttonsYesNo, npcBehaviour.buttonsOpt,
            npcBehaviour.textStartLine, npcBehaviour.textEndLine);
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
    }
}
