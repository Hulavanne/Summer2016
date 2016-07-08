using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssetManager : MonoBehaviour
{
    List<GameObject> buttonListG = new List<GameObject>();
    List<Button> buttonList = new List<Button>();

    public static AssetManager current;

    #region buttons

    public Button optButton1;
    public Button optButton2;
    public Button optButton3;
    public Button optButton4;

    public GameObject optButton1G;
    public GameObject optButton2G;
    public GameObject optButton3G;
    public GameObject optButton4G;

    #endregion

    

    void Awake()
    {
        current = this;

        for (int i = 0; i < GameObject.Find("OptButtons").transform.childCount; ++i)
        {
            buttonListG.Add(GameObject.Find("OptButtons").transform.GetChild(i).gameObject);
            buttonList.Add(buttonListG[i].GetComponent<Button>());
        }
    }
}