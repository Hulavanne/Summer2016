using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Instructions : MonoBehaviour {

    public GameObject[] tutPics;
    GameObject current;
    int num = 0;

    void Awake()
    {
        current = tutPics[num];
        current.SetActive(true);
    }
    public void Next(bool forward)
    {
        current.SetActive(false);
        if (forward)
            num++;
        else num--;

        if (num >= tutPics.Length)
            num = 0;
        else if (num < 0)
            num = tutPics.Length - 1;
        current = tutPics[num];
        current.SetActive(true);
    }
}
