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
        FOREST_RIGHT_BELLADONNA = 6,
        FOREST_DEER = 7,
        FOREST_ENEMY = 8,
        FOREST_LILIES = 9,
        CAVE_ENTRANCE = 10,
        CAVE_CREVICE = 11,
        CAVE_PUZZLE = 12,
        CAVE_AFTER_PUZZLE = 13,
        WEIRD_FOREST_AFTER_CAVE = 14,
        WEIRD_FOREST_BELLADONNA = 15,
        WEIRD_FOREST_OPENING = 16,
        WEIRD_YARD = 17,
        HELL_KITCHEN = 18,
        HELL_BEDROOM = 19,
        HELL_WELL = 20,
        HELL_CANDLE = 21
    };

	public Levels currentLevel = Levels.CIERAN_BEDROOM;

    public List<Level> levelsList = new List<Level>();
    public List<Savepoint> savepointsList = new List<Savepoint>();
    public List<EnemyBehaviour> enemiesList = new List<EnemyBehaviour>();

    public GameObject currentDoor;
    public GameObject nextDoor;
    public DoorBehaviour currentDoorBehav;
    public DoorBehaviour nextDoorBehav;

    Camera cameraComponent;
    CameraEffects cameraScript;

    public float lightAmount;
    
    public GameObject lightInLevel;

    MenuController menuController;

    public Color tempColor;
    public CanvasRenderer lightPlaneRenderer, darknessPlaneRenderer;
    public Image colorOfLightPlane, colorOfDarkPlane;

    public PlayerController player;
    public GameObject playerG;
    public GameObject questionMark;

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

		for (int i = 0; i < levelsParent.childCount; ++i)
		{
            levelsList.Add(levelsParent.GetChild(i).GetComponent<Level>());
        }

        savepointsList = GameObject.FindObjectsOfType<Savepoint>().ToList();
        enemiesList = GameObject.FindObjectsOfType<EnemyBehaviour>().ToList();

        // Deactivate all enemies
        foreach (EnemyBehaviour behaviour in enemiesList)
        {
            behaviour.gameObject.SetActive(false);
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
                // For starting a new game from somewhere else than the starting level
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

            // For starting a new game from somewhere else than the starting level
            if (currentLevel != Levels.CIERAN_BEDROOM)
            {
                TestStart();
            }
        }

        TextBoxManager.current.DisableTextBox(false);

        // If game starts in Cieran's room
        if (currentLevel == Levels.CIERAN_BEDROOM)
        {
            // Start intro dialogue
            GameObject.FindObjectOfType<IsIntro>().StartIntro();
        }
        else
        {
            // Destroy intro dialogue
            GameObject.FindObjectOfType<IsIntro>().DestroyIntro();
        }

        // Play the music track of the level
        AudioManager.current.SwitchMusic(levelsList[(int)currentLevel].GetComponent<Level>().levelMusic);

        // Deactivate the levels the player is not in
        DeactivateOtherLevels();
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
        if (lightAmount > 1)
        {
            lightAmount = 1;
        }
        if (lightAmount < -1)
        {
            lightAmount = -1;
        }
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
    }

    public void ChangeLevel()
    {
        nextDoorBehav = nextDoor.GetComponent<DoorBehaviour>();
        
        currentLevel = nextDoorBehav.thisDoorLevel;
		lightAmount = levelsList[(int)currentLevel].GetComponent<Level>().levelLightAmount;

        player.transform.position = new Vector3(nextDoor.transform.position.x, player.transform.position.y, player.transform.position.z);
        player.isFacingRight = player.currentDoor.GetComponent<DoorBehaviour>().willFaceRight;

        CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel].gameObject);
        CameraEffects.current.fadeToBlack = false;

        // Play the music track of the level
        AudioManager.current.SwitchMusic(levelsList[(int)currentLevel].GetComponent<Level>().levelMusic);

        foreach (EnemyBehaviour behaviour in enemiesList)
        {
            if (behaviour.thisEnemyLevel == Levels.CAVE_CREVICE)
            {
                if (player.transform.position.x > behaviour.transform.position.x)
                {
                    behaviour.initialMovementDirection = -1;
                    behaviour.movementDirection = -1;
                }
                else
                {
                    behaviour.initialMovementDirection = 1;
                    behaviour.movementDirection = 1;
                }
            }
        }

        DeactivateOtherLevels();
        EventManager.current.LevelEnteringEvent(currentLevel);
    }

	public void LoadSavedLevel()
	{
		currentLevel = (Levels)Game.current.levelIndex;

		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

        player.transform.position = new Vector3(Game.current.spawnPosition.Key, Game.current.spawnPosition.Value, player.transform.position.z);
        CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel].gameObject);
	}

	public void TestStart()
	{
		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

		float startingPositionX = levelScript.transform.position.x;
		player.transform.position = new Vector3(startingPositionX, player.transform.position.y, player.transform.position.z);

        CameraEffects.current.AdjustToLevel(levelsList[(int)currentLevel].gameObject);
	}

    public void DeactivateOtherLevels()
    {
        // Keeps the current level active, deactivates all others
        foreach (Level level in levelsList)
        {
            if (level.GetComponent<Level>().levelName != currentLevel)
            {
                level.gameObject.SetActive(false);
            }
            else
            {
                level.gameObject.SetActive(true);
            }
        }
    }
}
