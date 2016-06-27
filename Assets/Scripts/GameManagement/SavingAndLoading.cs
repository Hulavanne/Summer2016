using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public static class SavingAndLoading
{
	public static List<Game> savedGames = new List<Game>();
	public const string SAVE_PATH = "/SavedGames.memories";

	public static void SaveGame()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + SAVE_PATH);
		binaryFormatter.Serialize(file, SavingAndLoading.savedGames);

		file.Close();
	}

	public static void LoadSavedGames()
	{
		if (File.Exists(Application.persistentDataPath + SAVE_PATH))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + SAVE_PATH, FileMode.Open);
			SavingAndLoading.savedGames = (List<Game>)binaryFormatter.Deserialize(file);

			file.Close();
		}
	}
}