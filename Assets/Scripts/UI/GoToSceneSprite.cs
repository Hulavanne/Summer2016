using UnityEngine;
using System.Collections;

public class GoToSceneSprite : MonoBehaviour
{
    void Start()
    {
        Invoke("startgame", 1.5f);
    }
    void startgame()
    {        
        Application.LoadLevel("StartMenu");
    }
}