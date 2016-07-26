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
    public KeyValuePair<float, float> spawnPosition;

    //public List<bool> eventFlags;
    public Dictionary<NpcBehaviour.Type, int> triggeredEvents;
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
        spawnPosition = new KeyValuePair<float, float>(0.0f, 0.0f);

        //eventFlags = new List<bool>();
        triggeredEvents = new Dictionary<NpcBehaviour.Type, int>();
        itemsDataScene = new List<ItemData>();
        itemsDataInventory = new List<ItemData>();

        dateTime = System.DateTime.Now;
        playedTime = 0.0f;
        seconds = 0;
        minutes = 0;
        hours = 0;
	}

    public bool AddToTriggeredEvents(NpcBehaviour.Type eventType, int value = 0)
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
