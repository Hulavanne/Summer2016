using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroTutorial : MonoBehaviour {

    bool childDestroyed;
    public CanvasRenderer tuto1Img, tuto2Img;
    float opacity1, opacity2;
    int state;

    void Start () {
        state = 0;
        Time.timeScale = 0.0f;
        tuto2Img = GetComponent<CanvasRenderer>();
        tuto1Img = transform.GetChild(0).GetComponent<CanvasRenderer>();
        tuto2Img.SetAlpha(0.0f);
        tuto1Img.SetAlpha(0.0f);
        opacity1 = 0.0f;
        opacity2 = 0.0f;
	}

	void Update () {
        if (state == 0)
        {
            opacity2 += 0.022f;
            tuto2Img.SetAlpha(opacity2);
            tuto1Img.SetAlpha(opacity1);
        }
        else if (state == 1)
        {
            opacity2 -= 0.022f;
            opacity1 += 0.022f;
            tuto2Img.SetAlpha(opacity2);
            tuto1Img.SetAlpha(opacity1);
        }
        else if (state == 2)
        {
            opacity1 -= 0.022f;
            tuto1Img.SetAlpha(opacity1);
            Time.timeScale = 1.0f;

            if (opacity1 <= 0.0f)
            {
                Destroy(gameObject);
            }
        }

        if (opacity1 < 0)
        {
            opacity1 = 0;
        }
        else if (opacity1 > 1)
        {
            opacity1 = 1;
        }
        else if (opacity2 < 0)
        {
            opacity2 = 0;
        }
        else if (opacity2 > 1)
        {
            opacity2 = 1;
        }

        Touch touch = Input.GetTouch(0);

        if (Input.GetMouseButtonDown(0) || touch.phase == TouchPhase.Began)
        {
            if (!childDestroyed)
            {
                state = 1;
                childDestroyed = true;
            }
            else
            {
                state = 2;
                Time.timeScale = 1.0f;
            }
        }
    }
}
