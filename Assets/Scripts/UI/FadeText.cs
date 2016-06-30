using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeText : MonoBehaviour
{
	Text text;

	float showTextTimer = 0.5f;
	float showTextTimerFull;

	float fadeTimer = 0.5f;
	float fadeTimerFull;

	void Awake()
	{
		text = transform.GetComponent<Text>();

		showTextTimerFull = showTextTimer;
		fadeTimerFull = fadeTimer;

		showTextTimer = 0.0f;
		fadeTimer = 0.0f;
	}

	void Update()
	{
		if (showTextTimer > 0.0f)
		{
			showTextTimer -= Time.deltaTime;
		}
		else
		{
			showTextTimer = 0.0f;

			if (fadeTimer > 0.0f)
			{
				fadeTimer -= Time.deltaTime;

				float asd = fadeTimerFull / text.color.a;

				text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 5 * Time.deltaTime);
			}
		}




		//Debug.Log(timer);
	}

	public void StartTimer()
	{
		//text.color = new Color(text.color.r, text.color.g, text.color.b, 255);
		//showTextTimer = showTextTimerFull;
		//fadeTimer = fadeTimerFull;
	}
}
