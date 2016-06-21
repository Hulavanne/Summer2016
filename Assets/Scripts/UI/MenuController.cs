using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public static bool gamePaused = false;
	//public string SceneToGo;
	//public int levelId;

	InventoryManager inventoryManager;
	GameObject pauseOverlay;
	GameObject optionsOverlay;
	GameObject menu;
    
	void Awake ()
    {
		Time.timeScale = 1;

		if (transform.GetComponentInChildren<InventoryManager>() != null)
		{
			inventoryManager = transform.GetComponentInChildren<InventoryManager>();
		}

		if (transform.FindChild("PauseScreen") != null)
		{
			pauseOverlay = transform.FindChild("PauseScreen").gameObject;
			pauseOverlay.SetActive(false);
		}

		if (transform.name != "OptionsOverlay")
		{
			menu = transform.gameObject;

			if (GameObject.Find("OptionsOverlay") != null)
			{
				optionsOverlay = GameObject.Find("OptionsOverlay");
				optionsOverlay.GetComponent<MenuController>().menu = menu;
				optionsOverlay.SetActive(false);
			}
		}
		else
		{
			optionsOverlay = transform.gameObject;
		}
	}

	public void GoToScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void PauseGame()
	{
		gamePaused = true;
		pauseOverlay.SetActive(true);
		Time.timeScale = 0;

		inventoryManager.FillItemSlots();
	}

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseOverlay.SetActive(false);
        gamePaused = false;
    }

	public void InspectItem(GameObject item)
	{

	}

	public void ActivateOptionsOverlay()
	{
		optionsOverlay.SetActive(true);
		menu.SetActive(false);
	}

	public void DectivateOptionsOverlay()
	{
		menu.SetActive(true);
		optionsOverlay.SetActive(false);
	}

	public void PlaySoundEffect()
	{
		Debug.Log("Playing Button Sound Effect");
	}

	//OPTIONS

	public void ToggleMute()
	{
		Debug.Log("Mute Toggled");
	}

	public void SetMasterVolume()
	{
		Debug.Log("Master Volume Changed");
	}

	public void SetMusicVolume()
	{
		Debug.Log("Music Volume Changed");
	}

	public void SetSoundEffectsVolume()
	{
		Debug.Log("Sound Effects Volume Changed");
	}

	public void SetGamma()
	{
		Debug.Log("Gamma Changed");
	}

	public void DisplayCredits()
	{
		Debug.Log("Displaying Credits");
	}
}
