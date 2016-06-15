using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	GameObject pauseOverlay;
	public static bool gamePaused = false;

	//public string SceneToGo;
	//public int levelId;
    
	void Awake ()
    {
		Time.timeScale = 1;
		pauseOverlay = transform.FindChild("PauseScreen").gameObject;
        pauseOverlay.SetActive(false);
	}

	public void GoToScene(string sceneName)
	{
		//loader.showLoader(SceneToGo, levelId);
		SceneManager.LoadScene(sceneName);
	}

	public void PauseGame()
	{
		gamePaused = true;
		pauseOverlay.SetActive(true);
		Time.timeScale = 0;
	}

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseOverlay.SetActive(false);
        gamePaused = false;
    }

	public void PlaySoundEffect()
	{

	}

	public void ToggleMute()
	{

	}

	public void SetMasterVolume()
	{

	}

	public void SetMusicVolume()
	{

	}

	public void SetSFXVolume()
	{

	}
}
