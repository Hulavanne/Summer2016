using UnityEngine;
using System.Collections;

//[System.Serializable]
public class Savepoint : MonoBehaviour
{
	MenuController menuController;
    PlayerController player;
    GameObject textManager;

	bool isOverlappingPlayer;

	void Awake()
	{
		if (GameObject.Find("InGameUI") != null)
		{
			menuController = GameObject.Find("InGameUI").gameObject.GetComponent<MenuController>();
		}
		if (GameObject.Find("Player").GetComponent<PlayerController>() != null)
		{
			player = GameObject.Find("Player").GetComponent<PlayerController>();

		}
		if (GameObject.Find("TextBoxManager"))
		{
			textManager = GameObject.Find("TextBoxManager");
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
			GameManager.instance.collidingSavepoint = transform.gameObject;
			textManager.GetComponent<TextBoxManager>().DisableTextBox();
            menuController.OpenLoadMenu();
        }
    }
}
