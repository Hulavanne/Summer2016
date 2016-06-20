using UnityEngine;
using System.Collections;

public class AndroidQuit : MonoBehaviour {
	void Update () {
#if UNITY_ANDROID
	    if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
	}
}
