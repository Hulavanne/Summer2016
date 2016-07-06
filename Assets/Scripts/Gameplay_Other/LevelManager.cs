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
        LEVEL1 = 0,
        LEVEL2 = 1,
        LEVEL3 = 2,
        LEVEL4 = 3,
    };

	public Levels currentLevel = Levels.LEVEL1;
	public List<GameObject> levelsList = new List<GameObject>();
	public List<Savepoint> savepoints = new List<Savepoint>();
    
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
		current = this;
		List<GameObject> npcs = GameObject.FindGameObjectsWithTag("NPC").ToList();

		foreach (GameObject npc in npcs)
		{
			if (npc.GetComponent<Savepoint>() != null)
			{
				savepoints.Add(npc.GetComponent<Savepoint>());
				npc.GetComponent<Savepoint>().savepointIndex = savepoints.Count - 1;
			}
		}

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
        nextDoorBehav = nextDoor.GetComponent<DoorBehaviour>();

        currentLevel = nextDoorBehav.thisDoorLevel;
		lightAmount = levelsList[(int)currentLevel].GetComponent<Level>().levelLightAmount;

        player.transform.position = new Vector3(nextDoor.transform.position.x,
			player.transform.position.y, player.transform.position.z);
		
        CameraFollowAndEffects.current.AdjustToLevel(levelsList[(int)currentLevel]);
    }

	public void LoadSavedLevel()
	{
		currentLevel = (Levels)Game.current.levelIndex;

		Level levelScript = levelsList[(int)currentLevel].GetComponent<Level>();
		lightAmount = levelScript.levelLightAmount;

		Transform savepoint = LevelManager.current.savepoints[Game.current.savepointIndex].transform;
		Vector2 startingPosition = new Vector2(savepoint.position.x, savepoint.position.y);
		Debug.Log(startingPosition);
		player.transform.position = new Vector3(startingPosition.x, startingPosition.y, transform.position.z);

		Transform boundary = CameraFollowAndEffects.current.GetNearestBoundary(levelsList[(int)currentLevel]);
		CameraFollowAndEffects.current.boundaryColliding = boundary;
		//CameraFollowAndEffects.current.boundaryColliding = true;

		Transform mainCamera = CameraFollowAndEffects.current.transform;
		mainCamera.position = new Vector3(Game.current.cameraStartPositionX, mainCamera.position.y, mainCamera.position.z);//AdjustToLevel(levelsList[(int)currentLevel]);
		CameraFollowAndEffects.current.fixedCamera = levelScript.fixedCamera;
	}
}
