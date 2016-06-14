using UnityEngine;
using System.Collections;

public class AchievementsButton : MonoBehaviour {
	
    void Awake()
    {
#if UNITY_ANDROID
        //gps = FindObjectOfType<GooglePlayServices>();
        //if (gps == null)
        //{
        //    Destroy(gameObject);
        //}
#else
        Destroy(gameObject);
#endif
    }

    public void ShowAchievements()
    {
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                Social.ShowAchievementsUI();
            }
        });
    }
}
