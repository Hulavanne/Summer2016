using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game
{
	public static Game current; // Current game
	public static int currentIndex; // Current game's index in the saved games list

	public bool newGame;
	public string level; // Saved level
	public float playerStartPositionX = 0.0f; // Position on the X-axis at which the player starts when loading in
	public float playerStartPositionY = 0.0f; // Position on the Y-axis at which the player starts when loading in
	public float cameraStartPositionX = 0.0f; // Position on the X-axis at which the camera starts when loading in
	//public Sprite levelImage;
	public System.DateTime dateTime; // Date and time
	public float playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

	public List<Item> items = new List<Item>();

	public Game()
	{
		newGame = true;
		level = "Level1";
		cameraStartPositionX = 0.0f;
	}

	public void PrintGameVariables()
	{
		Debug.Log("Level: " + level +
			" | Savepoint: " + dateTime +
			" | Date and Time: " + dateTime +
			" | Played Time: " + playedTime +
			" | Played Hours: " + hours +
			" | Played Minutes: " + minutes +
			" | Played Seconds: " + seconds);
	}
}
