using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{
	public static Game current;

	public string level;

	public Game()
	{
		level = "MainScene";
	}
}
