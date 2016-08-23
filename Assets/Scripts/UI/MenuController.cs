using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    public static string currentScene = "MainMenu";
	public static bool savingGame = false;
	public static bool gamePaused = false;
    public static bool confirming = false;

    public enum State
    {
        MAIN_MENU_OR_CLOSED,
        INVENTORY,
        LOAD_MENU,
        OPTIONS,
        CREDITS,
    }
    public static State currentState = State.MAIN_MENU_OR_CLOSED;

    public AudioClip buttonSoundEffect;

    GameObject gui;
	GameObject floatingMessage;
	GameObject gameOverScreen;

	GameObject menu;
	GameObject pauseOverlay;
	GameObject loadMenu;
	GameObject optionsOverlay;
    GameObject creditsOverlay;
    GameObject confirmationOverlay;

	string gameScene = "MainScene";
	string pickSaveSlotString = "Pick a Save Slot";
	string loadGameString = "Load Game";
    string quitConfirmationString = "Are you sure you want to exit to the main menu?";
    string saveConfirmationString = "Are you sure you want to overwrite the existing save file?";

    int index = 0;
    
	void Awake()
    {
        Time.timeScale = 1;

        if (GameObject.Find("MainMenuUI") != null)
        {
            menu = GameObject.Find("MainMenuUI");
        }
        if (GameObject.Find("InGameUI") != null)
        {
            menu = GameObject.Find("InGameUI");
        }
        if (GameObject.Find("LoadMenu") != null)
        {
            loadMenu = GameObject.Find("LoadMenu");
        }
        if (GameObject.Find("OptionsOverlay") != null)
        {
            optionsOverlay = GameObject.Find("OptionsOverlay");
        }
        if (GameObject.Find("CreditsOverlay") != null)
        {
            creditsOverlay = GameObject.Find("CreditsOverlay");
        }
        if (GameObject.Find("ConfirmationOverlay") != null)
        {
            confirmationOverlay = GameObject.Find("ConfirmationOverlay");
        }
	}

	void Start()
	{
        if (transform.name == "InGameUI")
        {
            if (transform.FindChild("GUI") != null)
            {
                gui = transform.FindChild("GUI").gameObject;
                floatingMessage = gui.transform.FindChild("FloatingMessageText").gameObject;
                gameOverScreen = gui.transform.FindChild("GameOver").gameObject;

                foreach (Transform child in gameOverScreen.transform)
                {
                    child.gameObject.SetActive(false);
                }
                gameOverScreen.SetActive(false);
            }
            if (transform.FindChild("PauseScreen") != null)
            {
                pauseOverlay = transform.FindChild("PauseScreen").gameObject;
                //pauseOverlay.GetComponentInChildren<InventoryManager>().Setup();
                pauseOverlay.SetActive(false);
            }
        }

        if (transform.name != "MainMenuUI" && transform.name != "InGameUI")
        {
            transform.gameObject.SetActive(false);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (confirming)
            {
                DeactivateConfirmationOverlay();
                return;
            }

            if (currentState == State.INVENTORY)
            {
                if (!InventoryItemSlots.inspectingItem)
                {
                    CloseInventory();
                }
                else
                {
                    ItemSlideMenu.current.itemSlides[1].GetComponent<InventoryItemSlots>().StopInspectingItem();
                }
            }
            else if (currentState == State.LOAD_MENU)
            {
                CloseLoadMenu();
            }
            else if (currentState == State.OPTIONS)
            {
                DeactivateOptionsOverlay();
            }
            else if (currentState == State.CREDITS)
            {
                DeactivateCreditsOverlay();
            }
        }
    }

	public void GoToScene(string sceneName)
    {
        // Set state
        currentState = State.MAIN_MENU_OR_CLOSED;

        // Make sure the game isn't paused
        ResumeGame();

        // Set currentScene, that will be used in GameManager to move on from the loading scene
        // and then go to the loading scene
        currentScene = sceneName;
        GameManager.sceneLoadOperation = SceneManager.LoadSceneAsync("LoadingScene");
	}

	public void ActivateOptionsOverlay()
	{
        // Set state
        currentState = State.OPTIONS;

		// Load values
		optionsOverlay.GetComponent<MenuController>().SetOptionsValues();

		optionsOverlay.SetActive(true);
		menu.SetActive(false);
	}

	public void DeactivateOptionsOverlay()
	{
        // If in the Main Menu:
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // Set state
            currentState = State.MAIN_MENU_OR_CLOSED;
        }
        else
        {
            // Set state
            currentState = State.INVENTORY;
        }

		// Saving the settings
        AudioManager.current.SaveAudioSettings();
		GameManager.current.SaveGraphicalSettings();

		menu.SetActive(true);
		optionsOverlay.SetActive(false);
	}

	public void ActivateCreditsOverlay()
	{
        // Set state
        currentState = State.CREDITS;

		creditsOverlay.SetActive(true);
		optionsOverlay.SetActive(false);
	}

	public void DeactivateCreditsOverlay()
	{
        // Set state
        currentState = State.OPTIONS;

		optionsOverlay.SetActive(true);
		creditsOverlay.SetActive(false);
	}

	public void PlayButtonSoundEffect()
	{
        AudioManager.current.PlayRandomSoundEffect(buttonSoundEffect);
	}

	//----------------------IN-GAME----------------------

	public void ActionButton()
	{
		PlayerController.current.OnActionButtonClick();
	}

	public void ReloadLevel()
	{
		LevelManager.current.ReloadLevel();
	}

	public void OpenInventory()
	{
        // Set state
        currentState = State.INVENTORY;

        PauseGame();

        //TextBoxManager.current.DisableTextBox();
		gui.SetActive(false);
		pauseOverlay.SetActive(true);

		InventoryManager.current.SetSlideVariables();
		InventoryManager.current.FillItemSlots();
	}

    public void CloseInventory()
    {
        // Set state
        currentState = State.MAIN_MENU_OR_CLOSED;

        ResumeGame();

        ItemSlideMenu.current.itemSlides[1].GetComponent<InventoryItemSlots>().StopInspectingItem();
		gui.SetActive(true);
        pauseOverlay.SetActive(false);
    }

    public void PauseGame()
    {
        AudioListener.pause = true;
        MenuController.gamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        MenuController.gamePaused = false;
        Time.timeScale = 1;
    }

	//---------------------LOAD MENU---------------------

	public void NewGame()
	{
		Game game = new Game();
		Game.current = game;
		Game.currentIndex = -1;

		GoToScene(gameScene);
	}

	public void OpenLoadMenu()
	{
        // Set state
        currentState = State.LOAD_MENU;

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
            PauseGame();
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
            FillSaveSlot(saveSlots[0], 0);

			for (int i = 2 + indexModifier; i < saveSlots.Count; ++i)
			{
				saveSlots[i].GetComponent<Button>().interactable = false;
			}
		}
		else if (SavingAndLoading.savedGames.Count == 2)
		{
			for (int i = 0; i < saveSlots.Count - 1; ++i)
			{
                FillSaveSlot(saveSlots[i], i);
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
                FillSaveSlot(saveSlots[i], i);
			}
		}

		loadMenu.SetActive(true);
		menu.SetActive(false);
	}

    public void FillSaveSlot(GameObject saveSlot, int saveSlotIndex)
    {
        saveSlot.transform.FindChild("SaveInfo").GetComponent<Text>().text = GetSaveInfo(saveSlotIndex);

        Sprite image = GameManager.current.levelImages[Mathf.Clamp(SavingAndLoading.savedGames[saveSlotIndex].levelIndex, 0, GameManager.current.levelImages.Count- 1)];
        saveSlot.transform.FindChild("Image").GetComponent<Image>().sprite = image;
        saveSlot.transform.FindChild("Image").gameObject.SetActive(true);
    }

	public string GetSaveInfo(int savedGamesIndex)
	{
        // Convert the index of the saved level to a string
        int levelIndex = SavingAndLoading.savedGames[savedGamesIndex].levelIndex;
        string levelName = "Unknown";

        if (levelIndex <= 3)
        {
            levelName = "House";
        }
        else if (levelIndex >= 4 && levelIndex <= 9)
        {
            levelName = "Forest";
        }
        else if (levelIndex >= 10 && levelIndex <= 13)
        {
            levelName = "Cave";
        }
        else if (levelIndex >= 14 && levelIndex <= 15)
        {
            levelName = "Strange Forest";
        }

		// Format the date into yyyy-mm-dd, excluding the time
		string formattedDateTime = string.Format("{0:yyyy.MM.dd}", SavingAndLoading.savedGames[savedGamesIndex].dateTime);

        return levelName + "\n" +
			formattedDateTime + "\n" +
			SavingAndLoading.savedGames[savedGamesIndex].hours + " h " +
			SavingAndLoading.savedGames[savedGamesIndex].minutes + " min";
	}

	public void PickSaveSlot(int saveSlotIndex)
	{
		// Picking a slot to save a new game to
		if (MenuController.savingGame)
		{
            // If the slot already has a save file
            if (saveSlotIndex <= SavingAndLoading.savedGames.Count - 1)
            {
                index = saveSlotIndex;
                ActivateConfirmationOverlay();
            }
            else
            {
                SaveGame(saveSlotIndex);
            }
		}
		// Loading an existing game file
		else
		{
			LoadGame(saveSlotIndex);
		}
	}

    public void ActivateConfirmationOverlay()
    {
        confirming = true;
        confirmationOverlay.SetActive(true);

        Text textComponent = confirmationOverlay.transform.FindChild("WarningText").GetComponent<Text>();

        if (currentState == State.INVENTORY)
        {
            textComponent.text = quitConfirmationString;
        }
        else if (currentState == State.LOAD_MENU)
        {
            textComponent.text = saveConfirmationString;
        }
    }

    public void DeactivateConfirmationOverlay()
    {
        confirming = false;
        confirmationOverlay.SetActive(false);
    }

    public void Confirm(bool confirmed)
    {
        if (confirmed)
        {
            if (currentState == State.INVENTORY)
            {
                GoToScene("MainMenu");
            }
            else if (currentState == State.LOAD_MENU)
            {
                SaveGame(index);
            }
        }

        DeactivateConfirmationOverlay();
    }

	public void SaveGame(int saveSlotIndex)
	{
        Game game;

        if (Game.current != null)
        {
            game = Game.current;
        }
        else
        {
            game = new Game();
            Game.current = game;
            Game.currentIndex = -1;
        }

        GameManager.current.UpdateCurrentGameVariables();
        game.newGame = false;

		if (saveSlotIndex <= SavingAndLoading.savedGames.Count - 1)
		{
			SavingAndLoading.savedGames[saveSlotIndex] = game;
			Game.currentIndex = saveSlotIndex;
		}
		else
		{
			SavingAndLoading.savedGames.Add(game);
			Game.currentIndex = SavingAndLoading.savedGames.Count - 1;
		}

		SavingAndLoading.SaveGames();

		// Close load menu and resume the game
		CloseLoadMenu();

		// Display "Game Saved" message
		menu.GetComponent<MenuController>().floatingMessage.GetComponent<Text>().text = "Game Saved";
		menu.GetComponent<MenuController>().floatingMessage.GetComponent<FadeText>().StartTimer();
	}

	public void LoadGame(int gameIndex)
	{
		SavingAndLoading.LoadSavedGames();
		Game.current = SavingAndLoading.savedGames[gameIndex];
		Game.currentIndex = gameIndex;

		GoToScene(gameScene);
	}

	public void CloseLoadMenu()
	{
        // Set state
        currentState = State.MAIN_MENU_OR_CLOSED;
        savingGame = false;

        if (PlayerController.current != null && !PlayerController.current.isGameOver)
        {
            ResumeGame();
        }

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

        masterVolumeSlider.value = AudioManager.masterVolume * 100;
        masterVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = masterVolumeSlider.value.ToString() + "%";

        musicVolumeSlider.value = AudioManager.musicVolume * 100;
        musicVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = musicVolumeSlider.value.ToString() + "%";

        soundEffectsVolumeSlider.value = AudioManager.soundEffectsVolume * 100;
        soundEffectsVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = soundEffectsVolumeSlider.value.ToString() + "%";

        gammaSlider.value = GameManager.gammaValue * 100;
        gammaSlider.transform.FindChild("Percentage").GetComponent<Text>().text = gammaSlider.value.ToString() + "%";

        Image unmutedImage = transform.FindChild("MuteButton").GetComponent<Image>();
        Image mutedImage = transform.FindChild("MuteButton").FindChild("MutedImage").GetComponent<Image>();

        unmutedImage.enabled = !AudioManager.audioMuted;
        mutedImage.enabled = AudioManager.audioMuted;

        if (AudioManager.audioMuted)
        {
            masterVolumeSlider.interactable = false;
            masterVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = masterVolumeSlider.colors.disabledColor;

            musicVolumeSlider.interactable = false;
            musicVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = musicVolumeSlider.colors.disabledColor;

            soundEffectsVolumeSlider.interactable = false;
            soundEffectsVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = soundEffectsVolumeSlider.colors.disabledColor;
        }
        else
        {
            masterVolumeSlider.interactable = true;
            masterVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            musicVolumeSlider.interactable = true;
            musicVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            soundEffectsVolumeSlider.interactable = true;
            soundEffectsVolumeSlider.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	    }
    }

	public void ToggleMute()
	{
        AudioManager.current.ToggleMute();
        SetOptionsValues();
	}

	public void SetMasterVolume(Slider slider)
	{
        AudioManager.current.SetMasterVolume(slider.value / 100);
        slider.transform.FindChild("Percentage").GetComponent<Text>().text = slider.value.ToString() + "%";
	}

	public void SetMusicVolume(Slider slider)
	{
        AudioManager.current.SetMusicVolume(slider.value / 100);
        slider.transform.FindChild("Percentage").GetComponent<Text>().text = slider.value.ToString() + "%";
	}

	public void SetSoundEffectsVolume(Slider slider)
	{
        AudioManager.current.SetSoundEffectsVolume(slider.value / 100);
        slider.transform.FindChild("Percentage").GetComponent<Text>().text = slider.value.ToString() + "%";

		if (gameObject.activeSelf)
		{
            AudioManager.current.PlaySoundEffect(buttonSoundEffect);
		}
	}

	public void SetGamma(Slider slider)
	{
        GameManager.current.SetGamma(slider.value / 100);
        slider.transform.FindChild("Percentage").GetComponent<Text>().text = slider.value.ToString() + "%";
	}
}
