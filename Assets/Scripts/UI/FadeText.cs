using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeText : MonoBehaviour
{
	public float textVisibleTime = 0.5f; // How long the text will be fully visible before starting to fade
	public float textFadeTime = 0.2f; // How long it takes for the text to fade away

	float visibleTimer = 0.0f; // For how long the text will still be fully visible
	float fadePercentage = 100.0f; // How much the text has faded

	Text text;
	Color32 visibleColor;
	Color32 fadedColor;

	void Awake()
	{
		text = transform.GetComponent<Text>();
		visibleColor = text.color;
		fadedColor = new Color32(visibleColor.r, visibleColor.g, visibleColor.b, 0);
	}

	void Update()
	{
		visibleTimer -= Time.deltaTime;

		// If the visibleTimer has reached zero, start fading the text
		if (visibleTimer <= 0.0f)
		{
			visibleTimer = 0.0f;

			// If fadeTimer has not reached 100% yet
			if (fadePercentage <= 100.0f)
			{
				// Update fading timer by adding delta time divided by the desired fade time to it
				fadePercentage += 100.0f * Time.deltaTime / textFadeTime;
				// Lerp the color of the text from fully visible towards faded
				text.color = Color32.Lerp(visibleColor, fadedColor, fadePercentage / 100.0f);
			}
		}
	}

	public void StartTimer()
	{
		text.color = visibleColor;
		visibleTimer = textVisibleTime;
		fadePercentage = 0.0f;
	}
}
