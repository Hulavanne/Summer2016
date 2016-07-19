﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game
{
	public static Game current; // Current game
	public static int currentIndex; // Current game's index in the saved games list

	public bool newGame;
	public int levelIndex; // Saved level
    public string savepointId; // Index of the savepoint, -1 if there is none
    public EventFlags eventFlags;
	public float cameraStartPositionX = 0.0f; // Position on the X-axis at which the camera starts when loading in
	public System.DateTime dateTime; // Date and time
    public double playedTime = 0.0f; // Seconds (with decimals) spent in-game
	public int seconds = 0; // Seconds spent in-game
	public int minutes = 0; // Minutes spent in-game
	public int hours = 0; // Hours spent in-game

    public List<ItemData> itemsDataScene = new List<ItemData>();
    public List<ItemData> itemsDataInventory = new List<ItemData>();

	public Game()
	{
		newGame = true;
		levelIndex = 0;
        savepointId = "";
        eventFlags = new EventFlags();
		cameraStartPositionX = 0.0f;
	}

	public void PrintGameVariables()
	{
		Debug.Log("Level: " + levelIndex +
			" | Savepoint: " + dateTime +
			" | Date and Time: " + dateTime +
			" | Played Time: " + playedTime +
			" | Played Hours: " + hours +
			" | Played Minutes: " + minutes +
			" | Played Seconds: " + seconds);
	}
}
