using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatPointUI : MonoBehaviour 
{
    //public Canvas floatingPoints;
    public GameObject pointsImage;
    private Canvas points;
    private GameObject pointImg;
    Vector3 pointPosition;
    //public Vector3 offset;

    int cookieclickcount = 0;
    void Start()
    {
        pointPosition = new Vector3(0, 120, 0);
    }

    public void GeneratePoints(int scoreAmount)
    {
        pointImg = Instantiate(pointsImage, pointPosition, Quaternion.identity) as GameObject;
        pointImg.transform.SetParent(this.transform);
        pointImg.transform.localPosition = pointPosition;
        pointImg.GetComponent<Text>().text = scoreAmount.ToString();
        if (scoreAmount < 0)
            pointImg.GetComponent<Text>().color = Color.red;

    }
    public void GeneratePointImage(int scoreAmount)
    {
        cookieclickcount++;
        pointImg = Instantiate(pointsImage, pointPosition, Quaternion.identity) as GameObject;
        pointImg.transform.SetParent(this.transform);
        pointImg.transform.localPosition = pointPosition;
        pointImg.GetComponent<Text>().text = cookieclickcount.ToString();
    }

}
