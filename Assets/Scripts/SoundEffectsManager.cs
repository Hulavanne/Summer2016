using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager current;

    // Sound effect clips
    public AudioClip[] footstepSoundsFloor;
    public AudioClip[] footstepSoundsLeaves;
    public AudioClip doorCreakSound;
    public AudioClip leafRustleSound;
    public AudioClip cavernDoorSound;

    // Sound effect sources
    [HideInInspector]
    public AudioSource footstepsSource;
    [HideInInspector]
    public AudioSource actionSource;

	void Awake()
    {
        current = this;

        footstepsSource = AddAudio(gameObject, footstepSoundsFloor[0], false, false, 1.0f);
        actionSource = AddAudio(gameObject, doorCreakSound, false, false, 1.0f);
	}

    public static AudioSource AddAudio(GameObject sourceParent, AudioClip clip, bool loop, bool playAwake, float vol)
    { 
        AudioSource newAudio = sourceParent.AddComponent<AudioSource>();

        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

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
        if (!source.isPlaying)
        {
            StartCoroutine(LoopRandomSoundEffects(clips, source));
        }
    }

    IEnumerator LoopRandomSoundEffects(AudioClip[] clips, AudioSource source)
    {
        source.loop = true;

        while (source.loop)
        {
            // Get a random clip and play it
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();

            yield return new WaitForSeconds(source.clip.length);
        }
    }

    public void StopLoop(AudioSource source)
    {
        source.loop = false;
    }
}
