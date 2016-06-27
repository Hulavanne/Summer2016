using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour
{
	void Awake()
	{
		Game.current = new Game();
	}

	void Update()
	{
		
	}

	void SaveGame()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			//SavingAndLoading.Save();

			//Game.current.testString1 = "asd";
			//Game.current.testString2 = "asd";
			//Game.current.testString3 = "asd";

			//SavingAndLoading.Load();

			for (int i = 0; i < SavingAndLoading.savedGames.Count; ++i)
			{
				//Debug.Log(SavingAndLoading.savedGames[i].testString1);
				//Debug.Log(SavingAndLoading.savedGames[i].testString2);
				//Debug.Log(SavingAndLoading.savedGames[i].testString3);
			}
		}
	}
}
