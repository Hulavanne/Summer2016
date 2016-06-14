using UnityEngine;
using System.Collections;

public class StartNext : MonoBehaviour {

    // Start game level that comes after last played level
	
    //Statistics st;
   
    void Start()
    {
        //st = FindObjectOfType<Statistics>();
        //if (st.GetCurrentLevel() >= 6)
        //    this.gameObject.SetActive(false);
    }

    public void ChangeToNextScene()
    {
        /*st.ResetScore();
        int level = st.GetCurrentLevel() + 1;
        string levelname = st.GetLevelNameAsString(level);
        Debug.Log("starting level " + levelname);
        GameObject.FindWithTag("statistics").GetComponent<LoadScreen>().showLoader(levelname, level);
        Application.LoadLevel(levelname);*/
    }
}
