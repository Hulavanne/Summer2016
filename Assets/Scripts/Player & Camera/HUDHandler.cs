using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {

    public GameObject questionMark;
    public GameObject gameOverObj;
    public GameObject gameOverImg;
    public GameObject reloadSaveButton;
    public GameObject backToMenuButton;
    public Slider staminaBar;
    public float opacity = 0.0f;
    public bool isSelectionActive;
    public bool canShowGameOverButtons;

    public GameObject staminaBarObj;
    public GameObject pauseButtonObj;
    public GameObject actionButtonObj;

    void Awake () {
        staminaBarObj = GameObject.Find("StaminaBar");
        pauseButtonObj = GameObject.Find("Pause");
        actionButtonObj = GameObject.Find("ActionButton");

        GameObject inGameUI = GameObject.Find("InGameUI").gameObject;
        GameObject gui = inGameUI.transform.FindChild("GUI").gameObject;

        questionMark = transform.FindChild("QuestionMark").gameObject;
        gameOverObj = gui.transform.FindChild("GameOver").gameObject;
        gameOverImg = gameOverObj.transform.FindChild("Title").gameObject;
        reloadSaveButton = gameOverObj.transform.FindChild("ReloadSave").gameObject;
        backToMenuButton = gameOverObj.transform.FindChild("BackToMenu").gameObject;
        staminaBar = gui.transform.FindChild("StaminaBar").GetComponent<Slider>();
    }

    public void SetHud(bool option)
    { // sets stamina, pause and action button active / inactive (for dialogue box mostly)
        staminaBarObj.SetActive(option);
        pauseButtonObj.SetActive(option);
        actionButtonObj.SetActive(option);
    }

    public void GameOverSplash()
    { // splashes black screen, fades gameover frame and after that displays buttons.
        gameOverObj.SetActive(true);
        gameOverImg.SetActive(true);
        opacity += 0.015f;
        gameOverImg.GetComponent<CanvasRenderer>().SetAlpha(opacity);

        if ((opacity >= 1.0f) && (canShowGameOverButtons))
        {
            canShowGameOverButtons = false;
            reloadSaveButton.SetActive(true);
            backToMenuButton.SetActive(true);
        }
    }
}
