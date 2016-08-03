using UnityEngine;
using System.Collections;

public class Android : MonoBehaviour
{
	static void Awake()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        Debug.Log("DONE");
	}
}
