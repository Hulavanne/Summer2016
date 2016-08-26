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

    CanvasRenderer gameEndingText;

    void Awake ()
    {
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

        gameEndingText = gameOverObj.transform.FindChild("EndingText").GetComponent<CanvasRenderer>();
    }

    public void SetHud(bool option)
    {
        // Sets stamina, pause and action button active / inactive (for dialogue box mostly)
        staminaBarObj.SetActive(option);
        pauseButtonObj.SetActive(option);
        actionButtonObj.SetActive(option);
    }

    public IEnumerator GameOverSplash(bool gameEnding)
    {
        if (gameEnding && EventManager.ending == EventManager.Ending.TRUE)
        {
            string endText = "And he lived happily ever after…";
            gameOverImg.GetComponent<Text>().text = endText;
        }

        // Splashes black screen, fades gameover frame and after that displays buttons.
        gameOverObj.SetActive(true);
        gameOverImg.SetActive(true);

        while (opacity < 1.0f)
        {
            opacity += 0.015f;
            gameOverImg.GetComponent<CanvasRenderer>().SetAlpha(opacity);
            yield return null;
        }

        opacity = 1.0f;
        gameOverImg.GetComponent<CanvasRenderer>().SetAlpha(opacity);

        if (!gameEnding)
        {
            if (canShowGameOverButtons)
            {
                canShowGameOverButtons = false;
                reloadSaveButton.SetActive(true);
                backToMenuButton.SetActive(true);
            }
        }
        else
        {
            if (EventManager.ending == EventManager.Ending.TRUE)
            {
                StartCoroutine(GameEndingSplash());
            }
        }
    }

    public IEnumerator GameEndingSplash()
    {
        opacity = 0.0f;
        gameEndingText.gameObject.SetActive(true);

        while (opacity < 1.0f)
        {
            opacity += 0.015f;
            gameEndingText.SetAlpha(opacity);
            yield return null;
        }

        opacity = 1.0f;
        gameEndingText.SetAlpha(opacity);
    }
}
