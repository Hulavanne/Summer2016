using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour
{

    //Statistics st;
    // Use this for initialization
    void Start()
    {
        //st = FindObjectOfType<Statistics>();
    }
    public void GoToLast()
    {
        /*st.ResetScore();
        int level = st.GetCurrentLevel();
        // very evil patch for endless mode: returns "infinite" if level id is 99...
        string levelname = st.GetLevelNameAsString(level);
        GameObject.FindWithTag("statistics").GetComponent<LoadScreen>().showLoader(levelname,level);
        Time.timeScale = 1.0f;
        PauseGame.gamePaused = false;
        Debug.Log(levelname);
        Application.LoadLevel(levelname);*/
    }

}
