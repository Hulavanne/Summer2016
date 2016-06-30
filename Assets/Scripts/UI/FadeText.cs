using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeText : MonoBehaviour
{
	public float fadeTime = 5.0f;

	Text text;
	Color visibleColor;
	Color fadedColor;

	float timer = 1.0f;
	float timerFull;

	void Awake()
	{
		text = transform.GetComponent<Text>();
		visibleColor = new Color(text.color.r, text.color.g, text.color.b, 255);
		fadedColor = new Color(visibleColor.r, visibleColor.g, visibleColor.b, 0);
		timerFull = timer;
	}

	void Update()
	{
		// if end color not reached yet
		if (timer <= timerFull)
		{
			timer += Time.deltaTime / fadeTime; // advance timer at the right speed
			text.color = Color.Lerp(visibleColor, fadedColor, timer);
		}


		//Debug.Log(timer);
	}

	public void StartTimer()
	{
		text.color = visibleColor;
		timer = 0.0f;
		//showTextTimer = showTextTimerFull;
		//fadeTimer = fadeTimerFull;
	}
}
