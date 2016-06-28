using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

	void Awake ()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update ()
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
				game.startingPositionX = collidingSavepoint.transform.position.x;
				game.startingPositionY = collidingSavepoint.transform.position.y;
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

	void OnGUI()
	{
		//GUI.Label(new Rect(50, 50, 400, 50), "Played Time: " + hours.ToString() + " Hours " + minutes.ToString() + " Minutes " + seconds.ToString() + " Seconds");
	}
}
