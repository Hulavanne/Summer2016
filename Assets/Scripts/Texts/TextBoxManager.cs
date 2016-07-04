using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To comment this properly, I still have to go through this code so I know how it really works
// ^ I followed a youtube tutorial, so I'm still kind of unsure of what some things do.

public class TextBoxManager : MonoBehaviour {

    public bool isCursorOnActionButton;

    public GameFlowManager gameFlow;

    public static GameObject currentNPC;
    public bool hasClickedYesButton;

    public bool showCurrentYesNoButtons;
    public bool showCurrentOptButtons;

    public GameObject OptTextBox;
    public ActivateTextAtLine textRef;

    public int[] activateYesNoButtonsAtLines;
    public int[] activateOptButtonsAtLines;


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

    GameObject btn1G, btn2G, btn3G, btn4G;

    public bool showYesNoButtons = true;
    public bool showOptButtons = true;

    public bool hasClickedYesNoButton;
    public bool hasClickedOptButton;

    public GameObject textBox;

    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public PlayerController player;

    public bool isActive = true;

    public bool stopPlayerMovement = true;

    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;

    public void OnOptButtonClick()
    {
        OptTextBox.SetActive(false);
        showCurrentOptButtons = false;
        hasClickedOptButton = true;
    }

    public void OnOpt1Click()
    {
        Debug.Log("Clicked Opt 1 Button.");
        OnOptButtonClick();
    }
    public void OnOpt2Click()
    {
        Debug.Log("Clicked Opt 2 Button.");
        OnOptButtonClick();
    }
    public void OnOpt3Click()
    {
        Debug.Log("Clicked Opt 3 Button.");
        OnOptButtonClick();
    }
    public void OnOpt4Click()
    {
        Debug.Log("Clicked Opt 4 Button.");
        OnOptButtonClick();
    }

    public void OnYesClick()
    {
        Debug.Log("Has clicked Yes Button!");
        currentNPC = GameObject.Find(player.NPCName);
        if (currentNPC.GetComponent<Savepoint>() != null)
        {
            currentNPC.GetComponent<Savepoint>().OpenSaveMenu();
        }
        showCurrentYesNoButtons = false;
        hasClickedYesNoButton = true;
    }

    public void OnNoClick()
    {
        Debug.Log("Has clicked No Button!");
        showCurrentYesNoButtons = false;
        hasClickedYesNoButton = true;
    }
    
    public void GetYesNoButtonLines()
    {
        if (showYesNoButtons)
        {
            foreach (int lineNum in activateYesNoButtonsAtLines)
            {
                if (lineNum == currentLine)
                {
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
                    OptTextBox.SetActive(true);
                    showCurrentOptButtons = true;
                }
            }

            OptTextBox.SetActive(true);
        }
    }

    public void CursorOverButton()
    {
        isCursorOnActionButton = true;
    }

    public void CursorOutsideButton()
    {
        isCursorOnActionButton = false;
    }

    void Awake()
    {
        gameFlow = GameObject.Find("GameFlowManager").GetComponent<GameFlowManager>();

        yesButtonG.SetActive(false);
        noButtonG.SetActive(false);
        OptTextBox.SetActive(false);

        player = FindObjectOfType<PlayerController>();

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        
        if (showCurrentYesNoButtons)
        {
            yesButtonG.SetActive(true);
            noButtonG.SetActive(true);
        }

        else if (showCurrentYesNoButtons == false)
        {
            yesButtonG.SetActive(false);
            noButtonG.SetActive(false);
        }

        if (showCurrentOptButtons)
        {
            if (Button1G != null) Button1G.SetActive(true);
            if (Button2G != null) Button2G.SetActive(true);
            if (Button3G != null) Button3G.SetActive(true);
            if (Button4G != null) Button4G.SetActive(true);
        }

        else if (showCurrentOptButtons == false)
        {
            if (Button1G != null) Button1G.SetActive(false);
            if (Button2G != null) Button2G.SetActive(false);
            if (Button3G != null) Button3G.SetActive(false);
            if (Button4G != null) Button4G.SetActive(false);
        }

        if (((Input.GetMouseButtonDown(0) && !showCurrentYesNoButtons) // disable click if there are yes/no buttons
            && (Input.GetMouseButtonDown(0) && !showCurrentOptButtons)) // disable click if there are option buttons
            || hasClickedYesNoButton || hasClickedOptButton ) // proceed if the player clicked a button
        {
            Debug.Log("working!");
            player.talkToNPC = false;
            hasClickedYesNoButton = false;
            hasClickedOptButton = false;
            showCurrentOptButtons = false;
            showCurrentYesNoButtons = false;

            if (showCurrentYesNoButtons == false || showOptButtons == false)
            {
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

    private IEnumerator TextScroll (string lineOfText)
    {
        int letter = 0;
        theText.text = "";

        isTyping = true;
        cancelTyping = false;

        while(isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        
        GetYesNoButtonLines();

        GetOptButtonLines();

        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        if (player.canTalkToNPC)
        {
            OptTextBox.SetActive(true); // activating button text box here

            textBox.SetActive(true);
            isActive = true;

            if (stopPlayerMovement)
            {
                player.canMove = false;
            }
            else
            {
                player.canMove = true;
            }

            #region Insert Variables

            #endregion

            StartCoroutine(TextScroll(textLines[currentLine]));
        }
    }

    public void DisableTextBox()
    {
        OptTextBox.SetActive(false);
        textBox.SetActive(false);
        isActive = false;
        player.canMove = true;
        gameFlow.ChangeNPCBehaviour();
    }

    public void ReloadScript(TextAsset newText, int[] buttonsYesNoAtLines, int[] buttonsOptAtLines,
        ActivateTextAtLine reference, int getCurrentLine, int endLine)
    {
        currentLine = getCurrentLine;
        endAtLine = endLine;

        textFile = newText;
        textRef = reference;

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (newText != null)
        {
            textLines = new string[1];
            textLines = (newText.text.Split('\n'));

            int x = 0;

            //reset numbers

            foreach (int num in activateYesNoButtonsAtLines)
            {
                activateYesNoButtonsAtLines[x] = -1;
                x++;
            }
            x = 0;

            foreach (int num in activateOptButtonsAtLines)
            {
                activateOptButtonsAtLines[x] = -1;
                x++;
            }
            x = 0;

            // cast buttons into this script

            foreach (int num in buttonsYesNoAtLines)
            {
                activateYesNoButtonsAtLines[x] = num;
                x++;
            }
            x = 0;                                                                                                                         
            foreach (int num in buttonsOptAtLines)
            {
                activateOptButtonsAtLines[x] = num;
                x++;
            }
            x = 0;
        }
    }

    public void ReloadButtons (Button btn1, Button btn2, Button btn3, Button btn4,
        GameObject Button1G, GameObject Button2G, GameObject Button3G, GameObject Button4G)
    {
        Button1 = btn1;
        Button2 = btn2;
        Button3 = btn3;
        Button4 = btn4;

        this.Button1G = Button1G;
        this.Button2G = Button2G;
        this.Button3G = Button3G;
        this.Button4G = Button4G;
    }
}