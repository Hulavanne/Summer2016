﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
public class TextBoxManager : MonoBehaviour
{
    public bool clickException;
    public static TextBoxManager current;

    public static GameObject currentNPC;
    public bool isTalkingToNPC;

    public bool showCurrentYesNoButtons;
    public bool showCurrentOptButtons;
    public bool buttonClickException;

    public GameObject OptTextBox;
    public ActivateTextAtLine textRef;

    public int[] activateYesNoButtonsAtLines;
    public int[] activateOptButtonsAtLines;

    public Button yesButton;
    public Button noButton;
    public GameObject yesButtonG;
    public GameObject noButtonG;

    Button Button1;
    Button Button2;
    Button Button3;
    Button Button4;

    public GameObject Button1G;
    public GameObject Button2G;
    public GameObject Button3G;
    public GameObject Button4G;

    GameObject btn1G, btn2G, btn3G, btn4G;

    public bool showYesNoButtons = true;
    public bool showOptButtons = true;

    public bool hasClickedYesNoButton;
    public bool hasClickedYesButton;
    public bool hasClickedNoButton;
    public bool hasClickedOptButton;

    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public bool isActive = true;

    public bool stopPlayerMovement = true;

    private bool isTyping = false;
    public bool cancelTyping = false;

    public bool canType;
    public float typeTimer;
    public float typeSpeed;
    public bool isDialogueActive;

    void Awake()
    {
        current = this;
        DisableAllButtons();
    }

    void Start()
    {
        clickException = false;
    }
    
    void Update()
    {
        typeTimer += 2 * Time.deltaTime;
        if (typeTimer > 0.2 && !canType)
        {
            canType = true;
            typeTimer = 0;
        }

        if (isActive)
        {
            SetCurrentYesNoButtons(); // sets active or not active
            SetCurrentOptButtons(); // sets active or not active

            if ((Input.GetMouseButtonDown(0) && !showCurrentYesNoButtons && !showCurrentOptButtons) ||
                hasClickedYesNoButton || hasClickedOptButton)
            {
                if (PlayerController.current.touchRun.selectionTouchException)
                {
                    PlayerController.current.touchRun.selectionTouchException = false;
                }

                else
                {
                    // executes every click without buttons, or every successful button press
                    //player.talkToNPC = false;
                    hasClickedYesNoButton = false;
                    hasClickedOptButton = false;

                    if (!isTyping)
                    {
                        currentLine++;

                        if (currentLine > endAtLine)
                        {
                            DisableTextBox();
                        }
                        else
                        {
                            StartCoroutine(TextScroll(textLines[currentLine]));
                        }
                    }
                    else if (isTyping && !cancelTyping)
                    {
                        cancelTyping = true;
                    }
                }
            }
        }
    }
    
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0; // int for the number of letters
        theText.text = "";

        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter]; // adds one letter to the textBox
            letter += 1; // adds 1 to letter variable
            yield return new WaitForSeconds(typeSpeed); // waits for an amount of time
        }
        // the loop ends if the line has reached its end

        GetYesNoButtonLines(); // checks the display of buttons
        GetOptButtonLines(); // checks the display of buttons

        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void OnOptButtonClick()
    {
        OptTextBox.SetActive(false);
        showCurrentOptButtons = false;
        hasClickedOptButton = true;
    }

    public void OnOpt1Click()
    {
        buttonClickException = true;
        // > check current NPC
        // > do something specific
        OnOptButtonClick();
    }
    public void OnOpt2Click()
    {
        buttonClickException = true;
        // > check current NPC
        // > do something specific
        OnOptButtonClick();
    }
    public void OnOpt3Click()
    {
        buttonClickException = true;
        // > check current NPC
        // > do something specific
        OnOptButtonClick();
    }
    public void OnOpt4Click()
    {
        buttonClickException = true;
        // > check current NPC
        // > do something specific
        OnOptButtonClick();
    }

    public void OnYesClick()
    {
        buttonClickException = true;
        currentNPC = PlayerController.current.overlappingNpc;

        showCurrentYesNoButtons = false;
        hasClickedYesNoButton = true;
        hasClickedYesButton = true;

        // Player self dialogue
        if (currentNPC == null)
        {
            EventManager.current.EatBerries(1);
            return;
        }

        // Savepoint interaction
        if (currentNPC.GetComponent<Savepoint>() != null)
        {
            currentNPC.GetComponent<Savepoint>().OpenSaveMenu();
            return;
        }

        CharacterBehaviour behaviour = currentNPC.GetComponent<CharacterBehaviour>();
        CharacterBehaviour.Type npcType = behaviour.npcType;

        // Door puzzle interaction
        if (npcType == CharacterBehaviour.Type.DOOR_PUZZLE)
        {
            // Activate the puzzle
            DoorPuzzle.current.Activate(true);
        }
        // Door puzzle interaction
        else if (npcType == CharacterBehaviour.Type.LILIES)
        {
            // Setup the next line
            behaviour.ChangeLines(2, 2);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Fade screen to black and back, while adding berries to the inventory
            StartCoroutine(EventManager.current.ScreenFadeEvent(behaviour));
        }
        // Money box interaction
        else if (npcType == CharacterBehaviour.Type.MONEY_BOX)
        {
            // Setup the next line
            behaviour.ChangeLines(2, 2);
            ActivateTextAtLine.current.TalkToNPC(false);

            // Fade screen to black and back, while adding mortar and pestle to the inventory and deactivating the object
            StartCoroutine(EventManager.current.ScreenFadeEvent(behaviour));
        }
        else if (npcType == CharacterBehaviour.Type.SLOPE)
        {
            currentNPC.SetActive(false);
            currentNPC = null;
            EventManager.current.UseSlope();
        }
        else
        {
            return;
        }

        hasClickedYesNoButton = false;
        hasClickedOptButton = false;
        DisableTextBox();
    }

    public void OnNoClick()
    {
        showCurrentYesNoButtons = false;
        hasClickedYesNoButton = true;
        hasClickedNoButton = true;
        buttonClickException = true;
    }
    
    public void GetYesNoButtonLines()
    {
        if (showYesNoButtons)
        {
            foreach (int lineNum in activateYesNoButtonsAtLines)
            {
                if (lineNum == currentLine)
                {
                    // checks if there are yes/no buttons to be shown in current dialogue
                    showCurrentYesNoButtons = true;
                }
            }
        }
    }

    public void GetOptButtonLines()
    {
        if (showOptButtons)
        {
            foreach (int lineNum in activateOptButtonsAtLines)
            {
                if (lineNum == currentLine)
                {
                    // checks if there are option buttons to be shown in current dialogue
                    OptTextBox.SetActive(true);
                    showCurrentOptButtons = true;
                }
            }

            OptTextBox.SetActive(true);
        }
    }
    
    void SetCurrentOptButtons()
    {
        if (Button1G != null) Button1G.SetActive(showCurrentOptButtons);
        if (Button2G != null) Button2G.SetActive(showCurrentOptButtons);
        if (Button3G != null) Button3G.SetActive(showCurrentOptButtons);
        if (Button4G != null) Button4G.SetActive(showCurrentOptButtons);
    }

    void SetCurrentYesNoButtons()
    {
        yesButtonG.SetActive(showCurrentYesNoButtons);
        noButtonG.SetActive(showCurrentYesNoButtons);
    }

    public void EnableTextBox() // activates all text output, disables player movement
    {
        OptTextBox.SetActive(true);
        textBox.SetActive(true);
        isActive = true;
        SetCurrentYesNoButtons();
        SetCurrentOptButtons();
        PlayerController.current.canMove = false;
        isTalkingToNPC = true;
        StartCoroutine(TextScroll(textLines[currentLine]));
        PlayerController.current.hud.SetHud(false);
    }

    public void DisableTextBox(bool endDialogue = true) // deactivates all text output, enables player movement
    {
        if (buttonClickException)
        {
            buttonClickException = false;
            clickException = false;
        }
        else
        {
            clickException = true;
        }

        OptTextBox.SetActive(false);
        textBox.SetActive(false);
        isActive = false;
        isTalkingToNPC = false;

        if (PlayerController.current != null)
        {
            PlayerController.current.canMove = true;

            if (IntroTutorial.isTutorialActive)
            {
                PlayerController.current.hud.SetHud(false);
            }
            else
            {
                PlayerController.current.hud.SetHud(true);
            }

            if (endDialogue)
            {
                EventManager.current.NpcDialogueFinished(PlayerController.current.overlappingNpc); // changes something everytime you finish talking to a NPC
            }
        }
    }

    public void ResetButtons()
    {
        int x = 0;
        
        foreach (int num in activateYesNoButtonsAtLines)
        {
            activateYesNoButtonsAtLines[x] = -1; // resetting buttons
            x++;
        }
        x = 0;

        foreach (int num in activateOptButtonsAtLines)
        {
            activateOptButtonsAtLines[x] = -1;  // resetting buttons
            x++;
        }
        x = 0;
    }

    void DisableAllButtons()
    {
        yesButtonG.SetActive(false);
        noButtonG.SetActive(false);
        OptTextBox.SetActive(false);
    }

    public void ReloadScript(TextAsset newText, int[] buttonsYesNoAtLines, int[] buttonsOptAtLines,
        ActivateTextAtLine reference, int getCurrentLine, int endLine)
    {
        currentLine = getCurrentLine; // current line in text
        endAtLine = endLine; // current line you want to finish the text

        textFile = newText;
        textRef = reference; // gets reference of ActivateTextAtLine component

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n')); // splits the text every endLine
        }

        if (newText != null)
        {
            textLines = new string[1]; // checks if the next letter after current is an endLine
            textLines = (newText.text.Split('\n')); // ands splits it if it is

            ResetButtons();

            int x = 0;

            foreach (int num in buttonsYesNoAtLines)
            {
                activateYesNoButtonsAtLines[x] = num; // casting yes no buttons into this script if they exist
                x++;
            }
            x = 0;                                                                                                                         
            foreach (int num in buttonsOptAtLines)
            {
                activateOptButtonsAtLines[x] = num; // casting opt buttons into this script if they exist
                x++;
            }
            x = 0;
        }
    }

    public void ReloadButtons (Button btn1, Button btn2, Button btn3, Button btn4,
        GameObject Button1G, GameObject Button2G, GameObject Button3G, GameObject Button4G /*,
        string button1Text, string button2Text, string button3Text, string button4Text*/)
    {
        /*
        Button1G.transform.FindChild("Text").GetComponent<Text>().text = button1Text;
        Button2G.transform.FindChild("Text").GetComponent<Text>().text = button2Text;
        Button3G.transform.FindChild("Text").GetComponent<Text>().text = button3Text;
        Button4G.transform.FindChild("Text").GetComponent<Text>().text = button4Text;
        */
        Button1 = Button1G.GetComponent<Button>();
        Button2 = Button2G.GetComponent<Button>();
        Button3 = Button3G.GetComponent<Button>();
        Button4 = Button4G.GetComponent<Button>();

        this.Button1G = Button1G;
        this.Button2G = Button2G;
        this.Button3G = Button3G;
        this.Button4G = Button4G;
    }
}