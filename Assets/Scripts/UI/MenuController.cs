using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public static bool savingGame = false;
	public static bool gamePaused = false;

	public AudioClip buttonSoundEffect;

	LevelManager levelManager;
	InventoryManager inventoryManager;
	PlayerController playerController;

    GameObject gui;
	GameObject floatingMessage;
	GameObject gameOverScreen;

	GameObject menu;
	GameObject pauseOverlay;
	GameObject optionsOverlay;
	GameObject loadMenu;

	string pickSaveSlotString = "Pick a Save Slot";
	string loadGameString = "Load Game";
    
	void Awake()
    {
		Time.timeScale = 1;

		if (GameObject.Find("LevelManager") != null)
		{
			levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		}
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		}
        if (transform.FindChild("GUI") != null)
        {
            gui = transform.FindChild("GUI").gameObject;
			floatingMessage = gui.transform.FindChild("FloatingMessageText").gameObject;
			gameOverScreen = gui.transform.FindChild ("GameOver").gameObject;

			foreach (Transform child in gameOverScreen.transform)
			{
				child.gameObject.SetActive(false);
			}
			gameOverScreen.SetActive(false);
        }
        if (transform.FindChild("PauseScreen") != null)
		{
			pauseOverlay = transform.FindChild("PauseScreen").gameObject;
			inventoryManager = pauseOverlay.transform.GetComponentInChildren<InventoryManager>();
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
		// If heading out of the Main Menu:
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			// If current game exists, load its variables into the game manager
			if (Game.current != null)
			{
				GameManager.current.LoadCurrentGameVariables();
			}
		}

		SceneManager.LoadSceneAsync("LoadingScene");
		SceneManager.LoadSceneAsync(sceneName);
	}

	//----------------------IN-GAME----------------------

	public void ActionButton()
	{
		playerController.OnActionButtonClick();
	}

	public void ReloadLevel()
	{
		levelManager.ReloadLevel();
	}

	public void GoToMenu()
	{
		levelManager.GoToMenu();
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
		MenuController.gamePaused = true;

		gui.SetActive(false);
		pauseOverlay.SetActive(true);

		inventoryManager.SetSlideVariables();
		inventoryManager.FillItemSlots();
	}

    public void ResumeGame()
    {
        Time.timeScale = 1;
		MenuController.gamePaused = false;

		gui.SetActive(true);
        pauseOverlay.SetActive(false);
    }

	public void InspectItem(GameObject item)
	{

	}

	public void ActivateOptionsOverlay()
	{
		// Load values
		optionsOverlay.GetComponent<MenuController>().SetOptionsValues();

		optionsOverlay.SetActive(true);
		menu.SetActive(false);
	}

	public void DectivateOptionsOverlay()
	{
		// Saving the settings
		AudioManager.current.SaveAudioSettings();
		GameManager.current.SaveGraphicalSettings();

		menu.SetActive(true);
		optionsOverlay.SetActive(false);
	}

	public void PlayButtonSoundEffect()
	{
		AudioManager.current.PlayRandomizedSoundEffect(buttonSoundEffect);
	}

	//---------------------MAIN MENU---------------------

	public void NewGame()
	{
		Game game = new Game();
		Game.current = game;
		Game.currentIndex = -1;

		GoToScene(game.level);
	}

	public void OpenLoadMenu()
	{
		Text titleText = loadMenu.transform.FindChild("Title").GetComponent<Text>();

		// If selecting a game to be loaded this insures that all empty slots are set to non-interactable,
		// but if saving a game this doesn't do anything
		int indexModifier = 0;

		// If saving a game:
		if (MenuController.savingGame)
		{
			// Set the title text
			titleText.text = pickSaveSlotString;

			// Pause the game
			MenuController.gamePaused = true;
			Time.timeScale = 0;
		}
		// If loading a game:
		else
		{
			// Set the title text and index modifier
			titleText.text = loadGameString;
			indexModifier = -1;
		}

		// Adding all save slots to a list
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

		SavingAndLoading.LoadSavedGames();

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
			saveSlots [0].transform.FindChild("Image").GetComponent<Image>().sprite = null;//SavingAndLoading.savedGames[0].image;

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

		loadMenu.SetActive(true);
		menu.SetActive(false);
	}

	public string GetSaveInfo(int savedGamesIndex)
	{
		// Format the date into yyyy-mm-dd and exclude the time
		string formattedDateTime = string.Format("{0:yyyy-MM-dd}", SavingAndLoading.savedGames[savedGamesIndex].dateTime);

		return SavingAndLoading.savedGames[savedGamesIndex].level + "\n" +
			formattedDateTime + "\n" +
			SavingAndLoading.savedGames[savedGamesIndex].hours + " hours " +
			SavingAndLoading.savedGames[savedGamesIndex].minutes + " minutes\n" +
			"played";
	}

	public void PickSaveSlot(int saveSlotIndex)
	{
		// Picking a slot to save a new game to
		if (MenuController.savingGame)
		{
			SaveGame(saveSlotIndex);
		}
		// Loading an existing game file
		else
		{
			LoadGame(saveSlotIndex);
		}
	}

	public void SaveGame(int saveSlotIndex)
	{
		Game game = Game.current;

		if (saveSlotIndex <= SavingAndLoading.savedGames.Count - 1)
		{
			//Game oldGame = SavingAndLoading.savedGames[saveSlotIndex];
			//Destroy(oldGame);

			SavingAndLoading.savedGames[saveSlotIndex] = game;
			Game.currentIndex = saveSlotIndex;
		}
		else
		{
			SavingAndLoading.savedGames.Add(game);
			Game.currentIndex = SavingAndLoading.savedGames.Count - 1;
		}

		SavingAndLoading.SaveGames();

		// Resume game
		CloseLoadMenu();

		// Display "Game Saved" message
		//menu.GetComponent<MenuController>().floatingMessage.SetActive(true);
		menu.GetComponent<MenuController>().floatingMessage.GetComponent<Text>().text = "Game Saved";
		menu.GetComponent<MenuController>().floatingMessage.GetComponent<FadeText>().StartTimer();
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
		Time.timeScale = 1;
		MenuController.gamePaused = false;
		MenuController.savingGame = false;

		menu.SetActive(true);
		loadMenu.SetActive(false);
	}

	//----------------------OPTIONS----------------------

	public void SetOptionsValues()
	{
		Slider masterVolumeSlider = transform.FindChild("MasterVolumeSlider").GetComponent<Slider>();
		Slider musicVolumeSlider = transform.FindChild("MusicVolumeSlider").GetComponent<Slider>();
		Slider soundEffectsVolumeSlider = transform.FindChild("SoundEffectsVolumeSlider").GetComponent<Slider>();
		Slider gammaSlider = transform.FindChild("GammaSlider").GetComponent<Slider>();
		GameObject mutedImage = transform.FindChild("MuteButton").FindChild("MutedImage").gameObject;

		masterVolumeSlider.value = AudioManager.masterVolume;
		musicVolumeSlider.value = AudioManager.musicVolume;
		soundEffectsVolumeSlider.value = AudioManager.soundEffectsVolume;
		gammaSlider.value = GameManager.gammaValue;
		mutedImage.SetActive(AudioManager.audioMuted);

		//SetMasterVolume(masterVolumeSlider);
		//SetMusicVolume(musicVolumeSlider);
		//SetSoundEffectsVolume(soundEffectsVolumeSlider);
		//SetGamma(gammaSlider);
	}

	public void ToggleMute()
	{
		AudioManager.current.ToggleMute();
		transform.FindChild("MuteButton").FindChild("MutedImage").gameObject.SetActive(AudioManager.audioMuted);
	}

	public void SetMasterVolume(Slider slider)
	{
		AudioManager.current.SetMasterVolume(slider.value);
	}

	public void SetMusicVolume(Slider slider)
	{
		AudioManager.current.SetMusicVolume(slider.value);
	}

	public void SetSoundEffectsVolume(Slider slider)
	{
		AudioManager.current.SetSoundEffectsVolume(slider.value);

		if (gameObject.activeSelf)
		{
			AudioManager.current.PlaySoundEffect(buttonSoundEffect);
			//AudioManager.instance.PlayPitchedSoundEffect(buttonSoundEffect, 1.0f, 2.5f);
		}
	}

	public void SetGamma(Slider slider)
	{
		GameManager.current.SetGamma(slider.value);
	}

	public void DisplayCredits()
	{
		Debug.Log("Displaying Credits");
	}
}
