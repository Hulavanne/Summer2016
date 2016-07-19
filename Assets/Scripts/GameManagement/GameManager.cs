using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager current = null;

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
                game.savepointId = collidingSavepoint.gameObject.GetComponent<UniqueId>().uniqueId;
                game.cameraStartPositionX = Camera.main.transform.position.x;
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
