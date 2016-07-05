using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance = null;

	public static bool audioMuted = false;
	public static float masterVolume = 1.0f;
	public static float soundEffectsVolume = 1.0f;
	public static float musicVolume = 1.0f;

	public float lowPitchRange = 0.9f;
	public float highPitchRange = 1.1f;

	AudioSource effectsSource;
	AudioSource musicSource;

	void Awake()
	{
		// Get a static reference to this game object
		if (instance == null)
		{
			instance = this;
		}
		// Making sure there is only a single AudioManager in the scene
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		effectsSource = transform.GetComponentsInChildren<AudioSource>()[0];
		musicSource = transform.GetComponentsInChildren<AudioSource>()[1];

		// Load audio values
		LoadAudioSettings();
	}

	void Update()
	{
		
	}

	public void PlaySoundEffect(AudioClip clip)
	{
		// Play the clip
		effectsSource.clip = clip;
		effectsSource.Play();
	}

	public void PlayRandomizedSoundEffect(params AudioClip[] clips)
	{
		// Get a random pitch and a random clip
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		int randomIndex = Random.Range (0, clips.Length);

		// Play the clip
		effectsSource.pitch = randomPitch;
		effectsSource.clip = clips[randomIndex];
		effectsSource.Play();
	}

	public void PlayPitchedSoundEffect(AudioClip clip, float pitchMin, float pitchMax)
	{
		// Pick a random value for the pitch between the given values, that are clamped between -3 and 3
		float randomPitch = Random.Range(Mathf.Clamp(pitchMin, -3.0f, 3.0f), Mathf.Clamp(pitchMax, -3.0f, 3.0f));

		// Play the clip
		effectsSource.pitch = randomPitch;
		effectsSource.clip = clip;
		effectsSource.Play();
	}

	public void LoadAudioSettings()
	{
		AudioManager.audioMuted = System.Convert.ToBoolean(PlayerPrefs.GetInt("AudioMuted", 0));
		AudioManager.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
		AudioManager.soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);
		AudioManager.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

		// Update mute audio
		effectsSource.mute = AudioManager.audioMuted;
		musicSource.mute = AudioManager.audioMuted;

		// Update volumes
		SetMasterVolume(AudioManager.masterVolume);
		SetMusicVolume(AudioManager.soundEffectsVolume);
		SetSoundEffectsVolume(AudioManager.musicVolume);
	}

	public void SaveAudioSettings()
	{
		PlayerPrefs.SetInt("AudioMuted", System.Convert.ToInt32(AudioManager.audioMuted));
		PlayerPrefs.SetFloat("MasterVolume", AudioManager.masterVolume);
		PlayerPrefs.SetFloat("SoundEffectsVolume", AudioManager.soundEffectsVolume);
		PlayerPrefs.SetFloat("MusicVolume", AudioManager.musicVolume);

		PlayerPrefs.Save();
	}

	public void ToggleMute()
	{
		Debug.Log("Mute Toggled to " + !AudioManager.audioMuted);
		AudioManager.audioMuted = !AudioManager.audioMuted;

		// Update mute audio
		effectsSource.mute = AudioManager.audioMuted;
		musicSource.mute = AudioManager.audioMuted;
	}

	public void SetMasterVolume(float newValue)
	{
		Debug.Log("Master Volume Changed");
		AudioManager.masterVolume = newValue;

		// Update volumes
		effectsSource.volume = AudioManager.soundEffectsVolume * AudioManager.masterVolume;
		musicSource.volume = AudioManager.musicVolume * AudioManager.masterVolume;
	}

	public void SetMusicVolume(float newValue)
	{
		Debug.Log("Music Volume Changed");
		AudioManager.musicVolume = newValue;

		// Update volume
		musicSource.volume = AudioManager.musicVolume * AudioManager.masterVolume;
	}

	public void SetSoundEffectsVolume(float newValue)
	{
		Debug.Log("Sound Effects Volume Changed");
		AudioManager.soundEffectsVolume = newValue;

		// Update volume
		effectsSource.volume = AudioManager.soundEffectsVolume * AudioManager.masterVolume;
	}
}
