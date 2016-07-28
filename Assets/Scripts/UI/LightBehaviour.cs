using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightBehaviour : MonoBehaviour {

    public static LightBehaviour current;
    public float fadeSpeed;
    public Image darknessImg;

    bool isTurningBlack;
    bool changeOpacity;
    float opacity;
    float targetOpacity;

	void Start () {
        current = this;
        opacity = 0.0f;
        darknessImg = GameObject.Find("Darkness").GetComponent<Image>();
        fadeSpeed = 2;
    }
	
    public void SetLighting(bool turnBlack, float amount)
    {
        isTurningBlack = turnBlack;
        targetOpacity = amount; // can set this to for example 0.5 for 0.5 alpha
        changeOpacity = true;
    }

	void Update ()
    {
        darknessImg.color = new Color (0, 0, 0, opacity);

        if (changeOpacity)
        {
            if (isTurningBlack)
            {
                opacity += fadeSpeed * Time.deltaTime; // fades dark screen
                if (opacity >= targetOpacity)
                {
                    changeOpacity = false;
                }
            }
            else
            {
                opacity -= fadeSpeed * Time.deltaTime; // fades back to scene
                if (opacity <= targetOpacity)
                {
                    changeOpacity = false;
                }
            }
        }
	}
}
