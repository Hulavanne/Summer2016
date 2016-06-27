using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour
{
	void Awake()
	{
		
	}

	void Update()
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("Before Save:");

			for (int i = 0; i < SavingAndLoading.savedGames.Count; ++i)
			{
				SavingAndLoading.savedGames[i].PrintGameVariables();
			}

			SavingAndLoading.SaveGame();

			Debug.Log("After Save:");

			for (int i = 0; i < SavingAndLoading.savedGames.Count; ++i)
			{
				SavingAndLoading.savedGames[i].PrintGameVariables();
			}
		}
	}
}
