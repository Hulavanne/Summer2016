using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public enum Level
    {
        LEVEL1,
        LEVEL2,
        LEVEL3,
    };

    Level thisLevel;
    
    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    public float LightAmount;
    public float lightLevel1 = 0.1f,
        lightLevel2 = 0.0f,
        lightLevel3 = -0.5f;
    public GameObject LightInLevel;

    MenuController menuController;

    public Color tempColor;
    public CanvasRenderer lightPlaneRenderer, darknessPlaneRenderer;
    public Image colorOfLightPlane, colorOfDarkPlane;

    public PlayerController player;
    public GameObject playerG;
    public GameObject QuestionMark;
    
    public bool goNextLevel = true; // goes down one level if true, goes down one level if false

    void Awake() {
        thisLevel = Level.LEVEL1;
        LightAmount = lightLevel1;

        menuController = GameObject.Find("InGameUI").GetComponent<MenuController>();
    }

    public void SetLighting()
    {
        if (LightAmount > 1) LightAmount = 1;
        if (LightAmount < -1) LightAmount = -1;

        if (LightAmount > 0) // turn white
        {
            lightPlaneRenderer.SetAlpha(LightAmount);
            //colorOfLightPlane.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            darknessPlaneRenderer.SetAlpha(0.0f);
        }

        else if (LightAmount < 0) // turn black
        {
            
            float newLightAmount;
            newLightAmount = LightAmount * -1;

            darknessPlaneRenderer.SetAlpha(newLightAmount);
            lightPlaneRenderer.SetAlpha(0.0f);

        }
        else
        {
            lightPlaneRenderer.SetAlpha(0.0f);
            darknessPlaneRenderer.SetAlpha(0.0f);
        }
    }

    public void ReloadLevel()
    {
        Debug.Log("Reloading Level");
        SavingAndLoading.LoadSavedGames();

        if (SavingAndLoading.savedGames.Count > 0)
        {
            for (int i = 0; i < SavingAndLoading.savedGames.Count; ++i)
            {
                Debug.Log("working 0");
                if (SavingAndLoading.savedGames[i] == Game.current)
                {
                    menuController.LoadGame(Game.currentIndex);
                    Debug.Log("working 1");
                }
                else
                {
                    if (i == SavingAndLoading.savedGames.Count - 1)
                    {
                        menuController.NewGame();
                        Debug.Log("working 2");
                    }
                }
            }
        }
        else
        {
            menuController.NewGame();
        }

        ReloadSaveButton.SetActive(false);
        BackToMenuButton.SetActive(false);
        player.isGameOver = false;
    }

    public void GoToMenu()
    {
        Debug.Log("Going to Menu");
        menuController.GoToScene("MainMenu");
        ReloadSaveButton.SetActive(false);
        BackToMenuButton.SetActive(false);
        player.isGameOver = false;
    }

    public void ChangePlayerPosition()
    {
        Vector3 tempVec = new Vector3(0, 20, 0); // temporary Vector3

        if (goNextLevel)
        {
            player.transform.position -= tempVec;  // going down means increasing a level
        }
        else
        {
            player.transform.position += tempVec; // therefore going up means decreasing a level
        }
    }

    public void ChangeLevel()
    {

        switch (thisLevel)
        {
            case Level.LEVEL1:
                if (goNextLevel)
                {
                    thisLevel = Level.LEVEL2;
                    ChangePlayerPosition();
                    LightAmount = lightLevel2;
                }
                else
                {
                    //thisLevel = Level.LEVEL;
                    //ChangePlayerPosition();
                    // do nothing here because there is no level -1
                }
                break;
            case Level.LEVEL2:
                if (goNextLevel)
                {
                    thisLevel = Level.LEVEL3;
                    ChangePlayerPosition();
                    LightAmount = lightLevel3;
                }
                else
                {
                    thisLevel = Level.LEVEL1;
                    ChangePlayerPosition();
                    LightAmount = lightLevel1;
                }
                break;
            case Level.LEVEL3:
                if (goNextLevel)
                {
                    // thisLevel = Level.LEVEL4;
                    // ChangePlayerPosition();
                    // do nothing here because there is no level 4
                }
                else
                {
                    thisLevel = Level.LEVEL2;
                    ChangePlayerPosition();
                    LightAmount = lightLevel2;
                }
                break;


                player.isSelectionActive = true;
                QuestionMark.SetActive(true);
                player.isOverlappingDoor = true;
        }


        // Debug.Log("this level > " + thisLevel);

    }

    

    
	
	void Update () {
	    if (!player.switchingLevel)
        {
            SetLighting();
        }
	}
}
