using UnityEngine;
using System.Collections;

public class FirstTimeInfo : MonoBehaviour {

    public GameObject info;
    GoToNext gonext;
    VolControl sounds;

    // for testing
    public float showtime;

	void Start () 
    {
        if (gonext = GetComponent<GoToNext>()) {}
        else Debug.Log("FirstTimeInfo: gotonext not found");
        if (sounds = GameObject.FindWithTag("statistics").GetComponent<VolControl>()) { }
        else Debug.Log("FirstTimeInfo: volcontrol not found");
	}

    public void ShowAndGO()
    {
        sounds.PlayButtonEffect();
        if (sounds.tutorialIsOn)
        {
            info.SetActive(true);
            // tutorial only shown once per game session (unless toggled)
            sounds.tutorialIsOn = false;
            //Invoke("Go", showtime);
        }
        else gonext.GoToScene();
    }
    void Go()
    {
        //gonext.Go();
    }
}
