using UnityEngine;
using System.Collections;

//[System.Serializable]
public class Savepoint : MonoBehaviour
{
	MenuController menuController;
	GameManager gameManager;
    PlayerController player;
    GameObject textManager;

	bool isOverlappingPlayer;

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

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        textManager = GameObject.Find("TextBoxManager");
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
            gameManager.collidingSavepoint = transform.gameObject;
			textManager.GetComponent<TextBoxManager>().DisableTextBox();
            menuController.OpenLoadMenu();
        }
    }
}
