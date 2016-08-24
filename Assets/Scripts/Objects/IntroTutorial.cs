using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroTutorial : MonoBehaviour
{
    public static bool isTutorialActive;
    public CanvasRenderer tuto1Img, tuto2Img;
    public GameObject child;

    float opacity1, opacity2;
    int state;
    bool childDestroyed;

    void Awake()
    {
        child = transform.GetChild(0).gameObject;
        tuto1Img = transform.GetChild(0).GetComponent<CanvasRenderer>();
        tuto2Img = transform.GetChild(0).transform.GetChild(0).GetComponent<CanvasRenderer>();
    }

	void Update ()
    {
        if (child.activeSelf)
        {
            isTutorialActive = true;

            if (state == 0)
            {
                PlayerController.current.hud.SetHud(false);
                PlayerController.current.canMove = false;
                opacity2 += 1.0f * Time.deltaTime;
                tuto1Img.SetAlpha(opacity2);
                tuto2Img.SetAlpha(opacity1);
            }
            else if (state == 1)
            {
                opacity1 += 1.0f * Time.deltaTime;
                tuto1Img.SetAlpha(opacity2);
                tuto2Img.SetAlpha(opacity1);
                if (opacity1 >= 1.0f)
                {
                    opacity2 -= 1.0f * Time.deltaTime;
                }
            }
            else if (state == 2)
            {
                opacity1 -= 1.0f * Time.deltaTime;
                tuto1Img.SetAlpha(opacity1);
                tuto2Img.SetAlpha(opacity1);

                if (opacity1 <= 0.0f)
                {
                    isTutorialActive = false;
                    PlayerController.current.hud.SetHud(true);
                    PlayerController.current.canMove = true;
                    child.gameObject.SetActive(false);
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

            //Touch touch = Input.GetTouch(0);

            if (Input.GetMouseButtonDown(0) /* || touch.phase == TouchPhase.Began */)
            {
                if (!childDestroyed)
                {
                    state = 1;
                    childDestroyed = true;
                }
                else
                {
                    state = 2;
                }
            }
        }
    }

    public void ResetValues()
    {
        state = 0;
        tuto1Img.SetAlpha(0.0f);
        tuto2Img.SetAlpha(0.0f);
        opacity1 = 0.0f;
        opacity2 = 0.0f;
    }
}
