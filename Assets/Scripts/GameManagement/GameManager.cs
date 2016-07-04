using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public GameObject collidingSavepoint; // Savepoint that the player is colliding currently
	public System.DateTime dateTime; // Date and time
	public float playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

	public enum GameState
	{
		MAIN_MENU,
		IN_GAME
	}
	public GameState currentState = GameState.MAIN_MENU;

	float gammaValue = 0.5f;

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void Update ()
	{
		if (SceneManager.GetActiveScene().name != "LoadingScene")
		{
			if (SceneManager.GetActiveScene().name != "MainMenu")
			{
				currentState = GameManager.GameState.IN_GAME;
			}
			else
			{
				currentState = GameManager.GameState.MAIN_MENU;
			}

			if (currentState == GameState.IN_GAME)
			{
				Game game = Game.current;

				if (collidingSavepoint != null)
				{
					game.playerStartPositionX = collidingSavepoint.transform.position.x;
					game.playerStartPositionY = collidingSavepoint.transform.position.y;
					game.cameraStartPositionX = Camera.main.transform.position.x;
				}

				// Update date and time, both in the manager and in the current game
				dateTime = System.DateTime.Now;
				game.dateTime = dateTime;

				// Update played time, both in the manager and in the current game
				playedTime += Time.deltaTime;
				game.playedTime = playedTime;

				// Update seconds, minutes and hours, both in the manager and in the current game
				seconds = game.seconds = (int)playedTime % 60;
				minutes = game.minutes = ((int)playedTime / 60) % 60;
				hours = game.hours = ((int)playedTime / 3600) % 24;

				// Update the items in the player's inventory of the current game
				game.items = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>().items;
			}
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
}
