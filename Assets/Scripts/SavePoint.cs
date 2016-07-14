using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UniqueId))]
public class Savepoint : MonoBehaviour
{
	public int savepointIndex = 0;

	MenuController menuController;
    PlayerController player;
    TextBoxManager textManager;

	bool isOverlappingPlayer;

	void Awake()
	{
		if (GameObject.Find("InGameUI") != null)
		{
			menuController = GameObject.Find("InGameUI").GetComponent<MenuController>();
            textManager = GameObject.Find("InGameUI").GetComponent<TextBoxManager>();
		}
		if (GameObject.Find("Player").GetComponent<PlayerController>() != null)
		{
			player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
    }

    void OnTriggerEnter2D()
    {
		TextBoxManager.currentNPC = transform.gameObject;
        isOverlappingPlayer = true;
    }

    void OnTriggerExit2D()
    {
        isOverlappingPlayer = false;
    }

    public void OpenSaveMenu()
    {
        if (isOverlappingPlayer)
        {
            MenuController.savingGame = true;
			GameManager.current.collidingSavepoint = this;
			textManager.DisableTextBox();
            menuController.OpenLoadMenu();
        }
    }
}
