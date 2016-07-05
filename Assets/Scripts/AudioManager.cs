using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance = null;

	public static float masterVolume = 1.0f;
	public static float soundEffectsVolume = 1.0f;
	public static float musicVolume = 1.0f;

	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

	// Sound effects
	public AudioClip buttonSoundEffect;

	// Music
	public AudioClip mainMenuMusic;

	AudioSource effectsSource;
	AudioSource musicSource;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		effectsSource = transform.GetComponentsInChildren<AudioSource>()[0];
		musicSource = transform.GetComponentsInChildren<AudioSource>()[1];
	}

	void Update()
	{
		effectsSource.volume = soundEffectsVolume * masterVolume;
		musicSource.volume = musicVolume * masterVolume;
	}

	public void PlaySingle(AudioClip clip)
	{
		effectsSource.clip = clip;
		effectsSource.Play();
	}

	public void PlayRandomizedSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		effectsSource.pitch = randomPitch;
		effectsSource.clip = clips[randomIndex];
		effectsSource.Play();
	}
}
