using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreen : MonoBehaviour {

    public GameObject screenImage;
    string nextlevel;
    int nextlvlNumber;
    VolControl sound;
    bool loadingDone;
    public bool isMenu;

	void Start()
    {
        sound = GetComponent<VolControl>();
        loadingDone = true;
        isMenu = true;
	}

	void Update () 
    {
        if (!loadingDone)
        {
            if (Application.loadedLevelName == nextlevel && !Application.isLoadingLevel)
            {
                sound = GetComponent<VolControl>();
                hideLoader();                
                Time.timeScale = 1;
                loadingDone = true;
            }
        }
	}

    public void showLoader(string leveltoload, int levelId)
    {
        loadingDone = false;
        nextlevel = leveltoload;
        nextlvlNumber = levelId;
        isMenu = (levelId == 0);
        sound.SetMusicType(isMenu, nextlvlNumber);
        if (!isMenu)
        screenImage.SetActive(true);       
    }

    public void hideLoader()
    {
        screenImage.SetActive(false);
    }
}
