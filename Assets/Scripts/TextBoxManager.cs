using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

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

    void Awake()
    {
        

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

        // theText.text = textLines[currentLine];

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(!isTyping)
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

        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {

        textBox.SetActive(true);
        isActive = true;

        if (stopPlayerMovement)
        {
            // Debug.Log("working");
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

