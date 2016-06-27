using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{
	public static Game current; // Current game
	public static int currentIndex; // Current game's index in the saved games list

	public string level;
	//public Sprite image;
	public System.DateTime dateTime;// Date and time
	public float playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

	public Game()
	{
		level = "Test";
	}

	public void PrintGameVariables()
	{
		Debug.Log("Level: " + level);
		Debug.Log("Date and Time: " + dateTime);
		Debug.Log("Played Time: " + playedTime);
		Debug.Log("Played Seconds: " + seconds);
		Debug.Log("Played Minutes: " + minutes);
		Debug.Log("Played Hours: " + hours);
	}
}
