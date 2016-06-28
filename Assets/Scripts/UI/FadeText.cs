using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeText : MonoBehaviour
{
	Text text;
	float timer = 0.0f;
	float fadeStartTime = 0.5f;
	float fadeTime = 0.1f;

	void Awake()
	{
		text = transform.GetComponent<Text>();
	}

	void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			text.CrossFadeColor(new Color(text.color.r, text.color.g, text.color.b, 0), fadeTime, false, true);
			timer = 0.0f;
		}
	}

	public void StartTimer()
	{
		timer = fadeStartTime;
	}
}
