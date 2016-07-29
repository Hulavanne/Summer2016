﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public static bool savingGame = false;
	public static bool gamePaused = false;

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

	LevelManager levelManager;
	InventoryManager inventoryManager;
	PlayerController playerController;

    GameObject gui;
	GameObject floatingMessage;
	GameObject gameOverScreen;

	GameObject menu;
	GameObject pauseOverlay;
	GameObject loadMenu;
	GameObject optionsOverlay;
	GameObject creditsOverlay;

	string gameScene = "MainScene";
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
            inventoryManager = pauseOverlay.transform.GetComponentInChildren<InventoryManager>();
            inventoryManager.Setup();
            pauseOverlay.SetActive(false);
        }
        if (transform.name != "LoadMenu" && transform.name != "OptionsOverlay" && transform.name != "CreditsOverlay")
        {
            menu = transform.gameObject;

            if (GameObject.Find("LoadMenu") != null)
            {
                loadMenu = GameObject.Find("LoadMenu");
                loadMenu.GetComponent<MenuController>().menu = menu;
            }
            if (GameObject.Find("OptionsOverlay") != null)
            {
                optionsOverlay = GameObject.Find("OptionsOverlay");
                optionsOverlay.GetComponent<MenuController>().menu = menu;
            }
            if (GameObject.Find("CreditsOverlay") != null)
            {
                creditsOverlay = GameObject.Find("CreditsOverlay");
                creditsOverlay.GetComponent<MenuController>().menu = menu;
            }
        }
        else
        {
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
        }
	}

	void Start()
	{
        if (transform.name == "OptionsOverlay" || transform.name == "LoadMenu" || transform.name == "CreditsOverlay")
        {
            transform.gameObject.SetActive(false);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == State.MAIN_MENU_OR_CLOSED)
            {
                
            }
            else if (currentState == State.INVENTORY)
            {
                if (!InventoryItemSlots.inspectingItem)
                {
                    ResumeGame();
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

		// If heading out of the Main Menu:
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			// If current game exists, load its variables into the game manager
			if (Game.current != null)
			{
				GameManager.current.LoadCurrentGameVariables();
			}
		}
        gamePaused = false;

		SceneManager.LoadSceneAsync("LoadingScene");
		SceneManager.LoadSceneAsync(sceneName);
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
		AudioManager.current.PlayRandomizedSoundEffect(buttonSoundEffect);
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
        // Set state
        currentState = State.INVENTORY;

		Time.timeScale = 0;
		MenuController.gamePaused = true;

        //TextBoxManager.current.DisableTextBox();
		gui.SetActive(false);
		pauseOverlay.SetActive(true);

		inventoryManager.SetSlideVariables();
		inventoryManager.FillItemSlots();
	}

    public void ResumeGame()
    {
        // Set state
        currentState = State.MAIN_MENU_OR_CLOSED;

        Time.timeScale = 1;
		MenuController.gamePaused = false;

        ItemSlideMenu.current.itemSlides[1].GetComponent<InventoryItemSlots>().StopInspectingItem();
		gui.SetActive(true);
        pauseOverlay.SetActive(false);
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
		// Format the date into yyyy-mm-dd and exclude the time
		string formattedDateTime = string.Format("{0:yyyy.MM.dd}", SavingAndLoading.savedGames[savedGamesIndex].dateTime);

		return SavingAndLoading.savedGames[savedGamesIndex].levelIndex + "\n" +
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

            }

			SaveGame(saveSlotIndex);
		}
		// Loading an existing game file
		else
		{
			LoadGame(saveSlotIndex);
		}
	}

    public void ActivateConfirmationOverlay(int saveSlotIndex)
    {

    }

    public void DeactivateConfirmationOverlay(int saveSlotIndex)
    {

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

		GoToScene(gameScene);
	}

	public void CloseLoadMenu()
	{
        // Set state
        currentState = State.MAIN_MENU_OR_CLOSED;

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

		masterVolumeSlider.value = AudioManager.masterVolume * 100;
        masterVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = masterVolumeSlider.value.ToString() + "%";

        musicVolumeSlider.value = AudioManager.musicVolume * 100;
        musicVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = musicVolumeSlider.value.ToString() + "%";

        soundEffectsVolumeSlider.value = AudioManager.soundEffectsVolume * 100;
        soundEffectsVolumeSlider.transform.FindChild("Percentage").GetComponent<Text>().text = soundEffectsVolumeSlider.value.ToString() + "%";

        gammaSlider.value = GameManager.gammaValue * 100;
        gammaSlider.transform.FindChild("Percentage").GetComponent<Text>().text = gammaSlider.value.ToString() + "%";

		mutedImage.SetActive(AudioManager.audioMuted);
	}

	public void ToggleMute()
	{
		AudioManager.current.ToggleMute();
		transform.FindChild("MuteButton").FindChild("MutedImage").gameObject.SetActive(AudioManager.audioMuted);
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
			//AudioManager.instance.PlayPitchedSoundEffect(buttonSoundEffect, 1.0f, 2.5f);
		}
	}

	public void SetGamma(Slider slider)
	{
        GameManager.current.SetGamma(slider.value / 100);
        slider.transform.FindChild("Percentage").GetComponent<Text>().text = slider.value.ToString() + "%";
	}
}
