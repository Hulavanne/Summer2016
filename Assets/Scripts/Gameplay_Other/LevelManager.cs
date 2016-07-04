using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public enum Level
    {
        LEVEL1,
        LEVEL2,
        LEVEL3,
        LEVEL4,
    };

    Level thisLevel;
    
    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    Camera cameraComponent;
    CameraFollowAndEffects cameraScript;

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
    
    public int goNextLevel = 0; // goes down one level if true, goes down one level if false

    void Awake() {
        thisLevel = Level.LEVEL1;
        LightAmount = lightLevel1;
        cameraComponent = Camera.main;
        cameraScript = cameraComponent.GetComponent<CameraFollowAndEffects>();
        menuController = GameObject.Find("InGameUI").GetComponent<MenuController>();
    }

    public void SetLighting()
    {
        if (LightAmount > 1) LightAmount = 1;
        if (LightAmount < -1) LightAmount = -1;
        if (LightAmount > 0) // turn white
        {
            lightPlaneRenderer.SetAlpha(LightAmount);
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
        SavingAndLoading.LoadSavedGames();

        if (SavingAndLoading.savedGames.Count > 0)
        {
            for (int i = 0; i < SavingAndLoading.savedGames.Count; ++i)
            {
                if (SavingAndLoading.savedGames[i] == Game.current)
                {
                    menuController.LoadGame(Game.currentIndex);
                }
                else
                {
                    if (i == SavingAndLoading.savedGames.Count - 1)
                    {
                        menuController.NewGame();
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

    public void ChangeLevel()
    {
        GameObject door;
        switch (thisLevel)
        {
            case Level.LEVEL1:
                if (player.doorName == "Door_1>2")
                {
                    thisLevel = Level.LEVEL2;
                    LightAmount = lightLevel2;
                    door = GameObject.Find("Door_2>1");
                    player.transform.position = new Vector3 (door.transform.position.x, player.transform.position.y, player.transform.position.z);
                    //cameraScript.JoinPlayer();
                }
            
                break;
            case Level.LEVEL2:
                if (player.doorName == "Door_2>3")
                {
                    thisLevel = Level.LEVEL3;
                    LightAmount = lightLevel3;
                    door = GameObject.Find("Door_3>2");
                    player.transform.position = new Vector3(door.transform.position.x, player.transform.position.y, player.transform.position.z);
                    //cameraScript.JoinPlayer();
                }
                else if (player.doorName == "Door_2>1")
                {
                    thisLevel = Level.LEVEL1;
                    LightAmount = lightLevel1;
                    door = GameObject.Find("Door_1>2");
                    player.transform.position = new Vector3(door.transform.position.x, player.transform.position.y, player.transform.position.z);
                    //cameraScript.JoinPlayer();
                }
                break;
            case Level.LEVEL3:
                if (player.doorName == "Door_3>2")
                {
                    thisLevel = Level.LEVEL2;
                    LightAmount = lightLevel2;
                    door = GameObject.Find("Door_2>3");
                    player.transform.position = new Vector3(door.transform.position.x, player.transform.position.y, player.transform.position.z);
                    //cameraScript.JoinPlayer();
                }
                break;

            player.isSelectionActive = true;
            QuestionMark.SetActive(true);
            player.isOverlappingDoor = true;
        }
    }
    
	void Update () {
	    if (!player.switchingLevel)
        {
            SetLighting();
        }
	}
}
