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
        FOREST_OPENING = 4,
        FOREST_LEFT = 5,
        FOREST_RIGHT_BELLADONA = 6,
        FOREST_DEER = 7,
        FOREST_ENEMY = 8,
        FOREST_LILIES = 9,
        CAVE_ENTRANCE = 10,
        CAVE_CREVICE = 11,
    };

	public Levels currentLevel = Levels.CIERAN_BEDROOM;
	public List<GameObject> levelsList = new List<GameObject>();
    public List<GameObject> savepointsList = new List<GameObject>();
    
    public GameObject ReloadSaveButton;
    public GameObject BackToMenuButton;

    public GameObject currentDoor;
    public GameObject nextDoor;
    public DoorBehaviour currentDoorBehav;
    public DoorBehaviour nextDoorBehav;

    Camera cameraComponent;
    CameraEffects cameraScript;

    public float lightAmount;
    
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
        darknessPlaneRenderer = GameObject.Find("DarknessModifier").GetComponent<CanvasRenderer>();
        lightPlaneRenderer = GameObject.Find("LightModifier").GetComponent<CanvasRenderer>();

        current = this;

		// Making sure the game starts from level 1 if playing a build version of the game
		#if (!UNITY_EDITOR)
        currentLevel = Levels.CIERAN_BEDROOM;
		#endif

		Transform levelsParent = GameObject.Find("Levels").transform;
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

		for (int i = 0; i < levelsParent.childCount; ++i)
		{
			levelsList.Add(levelsParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < npcs.Count(); ++i)
        {
            if (npcs[i].GetComponent<Savepoint>() != null)
            {
                savepointsList.Add(npcs[i]);
            }
        }
        
        cameraComponent = Camera.main;
        cameraScript = cameraComponent.GetComponent<CameraEffects>();
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
            Game game = new Game();
            Game.current = game;
            Game.currentIndex = -1;

            // For starting a new game from somewhere else than level 1
            if (currentLevel != Levels.CIERAN_BEDROOM)
            {
                TestStart();
            }
        }

        if (currentLevel == Levels.CIERAN_BEDROOM)
        {
            GameObject.FindObjectOfType<IsIntro>().StartIntro();
        }
	}

	void Update()
    {
		if (!player.switchingLevel)
		{
			SetLighting();
		}

        // Keeps the current level active, deactivates all others
        foreach (GameObject level in levelsList)
        {
            if (level.GetComponent<Level>().levelName != currentLevel)
            {
                //level.SetActive(false);
            }
            else
            {
                //level.SetActive(true);
            }
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
        player.playerAnim.SetBool("isFacingRight", player.currentDoor.GetComponent<DoorBehaviour>().willFaceRight);

        CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
        CameraEffects.current.fadeToBlack = false;
    }

	public void LoadSavedLevel()
	{
		currentLevel = (Levels)Game.current.levelIndex;

		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

        player.transform.position = new Vector3(Game.current.spawnPosition.Key, Game.current.spawnPosition.Value, player.transform.position.z);
		CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
	}

	public void TestStart()
	{
		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

		float startingPositionX = levelScript.transform.position.x;
		player.transform.position = new Vector3(startingPositionX, player.transform.position.y, player.transform.position.z);

		CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
	}
}
