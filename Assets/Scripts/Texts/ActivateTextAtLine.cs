﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateTextAtLine : MonoBehaviour
{
    public static ActivateTextAtLine current;

    public string NPCName;

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

    public CharacterBehaviour behaviour;

    public bool destroyWhenActivated;

    void Awake()
    {
        current = this;

        theTextBox = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
        selection = GameObject.Find("Player").transform.FindChild("QuestionMark").gameObject;
    }

    public void TalkToNPC(bool interact = true)
    {
        if (PlayerController.current.canTalkToNPC)
        {
            if (PlayerController.current.overlappingNpc != null)
            {
                behaviour = PlayerController.current.overlappingNpc.GetComponentInChildren<CharacterBehaviour>();

                if (interact)
                {
                    if (behaviour.npcType == CharacterBehaviour.Type.BELLADONNA)
                    {
                        EventManager.current.InteractWithBelladonna(behaviour);
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.DEER)
                    {
                        EventManager.current.InteractWithDeer(behaviour);
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.BEAR)
                    {
                        EventManager.current.InteractWithBear(behaviour);
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.LILIES)
                    {
                        EventManager.current.InteractWithLilies();
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.CHEST)
                    {
                        EventManager.current.InteractWithChest(behaviour);
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.EXIT)
                    {
                        EventManager.current.InteractWithExit(behaviour);
                    }
                    else if (behaviour.npcType == CharacterBehaviour.Type.CANDLE)
                    {
                        EventManager.current.InteractWithCandle(behaviour);
                    }
                }
            }
            else
            {
                behaviour = PlayerController.current.GetComponentInChildren<CharacterBehaviour>();
            }

            PlayerController.current.DeactivateSelection();
            ReloadTextRefScript(behaviour.text, behaviour.buttonsYesNo, behaviour.buttonsOpt,
                behaviour.textStartLine, behaviour.textEndLine);
        }
    }

    public void ReloadTextRefScript(TextAsset currentText,
        int[] currentYesNoButtons, int[] currentOptButtons,
        int npcStartLine, int npcEndLine)
    {
        PlayerController.current.PlayerAnimStop();

        theTextBox.ReloadButtons(Button1, Button2, Button3, Button4,
            Button1G, Button2G, Button3G, Button4G);

        theTextBox.ReloadScript(currentText, currentYesNoButtons, currentOptButtons,
            GetComponent<ActivateTextAtLine>(), npcStartLine, npcEndLine);       

        theTextBox.EnableTextBox();
    }
}
