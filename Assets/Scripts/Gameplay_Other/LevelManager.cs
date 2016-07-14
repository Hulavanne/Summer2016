using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class LevelManager : MonoBehaviour
{
	public static LevelManager current;

    public enum Levels
    {
        CIERAN_BEDROOM = 0,
        KITCHEN = 1,
        PARENTS_BEDROOM = 2,
        YARD = 3,
        FORESTOPENING = 4,
        FOREST_LEFT = 5,
        FOREST_RIGHT_BELLADONA = 6,
        FOREST_DEER = 7,
    };

	public Levels currentLevel = Levels.CIERAN_BEDROOM;
	public List<GameObject> levelsList = new List<GameObject>();
	public List<Savepoint> savepointsList = new List<Savepoint>();
    
    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    public GameObject currentDoor;
    public GameObject nextDoor;
    public DoorBehaviour currentDoorBehav;
    public DoorBehaviour nextDoorBehav;

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
		// Making sure the game starts from level 1 if playing a build version of the game
		#if (!UNITY_EDITOR)
        currentLevel = Levels.CIERAN_BEDROOM;
		#endif

		current = this;

		Transform levelsParent = GameObject.Find("Levels").transform;
        Transform savepointsParent = GameObject.Find("Savepoints").transform;

		for (int i = 0; i < levelsParent.childCount; ++i)
		{
			levelsList.Add(levelsParent.GetChild(i).gameObject);
		}

        for (int i = 0; i < savepointsParent.childCount; ++i)
        {
            savepointsList.Add(savepointsParent.GetChild(i).GetComponent<Savepoint>());
            savepointsList[i].savepointIndex = i;
        }

        lightAmount = lightLevel1;
        cameraComponent = Camera.main;
        cameraScript = cameraComponent.GetComponent<CameraFollowAndEffects>();
        menuController = GameObject.Find("InGameUI").GetComponent<MenuController>();
    }

	void Start()
	{
        if (Game.current != null)
        {
            // If current isn't a new game, load saved level
            if (!Game.current.newGame)
            {
                LoadSavedLevel();
            }
            else
            {
                // For starting a new game from somewhere else than level 1
                if (currentLevel != Levels.CIERAN_BEDROOM)
                {
                    TestStart();
                }
            }
        }
        else
        {
            // For starting a new game from somewhere else than level 1
            if (currentLevel != Levels.CIERAN_BEDROOM)
            {
                TestStart();
            }
        }
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
        nextDoorBehav = nextDoor.GetComponent<DoorBehaviour>();

        currentLevel = nextDoorBehav.thisDoorLevel;
		lightAmount = levelsList[(int)currentLevel].GetComponent<Level>().levelLightAmount;

        player.transform.position = new Vector3(nextDoor.transform.position.x, player.transform.position.y, player.transform.position.z);
		
        CameraFollowAndEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
    }

	public void LoadSavedLevel()
	{
		currentLevel = (Levels)Game.current.levelIndex;

		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

        Transform spawnPoint = savepointsList[Game.current.savepointIndex].transform.GetChild(0);
        Vector2 startingPosition = spawnPoint.position;
		player.transform.position = new Vector3(startingPosition.x, startingPosition.y, player.transform.position.z);

		CameraFollowAndEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
	}

	public void TestStart()
	{
		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

		float startingPositionX = levelScript.transform.position.x;
		player.transform.position = new Vector3(startingPositionX, player.transform.position.y, player.transform.position.z);

		CameraFollowAndEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
	}
}
