using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsControl : MonoBehaviour
{
    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteBox;
    VolControl globalVol;

    GameObject overlay;
    //Health hpGUI;
    
	void Start ()
    {
		overlay = GameObject.Find("PauseScreen");
        //if (!(overlay = GameObject.Find("PauseScreen")))
        //    overlay = GameObject.Find("volumes");

        /*if (globalVol = FindObjectOfType<VolControl>()) { }
        else Debug.Log("volume control not found!");
        //if (hpGUI = FindObjectOfType<Health>())
        //    Debug.Log("pause control found health");
        musicSlider.value = globalVol.musicMaxVol;
        sfxSlider.value = globalVol.effectVol;
        mainSlider.value = globalVol.masterVol;
        muteBox.isOn = globalVol.isMute;*/

        overlay.SetActive(false);
	}

    public void SetMusicVolume()
    {
        //globalVol.SetMusicVolume(musicSlider.value);
    }
    public void SetSFXVolume()
    {
        //globalVol.SetSFXVolume(sfxSlider.value);
    }
    public void PlaySoundEffect()
    {   
        //if (globalVol)
        //    globalVol.PlayButtonEffect();
        //else 
        //{
            Debug.Log("volumeControl missing");
        //}
    }
    public void SetMasterVolume()
    {
        //globalVol.SetMasterVolume(mainSlider.value);
        muteBox.isOn = false;
    }
    public void ResumeGame()
    {
        //if (hpGUI != null)
        //{
            //if (hpGUI.IsAlive())
            //{
            //    Time.timeScale = 1;
            //    overlay.SetActive(false);
            //}
        //}
        //else
        //{
            Time.timeScale = 1;
            overlay.SetActive(false);
        //}
        PauseGame.gamePaused = false;
    }
    public void MuteAll()
    {
        //globalVol.MuteAudio(muteBox.isOn);
    }

    public void DeleteSave()
    {
        //Statistics stats;
        //if (stats = GameObject.FindObjectOfType<Statistics>())
        //    stats.GetComponent<DataSave>().ClearSlot();
    }
    public void LockPlanets()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("planetButton");
        foreach (GameObject p in planets)
        {
            //p.GetComponent<LinearPlanet>().lockLevel();
        }
    }
}
