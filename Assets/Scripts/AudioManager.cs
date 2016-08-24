using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	public static AudioManager current = null;

	public static bool audioMuted = false;
	public static float masterVolume = 1.0f;
	public static float soundEffectsVolume = 1.0f;
	public static float musicVolume = 1.0f;
    public static AudioClip nextTrack;

	public float lowPitchRange = 0.9f;
	public float highPitchRange = 1.1f;

    public AudioClip menuMusic;
    public AudioClip gameOverMusic;

    [HideInInspector]
    public AudioSource effectsSource;
    [HideInInspector]
    public AudioSource musicSource;
    [HideInInspector]
    public List<AudioSource> effectsSources = new List<AudioSource>();
    [HideInInspector]
    public List<bool> loopingEffectsSources = new List<bool>();

	void Awake()
	{
		// Get a static reference to this game object
		if (current == null)
		{
			current = this;
		}
		// Making sure there is only a single AudioManager in the scene
		else if (current != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		effectsSource = transform.GetComponentsInChildren<AudioSource>()[0];
		musicSource = transform.GetComponentsInChildren<AudioSource>()[1];
        effectsSources = FindEffectsSources();

        effectsSource.ignoreListenerPause = true;
        musicSource.ignoreListenerPause = true;

		// Load audio values
		LoadAudioSettings();

        // Playing the menu music
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SwitchMusic(menuMusic);
        }
	}

    void Update()
    {
        if (effectsSources != FindEffectsSources())
        {
            effectsSources = FindEffectsSources();
            SetSoundEffectsVolume(soundEffectsVolume);
        }

        UpdateMute();

        // Executed during the first frame of a non-loading scene
        if (GameManager.sceneLoadOperation != null && GameManager.sceneLoadOperation.isDone && SceneManager.GetActiveScene().name != "LoadingScene")
        {
            GameManager.sceneLoadOperation = null;

            StopAllCoroutines();
            effectsSources = FindEffectsSources();

            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                SwitchMusic(menuMusic);
            }
            else
            {
                // Play the music track of the current level
                SwitchMusic(LevelManager.current.levelsList[(int)LevelManager.current.currentLevel].GetComponent<Level>().levelMusic);
            }

            // Load audio values
            LoadAudioSettings();
        }
    }

    public List<AudioSource> FindEffectsSources()
    {
        List<AudioSource> sources = new List<AudioSource>();

        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source != musicSource)
            {
                sources.Add(source);
            }
        }

        if (loopingEffectsSources.Count != sources.Count)
        {
            loopingEffectsSources.Clear();

            foreach (AudioSource source in sources)
            {
                loopingEffectsSources.Add(false);
            }
        }

        return sources;
    }

    // ---SOUND EFFECTS----

	public void PlaySoundEffect(AudioClip clip)
	{
		// Play the clip
		effectsSource.clip = clip;
		effectsSource.Play();
	}

	public void PlayRandomSoundEffect(params AudioClip[] clips)
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

    // -------MUSIC--------

    public void SwitchMusic(AudioClip track)
    {
        if (track != musicSource.clip)
        {
            // Set volume
            SetMusicVolume(musicVolume);

            // Play the new track
            nextTrack = track;
            musicSource.clip = nextTrack;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SwitchMusicGradually(AudioClip track = null, int phase = 0)
    {
        if (track != null)
        {
            nextTrack = track;
        }

        if (phase == 0)
        {
            musicVolume = musicSource.volume;
            // Fade current music
            StartFadeMusic(true, 0.5f, true);
        }
        else if (phase == 1)
        {
            if (nextTrack != null)
            {
                // Play the new track
                musicSource.clip = nextTrack;
                musicSource.Play();
            }
        }
    }

    public void StartFadeMusic(bool fadeOut, float fadeTime, bool fadeBack)
    {
        StartCoroutine(FadeMusic(fadeOut, fadeTime, fadeBack));
    }

    public IEnumerator FadeMusic(bool fadeOut, float fadeTime, bool fadeBack)
    {
        // Portion to be added or subtracted each frame
        float fadePortion = Time.deltaTime / fadeTime * musicVolume;

        // Fading out
        if (fadeOut)
        {
            while (musicSource.volume > 0.0f)
            {
                musicSource.volume -= fadePortion;

                if (musicSource.volume <= 0.0f)
                {
                    musicSource.volume = 0.0f;
                    musicSource.Stop();
                }

                yield return null;
            }
        }
        // Fading in
        else
        {
            musicSource.Play();

            while (musicSource.volume < musicVolume)
            {
                musicSource.volume += fadePortion;

                if (musicSource.volume >= musicVolume)
                {
                    musicSource.volume = musicVolume;
                }

                yield return null;
            }
        }

        if (fadeBack)
        {
            SwitchMusicGradually(null, 1);
            StartFadeMusic(!fadeOut, fadeTime, false);
        }
    }

    // ---AUDIO SETTINGS---

	public void LoadAudioSettings()
	{
        // Get the saved values
		audioMuted = System.Convert.ToBoolean(PlayerPrefs.GetInt("AudioMuted", 0));
		masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
		soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);
		musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

		// Update mute audio
        UpdateMute();

		// Update volumes
		SetMasterVolume(masterVolume);
		SetSoundEffectsVolume(soundEffectsVolume);
		SetMusicVolume(musicVolume);
	}

	public void SaveAudioSettings()
	{
		PlayerPrefs.SetInt("AudioMuted", System.Convert.ToInt32(audioMuted));
		PlayerPrefs.SetFloat("MasterVolume", masterVolume);
		PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolume);
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);

		PlayerPrefs.Save();
	}

	public void ToggleMute()
	{
		audioMuted = !audioMuted;
        UpdateMute();
	}

    public void UpdateMute()
    {
        // Update sound effects mute
        if (effectsSource.volume == 0 || soundEffectsVolume == 0 || masterVolume == 0)
        {
            effectsSource.mute = true;
        }
        else
        {
            effectsSource.mute = audioMuted;
        }

        foreach (AudioSource source in effectsSources)
        {
            source.mute = effectsSource.mute;
        }

        // Update music mute
        if (musicSource.volume == 0 || musicVolume == 0 || masterVolume == 0)
        {
            musicSource.mute = true;
        }
        else
        {
            musicSource.mute = audioMuted;
        }
    }

	public void SetMasterVolume(float newValue)
	{
		masterVolume = newValue;

		// Update volumes
        SetSoundEffectsVolume(soundEffectsVolume);
		musicSource.volume = musicVolume * masterVolume;
	}

    public void SetMusicVolume(float newValue)
	{
		musicVolume = newValue;

		// Update volume
		musicSource.volume = musicVolume * masterVolume;
	}

	public void SetSoundEffectsVolume(float newValue)
	{
		soundEffectsVolume = newValue;

		// Update volumes
        foreach (AudioSource source in effectsSources)
        {
            source.volume = soundEffectsVolume * masterVolume;
        }
	}
}
