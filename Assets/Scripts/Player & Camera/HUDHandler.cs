using UnityEngine;
using System.Collections;

public class HUDHandler : MonoBehaviour {

    public GameObject questionMark;
    public GameObject gameOverObj;
    //public Text gameOverImg;

    void Start () {
        GameObject inGameUI = GameObject.Find("InGameUI").gameObject;
        GameObject gui = inGameUI.transform.FindChild("GUI").gameObject;

        questionMark = transform.FindChild("QuestionMark").gameObject;
        gameOverObj = gui.transform.FindChild("GameOver").gameObject;
        /*
        gameOverImg = gameOverObj.GetComponent<Text>();
        reloadSaveButton = gameOverObj.transform.FindChild("ReloadSave").gameObject;
        backToMenuButton = gameOverObj.transform.FindChild("BackToMenu").gameObject;
        staminaBar = gui.transform.FindChild("StaminaBar").GetComponent<Slider>();
        yesButton = gui.transform.FindChild("TextBoxNormal").FindChild("ButtonYes").GetComponent<Button>();
        noButton = gui.transform.FindChild("TextBoxNormal").FindChild("ButtonNo").GetComponent<Button>();
        */
    }
	
	void Update () {
	
	}
}
