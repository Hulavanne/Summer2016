using UnityEngine;
using System.Collections;

//[System.Serializable]
public class Savepoint : MonoBehaviour
{
	MenuController menuController;
	GameManager gameManager;

	bool colliding = false;

	void Awake()
	{
		if (GameObject.Find("InGameUI") != null)
		{
			menuController = GameObject.Find("InGameUI").gameObject.GetComponent<MenuController>();
		}
		if (GameObject.Find("GameManager") != null)
		{
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
	}

	void Update()
	{
		if (colliding)// && Input.GetKeyDown(KeyCode.S))
		{
			MenuController.savingGame = true;
			gameManager.collidingSavepoint = transform.gameObject;
			colliding = false;
			menuController.OpenLoadMenu();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			colliding = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			colliding = false;
		}
	}
}
