using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Savepoint : MonoBehaviour
{
    public MenuController menuController;
    public PlayerController player;
    public TextBoxManager textManager;

	bool isOverlappingPlayer;

	void Awake()
	{
        #if UNITY_EDITOR

        if (transform.GetComponent<UniqueId>() == null)
        {
            gameObject.AddComponent<UniqueId>();
        }

        #endif

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
