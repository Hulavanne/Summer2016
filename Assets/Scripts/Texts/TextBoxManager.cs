using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To comment this properly, I still have to go through this code so I know how it really works
// ^ I followed a youtube tutorial, so I'm still kind of unsure of what some things do.

public class TextBoxManager : MonoBehaviour {

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

    public void OnOpt1Click()
    {
        Debug.Log("Clicked Opt 1 Button.");
        OptTextBox.SetActive(false);
        textRef.showOptButtons = false;
        hasClickedOptButton = true;
        // showOptButtons = false;
    }

    public void OnOpt2Click()
    {
        Debug.Log("Clicked Opt 2 Button.");
        OptTextBox.SetActive(false);
        textRef.showOptButtons = false;
        hasClickedOptButton = true;
        // showOptButtons = false;
    }

    public void OnOpt3Click()
    {
        Debug.Log("Clicked Opt 3 Button.");
        OptTextBox.SetActive(false);
        textRef.showOptButtons = false;
        hasClickedOptButton = true;
        // showOptButtons = false;
    }

    public void OnOpt4Click()
    {
        Debug.Log("Clicked Opt 4 Button.");
        OptTextBox.SetActive(false);
        textRef.showOptButtons = false;
        hasClickedOptButton = true;
        // showOptButtons = false;
    }



    public void onYesClick()
    {
        Debug.Log("Clicked Yes Button.");
        textRef.showYesNoButtons = false;
        hasClickedYesNoButton = true;
        // showYesNoButtons = false;
    }

    public void onNoClick()
    {
        Debug.Log("Clicked No Button");
        textRef.showYesNoButtons = false;
        hasClickedYesNoButton = true;
        // showYesNoButtons = false;
    }

    public void setYesNoButtons (bool value)
    {
        hasClickedYesNoButton = false;

        if (value)
        {
            textRef.showYesNoButtons = true;
        }
    }

    public void getYesNoButtonLines()
    {
        if (showYesNoButtons)
        {
            foreach (int lineNum in activateYesNoButtonsAtLines)
            {
                if (lineNum == currentLine)
                {
                    textRef.showYesNoButtons = true;
                }

            }
        }
    }

    public void getOptButtonLines()
    {
        if (showOptButtons)
        {
            foreach (int lineNum in activateOptButtonsAtLines)
            {
                if (lineNum == currentLine)
                {
                    OptTextBox.SetActive(true);
                    textRef.showOptButtons = true;
                }

            }

            OptTextBox.SetActive(true);
        }
    }

    void Awake()
    {

        yesButtonG.SetActive(false);
        noButtonG.SetActive(false);
        OptTextBox.SetActive(false);

        player = FindObjectOfType<PlayerController>();

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        else
        {
            // Debug.Log("Text not found.");
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

        // textLines[1] += currentLine;
        
    }

    void Update()
    {


        if (!isActive)
        {
            return;
        }

        if (textRef.showYesNoButtons)
        {
            yesButtonG.SetActive(true);
            noButtonG.SetActive(true);
        }

        else if (textRef.showYesNoButtons == false)
        {
            yesButtonG.SetActive(false);
            noButtonG.SetActive(false);
        }

        if (textRef.showOptButtons)
        {
            if (Button1G != null) Button1G.SetActive(true);
            if (Button2G != null) Button2G.SetActive(true);
            if (Button3G != null) Button3G.SetActive(true);
            if (Button4G != null) Button4G.SetActive(true);
        }

        else if (textRef.showOptButtons == false)
        {
            if (Button1G != null) Button1G.SetActive(false);
            if (Button2G != null) Button2G.SetActive(false);
            if (Button3G != null) Button3G.SetActive(false);
            if (Button4G != null) Button4G.SetActive(false);
        }

        // theText.text = textLines[currentLine];
        
        if ((((Input.GetMouseButtonDown(0)) && (!textRef.showYesNoButtons)) // disable click if there are yes/no buttons
            && ((Input.GetMouseButtonDown(0)) && (!textRef.showOptButtons))) // disable click if there are option buttons
            || (hasClickedYesNoButton) || (hasClickedOptButton)) // proceed if the player clicked a button
            
        {
            player.talkToNPC = false;
            hasClickedYesNoButton = false;
            hasClickedOptButton = false;

            if ((textRef.showYesNoButtons == false) || (showOptButtons == false))
            {

                if (!isTyping)
                {

                    currentLine++;

                    // textRef.showYesNoButtons = true; // ACTIVATING HERE

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

        // textRef.showYesNoButtons = true;

        getYesNoButtonLines();

        getOptButtonLines();

        
        
        // showOptButtons = true;

        if (showOptButtons)
        {
            // OptTextBox.SetActive(true);
        }

        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
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

        // Insert any variables or extra text at the end of a line HERE

        int apples = 105;

        textLines[1] += apples;

        #endregion
        
        StartCoroutine(TextScroll(textLines[currentLine]));
    }

    public void DisableTextBox()
    {
        // showOptButtons = false;
        OptTextBox.SetActive(false);

        textBox.SetActive(false);
        isActive = false;

        player.canMove = true;
    }

    public void ReloadScript(TextAsset theText, int[] a, int[] b, ActivateTextAtLine reference)
    {
        textRef = reference;

        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));

            int x = 0;
            foreach (int num in a)
            {
                activateYesNoButtonsAtLines[x] = num;
                x++;
            }
            x = 0;                                                                                                                         
            foreach (int num in b)
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