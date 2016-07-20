using UnityEngine;
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

    //public List<bool> eventFlags;
    public Dictionary<EventManager.Events, int> triggeredEvents;
    public List<ItemData> itemsDataScene;
    public List<ItemData> itemsDataInventory;

	public System.DateTime dateTime; // Date and time
    public double playedTime; // Seconds (with decimals) spent in-game
	public int seconds; // Seconds spent in-game
	public int minutes; // Minutes spent in-game
	public int hours; // Hours spent in-game

	public Game()
	{
		newGame = true;
		levelIndex = 0;
        savepointId = "";

        //eventFlags = new List<bool>();
        triggeredEvents = new Dictionary<EventManager.Events, int>();
        itemsDataScene = new List<ItemData>();
        itemsDataInventory = new List<ItemData>();

        dateTime = System.DateTime.Now;
        playedTime = 0.0f;
        seconds = 0;
        minutes = 0;
        hours = 0;
	}

    public bool AddToTriggeredEvents(EventManager.Events eventType, int value = 0)
    {
        if (!triggeredEvents.ContainsKey(eventType))
        {
            triggeredEvents.Add(eventType, value);
            return true;
        }
        else
        {
            return false;
        }
    }
}
