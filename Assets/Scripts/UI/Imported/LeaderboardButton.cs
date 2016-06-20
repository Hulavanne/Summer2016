using UnityEngine;
using System.Collections;

public class LeaderboardButton : MonoBehaviour {

    //GooglePlayServices gps;
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
    public void ShowLeaderboard()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Social.ShowLeaderboardUI();
            }
        });
    }
}
