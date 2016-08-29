using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager current;

    // Sound effect clips
    public AudioClip[] walkingSoundsSolid;
    public AudioClip[] walkingSoundsLeaves;
    public AudioClip[] walkingSoundsSlush;
    public AudioClip[] runningSoundsSolid;
    public AudioClip[] runningSoundsLeaves;
    public AudioClip[] runningSoundsSlush;
    public AudioClip doorCreakSound;
    public AudioClip leafRustleSound;
    public AudioClip cavernDoorSound;
    public AudioClip witchBreathingSound;

    // Sound effect sources
    [HideInInspector]
    public AudioSource footstepsSource;
    [HideInInspector]
    public AudioSource actionSource;

	void Awake()
    {
        current = this;

        footstepsSource = AddAudio(gameObject, walkingSoundsSolid[0], false, false, 1.0f);
        actionSource = AddAudio(gameObject, doorCreakSound, false, false, 1.0f);

        footstepsSource.mute = true;
        actionSource.mute = true;
	}

    public static AudioSource AddAudio(GameObject sourceParent, AudioClip clip, bool loop, bool playAwake, float vol)
    { 
        AudioSource newAudio = sourceParent.AddComponent<AudioSource>();

        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        if (newAudio.playOnAwake)
        {
            newAudio.Play();
        }

        return newAudio; 
    }

    public void PlaySoundEffect(AudioClip clip, AudioSource source)
    {
        // Play the clip
        source.clip = clip;
        source.Play();
    }

    public void LoopSoundEffect(AudioClip clip, AudioSource source)
    {
        // Play the clip
        source.clip = clip;
        source.loop = true;
        source.Play();
	}

    public void StartRandomSoundEffectsLoop(AudioClip[] clips, AudioSource source)
    {
        for (int i = 0; i < AudioManager.current.effectsSources.Count; ++i)
        {
            if (AudioManager.current.effectsSources[i] == source)
            {
                if (!AudioManager.current.loopingEffectsSources[i] && !source.isPlaying)
                {
                    StartCoroutine(LoopRandomSoundEffects(clips, source));
                }

                return;
            }
        }
    }

    IEnumerator LoopRandomSoundEffects(AudioClip[] clips, AudioSource source)
    {
        source.loop = true;

        // As long as this source is marked as looping, the sound effects randomizer will continue
        while (source.loop)
        {
            // Get a random clip and play it
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();

            // Set the looping boolean to true for this audio source
            for (int i = 0; i < AudioManager.current.effectsSources.Count; ++i)
            {
                if (AudioManager.current.effectsSources[i] == source)
                {
                    AudioManager.current.loopingEffectsSources[i] = true;
                    break;
                }
            }

            // Wait for the clip to finish
            yield return new WaitForSeconds(source.clip.length);
        }

        // Set the looping boolean to false for this audio source
        for (int i = 0; i < AudioManager.current.effectsSources.Count; ++i)
        {
            if (AudioManager.current.effectsSources[i] == source)
            {
                AudioManager.current.loopingEffectsSources[i] = false;
                break;
            }
        }

        // Stop the audio source from playing
        source.Stop();
    }

    public void StopLoop(AudioSource source, bool fullStop)
    {
        // This will make the source stop playing at the end of the current sound effect
        source.loop = false;

        if (fullStop)
        {
            source.Stop();
        }
    }
}
