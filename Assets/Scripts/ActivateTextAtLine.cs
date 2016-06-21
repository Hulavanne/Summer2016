using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// To comment this properly, I still have to go through this code so I know how it really works
// ^ I followed a youtube tutorial, so I'm still kind of unsure of what some things do.

public class ActivateTextAtLine : MonoBehaviour {

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
        theTextBox = FindObjectOfType<TextBoxManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (waitForPress && Input.GetKeyDown(KeyCode.J))
        {
            
            theTextBox.ReloadScript(theText, activateYesNoButtonsAtLines, activateOptButtonsAtLines);
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            theTextBox.ReloadScript(theText, activateYesNoButtonsAtLines, activateOptButtonsAtLines);
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            if(destroyWhenActivated)
            {
                Destroy(gameObject);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            waitForPress = false;
        }
    }
}
