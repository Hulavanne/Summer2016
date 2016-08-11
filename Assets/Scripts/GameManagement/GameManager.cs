using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager current = null;
    public static AsyncOperation sceneLoadOperation = null;
	public static float gammaValue = 0.5f;

	public Savepoint collidingSavepoint; // Savepoint that the player is colliding currently
	public System.DateTime dateTime; // Date and time
    public double playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

    public List<Sprite> levelImages = new List<Sprite>();

	void Awake ()
	{
		// Get a static reference to this game object
		if (current == null)
        {
			current = this;
		}
		// Making sure there is only a single AudioManager in the scene
		else if (current != this)
        {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		// Load graphical values
		LoadGraphicalSettings();
	}

	void Update ()
	{
        if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                // Update date and time in the manager
                dateTime = System.DateTime.Now;

                // Update played time in the manager
                playedTime += Time.deltaTime;

                // Update seconds, minutes and hours in the manager
                seconds = (int)playedTime % 60;
                minutes = ((int)playedTime / 60) % 60;
                hours = ((int)playedTime / 3600);
            }
        }
        else
        {
            // Executed during the first frame of the loading scene
            if (GameManager.sceneLoadOperation != null && GameManager.sceneLoadOperation.isDone)
            {
                // Fade the current music
                AudioManager.current.StartFadeMusic(true, 0.5f, false);

                // If loading the main scene and if current game exists
                if (MenuController.currentScene == "MainScene" && Game.current != null)
                {
                    // Load saved variables
                    LoadCurrentGameVariables();
                }

                // Load scene and set sceneLoadOperation
                sceneLoadOperation = SceneManager.LoadSceneAsync(MenuController.currentScene);
            }
        }
	}

    public void UpdateCurrentGameVariables()
    {
        if (Game.current != null)
        {
            Game game = Game.current;

            // Get the index of the level
            game.levelIndex = (int)LevelManager.current.currentLevel;

            // Get the index of the current savepoint and the position of the camera
            if (collidingSavepoint != null)
            {
                game.spawnPosition = new KeyValuePair<float, float>(PlayerController.current.transform.position.x, PlayerController.current.transform.position.y);
            }

            // Update date and time in the current game
            game.dateTime = dateTime;

            // Update played time in the current game
            game.playedTime = playedTime;

            // Update seconds, minutes and hours in the current game
            game.seconds = seconds;
            game.minutes = minutes;
            game.hours = hours;

            // Update the items in the scene
            game.itemsDataScene.Clear();

            for (int i = 0; i < InventoryManager.current.sceneItems.Count; ++i)
            {
                InventoryManager.current.sceneItems[i].itemData.dataStatus = "Modified Data";
                game.itemsDataScene.Add(InventoryManager.current.sceneItems[i].itemData);
            }

            // Update the items in the player's inventory of the current game
            game.itemsDataInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>().itemsData;
        }
    }

    /*public IEnumerator LoadSceneAfterMusicFade()
    {
        yield return StartCoroutine(AudioManager.current.FadeMusic(true, 0.5f, false));

        // If loading the main scene
        if (MenuController.currentScene == "MainScene")
        {
            // If current game exists, load its variables into the game manager
            if (Game.current != null)
            {
                LoadCurrentGameVariables();
            }
        }
        // If returning to the main menu
        else if (MenuController.currentScene == "MainMenu")
        {
            // Switching to the correct track
            AudioManager.current.SwitchMusic(AudioManager.current.menuMusic);
        }

        // Load scene and set sceneLoadOperation
        sceneLoadOperation = SceneManager.LoadSceneAsync(MenuController.currentScene);
    }*/

	public void LoadCurrentGameVariables()
	{
		Game game = Game.current;

		// Load in the time variables of the current game
		playedTime = game.playedTime;
		seconds = game.seconds;
		minutes = game.minutes;
		hours = game.hours;
	}

	public void LoadGraphicalSettings()
	{
		GameManager.gammaValue = PlayerPrefs.GetFloat("GammaValue", 1.0f);
		SetGamma(GameManager.gammaValue);
	}

	public void SaveGraphicalSettings()
	{
		PlayerPrefs.SetFloat("GammaValue", GameManager.gammaValue);
		PlayerPrefs.Save();
	}

	public void SetGamma(float newValue)
	{
		GameManager.gammaValue = newValue;

        // Update gamma
        if (Camera.main.GetComponent<Brightness>() != null)
        {
            Camera.main.GetComponent<Brightness>().brightness = GameManager.gammaValue;
        }
	}
}
