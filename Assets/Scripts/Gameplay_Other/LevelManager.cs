using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class LevelManager : MonoBehaviour {

    public enum Levels
    {
        LEVEL1 = 0,
        LEVEL2 = 1,
        LEVEL3 = 2,
        LEVEL4 = 3,
    };

	public Levels currentLevel;
	//public static int currentLevelIndex;
	public List<GameObject> levelsList = new List<GameObject>();
    
    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    Camera cameraComponent;
    CameraFollowAndEffects cameraScript;

    public float lightAmount;
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

    void Awake()
	{
        currentLevel = Levels.LEVEL1;
		//currentLevelIndex = 0;

        lightAmount = lightLevel1;
        cameraComponent = Camera.main;
        cameraScript = cameraComponent.GetComponent<CameraFollowAndEffects>();
        menuController = GameObject.Find("InGameUI").GetComponent<MenuController>();
    }

	void Update()
	{
		if (!player.switchingLevel)
		{
			SetLighting();
		}
	}

    public void SetLighting()
    {
        if (lightAmount > 1) lightAmount = 1;
        if (lightAmount < -1) lightAmount = -1;
        if (lightAmount > 0) // turn white
        {
            lightPlaneRenderer.SetAlpha(lightAmount);
            darknessPlaneRenderer.SetAlpha(0.0f);
        }

        else if (lightAmount < 0) // turn black
        {
            
            float newLightAmount;
            newLightAmount = lightAmount * -1;

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
        switch (currentLevel)
        {
		case Levels.LEVEL1:
			
            if (player.doorName == "Door_1>2")
            {
				SetLevelVariables(Levels.LEVEL2, lightLevel2, GameObject.Find("Door_2>1"));
            }
        
            break;

        case Levels.LEVEL2:
			
            if (player.doorName == "Door_2>3")
            {
				SetLevelVariables(Levels.LEVEL3, lightLevel3, GameObject.Find("Door_3>2"));
            }
            else if (player.doorName == "Door_2>1")
            {
				SetLevelVariables(Levels.LEVEL1, lightLevel1, GameObject.Find("Door_1>2"));
            }
            break;

        case Levels.LEVEL3:
			
            if (player.doorName == "Door_3>2")
            {
				SetLevelVariables(Levels.LEVEL2, lightLevel2, GameObject.Find("Door_2>3"));
            }
            break;

	        player.isSelectionActive = true;
	        QuestionMark.SetActive(true);
			player.isOverlappingDoor = true;
        }
    }

	void SetLevelVariables(Levels level, float lightValue, GameObject door)
	{
		currentLevel = level;
		lightAmount = lightValue;
		player.transform.position = new Vector3(door.transform.position.x, player.transform.position.y, player.transform.position.z);
		CameraFollowAndEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
	}
}
