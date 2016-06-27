using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public static bool startingNewGame = false;
	public static bool gamePaused = false;
	//public string SceneToGo;
	//public int levelId;

	public InventoryManager inventoryManager;
	public GameObject pauseOverlay;
	public GameObject optionsOverlay;
	public GameObject loadMenu;
	public GameObject menu;
    
	void Awake()
    {
		Time.timeScale = 1;

		if (transform.GetComponentInChildren<InventoryManager>() != null)
		{
			inventoryManager = transform.GetComponentInChildren<InventoryManager>();
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
		else
		{
			optionsOverlay = transform.gameObject;
			transform.gameObject.SetActive(false);
		}
	}

	//IN-GAME

	public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void PauseGame()
	{
		gamePaused = true;
		pauseOverlay.SetActive(true);
		Time.timeScale = 0;

		inventoryManager.FillItemSlots();
	}

    public void ResumeGame()
    {
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

	//MAIN MENU

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
			OpenLoadMenu(true);
		}
	}

	public void OpenLoadMenu(bool pickSaveSlot)
	{
		loadMenu.SetActive(true);
		menu.SetActive(false);

		Text titleText = transform.FindChild("Title").GetComponent<Text>();

		if (pickSaveSlot)
		{
			titleText.text = "Pick A Save Slot";
		}
		else
		{
			titleText.text = "Load Game";
		}
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
		GoToScene("MainScene");
	}

	public void LoadGame(int gameIndex)
	{
		SavingAndLoading.LoadSavedGames();
		Game.current = SavingAndLoading.savedGames[gameIndex];
		GoToScene(Game.current.level);
	}

	public void CloseLoadMenu()
	{
		MenuController.startingNewGame = false;

		menu.SetActive(true);

		if (loadMenu != null)
		{
			loadMenu.SetActive(false);
		}
		else
		{
			this.gameObject.SetActive(false);
		}
	}

	//OPTIONS

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
