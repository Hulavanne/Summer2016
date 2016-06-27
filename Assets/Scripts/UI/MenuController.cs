using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public static bool startingNewGame = false;
	public static bool gamePaused = false;

	GameManager gameManager;
	InventoryManager inventoryManager;
    GameObject gui;
	GameObject menu;
	GameObject pauseOverlay;
	GameObject optionsOverlay;
	GameObject loadMenu;

	string pickSaveSlotString = "Pick A Save Slot";
	string loadGameString = "Load Game";
    
	void Awake()
    {
		Time.timeScale = 1;
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		if (transform.GetComponentInChildren<InventoryManager>() != null)
		{
			inventoryManager = transform.GetComponentInChildren<InventoryManager>();
		}

        if (transform.FindChild("GUI") != null)
        {
            gui = transform.FindChild("GUI").gameObject;
        }

        if (transform.FindChild("PauseScreen") != null)
		{
			pauseOverlay = transform.FindChild("PauseScreen").gameObject;
			pauseOverlay.SetActive(false);
		}

		if (transform.name != "OptionsOverlay" && transform.name != "LoadMenu")
		{
			menu = transform.gameObject;

			if (GameObject.Find("OptionsOverlay") != null)
			{
				optionsOverlay = GameObject.Find("OptionsOverlay");
				optionsOverlay.GetComponent<MenuController>().menu = menu;
				//optionsOverlay.SetActive(false);
			}
			if (GameObject.Find("LoadMenu") != null)
			{
				loadMenu = GameObject.Find("LoadMenu");
				loadMenu.GetComponent<MenuController>().menu = menu;
				//loadMenu.SetActive(false);
			}
		}
	}

	void Start()
	{
		if (transform.name == "OptionsOverlay")
		{
			optionsOverlay = transform.gameObject;
			transform.gameObject.SetActive(false);
		}
		else if (transform.name == "LoadMenu")
		{
			loadMenu = transform.gameObject;
			transform.gameObject.SetActive(false);
		}
	}

	public void GoToScene(string sceneName)
	{
		if (SceneManager.GetActiveScene().name == "MainMenu" && Game.current != null)
		{
			gameManager.LoadCurrentGameVariables();
		}
		SceneManager.LoadScene(sceneName);
	}

	//----------------------IN-GAME----------------------

	public void PauseGame()
	{
        gui.SetActive(false);

        gamePaused = true;
		pauseOverlay.SetActive(true);
		Time.timeScale = 0;

		inventoryManager.FillItemSlots();
	}

    public void ResumeGame()
    {
        gui.SetActive(true);

        Time.timeScale = 1;
        pauseOverlay.SetActive(false);
        gamePaused = false;
    }

	public void InspectItem(GameObject item)
	{

	}

	public void ActivateOptionsOverlay()
	{
		optionsOverlay.SetActive(true);
		menu.SetActive(false);
	}

	public void DectivateOptionsOverlay()
	{
		menu.SetActive(true);
		optionsOverlay.SetActive(false);
	}

	public void PlaySoundEffect()
	{
		Debug.Log("Playing Button Sound Effect");
	}

	//---------------------MAIN MENU---------------------

	public void NewGame()
	{
		MenuController.startingNewGame = true;
		SavingAndLoading.LoadSavedGames();

		if (SavingAndLoading.savedGames.Count == 0)
		{
			StartNewGame(0);
		}
		else
		{
			OpenLoadMenu();
		}
	}

	public void OpenLoadMenu()
	{
		SavingAndLoading.LoadSavedGames();
		loadMenu.SetActive(true);
		menu.SetActive(false);

		Text titleText = loadMenu.transform.FindChild("Title").GetComponent<Text>();

		List<GameObject> saveSlots = new List<GameObject>();

		saveSlots.Add(loadMenu.transform.FindChild("SaveSlot1").gameObject);
		saveSlots.Add(loadMenu.transform.FindChild("SaveSlot2").gameObject);
		saveSlots.Add(loadMenu.transform.FindChild("SaveSlot3").gameObject);

		// Setting all save slots to interactable and wiping their save information
		for (int i = 0; i < saveSlots.Count; ++i)
		{
			saveSlots[i].GetComponent<Button>().interactable = true;
			saveSlots[i].transform.FindChild("SaveInfo").GetComponent<Text>().text = "";
			saveSlots[i].transform.FindChild("Image").gameObject.SetActive(false);
		}

		// If starting a new game this doesn't do anything,
		// if selecting a game to be loaded this insures that all empty slots are set to non-interactable
		int indexModifier = 0;

		if (MenuController.startingNewGame)
		{
			titleText.text = pickSaveSlotString;
		}
		else
		{
			titleText.text = loadGameString;
			indexModifier = -1;
		}

		// Updating the save slots to display their save information and setting slots that aren't needed to non-interactable
		if (SavingAndLoading.savedGames.Count == 0)
		{
			for (int i = 1 + indexModifier; i < saveSlots.Count; ++i)
			{
				saveSlots[i].GetComponent<Button>().interactable = false;
			}
		}
		else if (SavingAndLoading.savedGames.Count == 1)
		{
			saveSlots[0].transform.FindChild("SaveInfo").GetComponent<Text>().text = GetSaveInfo(0);
			saveSlots [0].transform.FindChild ("Image").GetComponent<Image> ().sprite = null;//SavingAndLoading.savedGames[0].image;

			for (int i = 2 + indexModifier; i < saveSlots.Count; ++i)
			{
				saveSlots[i].GetComponent<Button>().interactable = false;
			}
		}
		else if (SavingAndLoading.savedGames.Count == 2)
		{
			for (int i = 0; i < saveSlots.Count - 1; ++i)
			{
				saveSlots[i].transform.FindChild("SaveInfo").GetComponent<Text>().text = GetSaveInfo(i);
				saveSlots[i].transform.FindChild("Image").GetComponent<Image>().sprite = null;//SavingAndLoading.savedGames[i].image;
			}

			for (int i = 3 + indexModifier; i < saveSlots.Count; ++i)
			{
				saveSlots[i].GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			for (int i = 0; i < saveSlots.Count; ++i)
			{
				saveSlots[i].transform.FindChild("SaveInfo").GetComponent<Text>().text = GetSaveInfo(i);
				saveSlots[i].transform.FindChild("Image").GetComponent<Image>().sprite = null;//SavingAndLoading.savedGames[i].image;
			}
		}
	}

	public string GetSaveInfo(int savedGamesIndex)
	{
		return SavingAndLoading.savedGames[savedGamesIndex].level + "\n" +
			SavingAndLoading.savedGames[savedGamesIndex].dateTime + "\n" +
			SavingAndLoading.savedGames[savedGamesIndex].hours + " hours " +
			SavingAndLoading.savedGames[savedGamesIndex].minutes + " minutes played";
	}

	public void PickSaveSlot(int saveSlotIndex)
	{
		// Picking a slot to save a new game to
		if (MenuController.startingNewGame)
		{
			StartNewGame(saveSlotIndex);
		}
		// Loading an existing game file
		else
		{
			LoadGame(saveSlotIndex);
		}
	}

	public void StartNewGame(int saveSlotIndex)
	{
		Game newGame = new Game();
		Game.current = newGame;
		Game.currentIndex = saveSlotIndex;

		if (saveSlotIndex <= SavingAndLoading.savedGames.Count - 1)
		{
			SavingAndLoading.savedGames[saveSlotIndex] = newGame;
		}
		else
		{
			SavingAndLoading.savedGames.Add(newGame);
		}

		SavingAndLoading.SaveGame();
		MenuController.startingNewGame = false;

		GoToScene(Game.current.level);
	}

	public void LoadGame(int gameIndex)
	{
		SavingAndLoading.LoadSavedGames();
		Game.current = SavingAndLoading.savedGames[gameIndex];
		Game.currentIndex = gameIndex;

		GoToScene(Game.current.level);
	}

	public void CloseLoadMenu()
	{
		MenuController.startingNewGame = false;
		menu.SetActive(true);
		loadMenu.SetActive(false);
	}

	//----------------------OPTIONS----------------------

	public void ToggleMute()
	{
		Debug.Log("Mute Toggled");
	}

	public void SetMasterVolume()
	{
		Debug.Log("Master Volume Changed");
	}

	public void SetMusicVolume()
	{
		Debug.Log("Music Volume Changed");
	}

	public void SetSoundEffectsVolume()
	{
		Debug.Log("Sound Effects Volume Changed");
	}

	public void SetGamma()
	{
		Debug.Log("Gamma Changed");
	}

	public void DisplayCredits()
	{
		Debug.Log("Displaying Credits");
	}
}
