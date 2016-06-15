using UnityEngine;
using System.Collections;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextBox;

    public bool requireButtonPress;
    private bool waitForPress;

    public bool destroyWhenActivated;

	// Use this for initialization
	void Awake () {
        theTextBox = FindObjectOfType<TextBoxManager>();

	}
	
	// Update is called once per frame
	void Update () {
	    if (waitForPress && Input.GetKeyDown(KeyCode.J))
        {
            theTextBox.ReloadScript(theText);
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

            theTextBox.ReloadScript(theText);
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
