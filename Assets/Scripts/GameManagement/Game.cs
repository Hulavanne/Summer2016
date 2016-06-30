using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game
{
	public static Game current; // Current game
	public static int currentIndex; // Current game's index in the saved games list

	public string level; // Saved level
	public float startingPositionX = 0.0f; // Position on the X-axis at which the player start when loading in
	public float startingPositionY = 0.0f; // Position on the Y-axis at which the player start when loading in
	//public Sprite levelImage;
	public System.DateTime dateTime; // Date and time
	public float playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

	public List<Item> items = new List<Item>();

	public Game()
	{
		// The first level of the game
		level = "MainScene";
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
