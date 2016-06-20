using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To comment this properly, I still have to go through this code so I know how it really works
// ^ I followed a youtube tutorial, so I'm still kind of unsure of what some things do.

public class TextBoxManager : MonoBehaviour {

    public GameObject OptTextBox;
    public ActivateTextAtLine textRef;

    public Button yesButton;
    public Button noButton;
    public GameObject yesButtonG;
    public GameObject noButtonG;
    public bool showYesNoButtons = true;
    public bool showOptButtons = false;

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
        hasClickedOptButton = true;
        showOptButtons = false;
    }

    public void OnOpt2Click()
    {
        Debug.Log("Clicked Opt 2 Button.");
        OptTextBox.SetActive(false);
        hasClickedOptButton = true;
        showOptButtons = false;
    }

    public void OnOpt3Click()
    {
        Debug.Log("Clicked Opt 3 Button.");
        OptTextBox.SetActive(false);
        hasClickedOptButton = true;
        showOptButtons = false;
    }

    public void OnOpt4Click()
    {
        Debug.Log("Clicked Opt 4 Button.");
        OptTextBox.SetActive(false);
        hasClickedOptButton = true;
        showOptButtons = false;
    }



    public void onYesClick()
    {
        Debug.Log("Clicked Yes Button.");
        textRef.showYesNoButtons = false;
        hasClickedYesNoButton = true;
        showYesNoButtons = false;
    }

    public void onNoClick()
    {
        Debug.Log("Clicked No Button");
        textRef.showYesNoButtons = false;
        hasClickedYesNoButton = true;
        showYesNoButtons = false;
    }

    public void setYesNoButtons (bool value)
    {
        hasClickedYesNoButton = false;

        if (value)
        {
            textRef.showYesNoButtons = true;
        }
        else
        {
            textRef.showYesNoButtons = false;
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
            Debug.Log("Text not found.");
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

        if (showOptButtons == false)
        {
            OptTextBox.SetActive(false);
        }
        

        // theText.text = textLines[currentLine];

        if ((Input.GetMouseButtonDown(0))||(hasClickedYesNoButton)||(hasClickedOptButton))
        {
            hasClickedYesNoButton = false;
            hasClickedOptButton = false;

           if ((textRef.showYesNoButtons == false)||(showOptButtons == false)) {

                if (!isTyping)
                {

                    currentLine++;

                    // textRef.showYesNoButtons = true;

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

        if (showYesNoButtons)
        {
            textRef.showYesNoButtons = true; // Activating buttons here
        }


        showOptButtons = true;
        if (showOptButtons)
        {
            OptTextBox.SetActive(true);
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
        showOptButtons = false;
        OptTextBox.SetActive(false);

        textBox.SetActive(false);
        isActive = false;

        player.canMove = true;
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (textFile.text.Split('\n'));
        }
    }
}