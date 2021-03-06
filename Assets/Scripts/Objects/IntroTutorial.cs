﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroTutorial : MonoBehaviour
{
    public static bool isTutorialActive;
    public CanvasRenderer tuto1Img, tuto2Img;
    public GameObject child;

    bool exception;
    float opacity1, opacity2;
    public float timer;
    public bool childDestroyed, showing;
    public static bool doesShow1, doesShow2;

    void Awake()
    {
        exception = true;
        tuto1Img = transform.GetChild(0).GetComponent<CanvasRenderer>();
        tuto2Img = transform.GetChild(1).GetComponent<CanvasRenderer>();
    }

	void Update ()
    {
        if (doesShow1)
        {
            timer += 1.0f * Time.deltaTime;
            showing = true;
            PlayerController.current.hud.SetHud(false);
            PlayerController.current.canMove = false;
            opacity1 += 1.0f * Time.deltaTime;
            tuto1Img.SetAlpha(opacity1);
        }
        else if (doesShow2)
        {
            showing = true;
            PlayerController.current.hud.SetHud(false);
            PlayerController.current.canMove = false;
            opacity2 += 1.0f * Time.deltaTime;
            tuto2Img.SetAlpha(opacity2);
        }
        else
        {
            if (showing)
            {
                PlayerController.current.hud.SetHud(true);
                PlayerController.current.canMove = true;
                showing = false;
            }

            opacity1 -= 1.0f * Time.deltaTime;
            tuto1Img.SetAlpha(opacity1);
            opacity2 -= 1.0f * Time.deltaTime;
            tuto2Img.SetAlpha(opacity2);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                if (doesShow1)
                {
                    if (timer <= 2.0f)
                    {
                        return;
                    }
                    doesShow1 = false;
                }
                if (doesShow2)
                {
                    doesShow2 = false;
                }
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

        if (Input.GetMouseButtonUp(0))
        {
            if (doesShow1)
            {
                if (timer <= 2.0f)
                {
                    return;
                }
                doesShow1 = false;
            }
            if (doesShow2)
            {
                doesShow2 = false;
            }
        }
    }

    public void ResetValues()
    {
        doesShow1 = false;
        doesShow2 = false;
        tuto1Img.SetAlpha(0.0f);
        tuto2Img.SetAlpha(0.0f);
        opacity1 = 0.0f;
        opacity2 = 0.0f;
    }
}
