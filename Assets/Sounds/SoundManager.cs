using UnityEngine;
using System.Collections.Generic;

public class SoundManager : GenericSingleton<SoundManager>
{
    public static readonly string FlashlightOn = "flashlighton";
    public static readonly string Footstep1 = "footstep1";
    public static readonly string Footstep2 = "footstep2";
    public static readonly string Footstep3 = "footstep3";
    public static readonly string OpenDoor = "opendoor";
    public static readonly string CloseDoor = "closedoor";
    public static readonly string PickUp = "pickup";
    public static readonly string OpenLockDoor = "openlockdoor";
    public static readonly string UnlockDoor = "unlockdoor";

    [System.Serializable]
    private class SoundEntry
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource primarySFXSource;
    [SerializeField] private AudioSource secondSFXSource;
    [SerializeField] private AudioSource primaryAmbientSource;
    [SerializeField] private AudioSource secondAmbientSource;

    [Header("SFX")]
    [SerializeField] private SoundEntry[] sfxSounds;

    [Header("Primary Ambient")]
    [SerializeField] private AudioClip[] ambientClips;
    [SerializeField] private float minWaitTime = 10f;
    [SerializeField] private float maxWaitTime = 15f;

    [Header("Secondary Ambient")]
    [SerializeField] private AudioClip passiveAmbientClip;


    private Dictionary<string, SoundEntry> soundLookup;

    protected override void Awake()
    {
        base.Awake();

        soundLookup = new Dictionary<string, SoundEntry>();

        foreach (var sound in sfxSounds)
        {
            if (!string.IsNullOrEmpty(sound.id) && !soundLookup.ContainsKey(sound.id))
            {
                soundLookup.Add(sound.id, sound);
            }
        }
    }

    private void Start()
    {
        CancelInvoke();
        StartPrimaryAmbient();
        StartSecondaryAmbient(passiveAmbientClip, PlayerManager.Instance.PlayerPosition, 0.1f);
    }

    public void PlaySFX(
        string id,
        AudioSource source,
        bool loop = false,
        float? volumeOverride = null,
        float? pitchOverride = null
    )
    {
        if (!soundLookup.TryGetValue(id, out var sound))
            return;

        float pitch = pitchOverride ?? sound.pitch;
        float volume = volumeOverride ?? sound.volume;

        source.clip = sound.clip;
        source.loop = loop;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    public void PlaySFXAtPosition(
        string id,
        Vector3 position,
        bool loop = false,
        float? volumeOverride = null,
        float? pitchOverride = null
    )
    {
        if (!soundLookup.TryGetValue(id, out var sound))
            return;

        float pitch = pitchOverride ?? sound.pitch;
        float volume = volumeOverride ?? sound.volume;

        if (loop)
        {
            Debug.LogWarning("For looping sounds, use PlaySFXOn1st/2nd instead.");
            return;
        }

        // temp obj
        GameObject tempGO = new GameObject($"SFX_{id}");
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = sound.clip;
        tempSource.volume = volume;
        tempSource.pitch = pitch;
        tempSource.loop = false;
        tempSource.spatialBlend = 0f; 
        tempSource.Play();

        Destroy(tempGO, sound.clip.length / pitch + 10f); // divide by pitch so length matches playback
    }


    public void PlaySFXOn1st(string id, Vector3 position, bool loop = false, float? volume = null, float? pitch = null)
    {
        primarySFXSource.transform.position = position;
        PlaySFX(id, primarySFXSource, loop, volume, pitch);
    }

    public void PlaySFXOn2nd(string id, Vector3 position, bool loop = false, float? volume = null, float? pitch = null)
    {
        secondSFXSource.transform.position = position;
        PlaySFX(id, secondSFXSource, loop, volume, pitch);
    }

    public void StopSFXOn1st()
    {
        primarySFXSource.Stop();
        primarySFXSource.loop = false;
    }

    public void StopSFXOn2nd()
    {
        secondSFXSource.Stop();
        secondSFXSource.loop = false;
    }

    public void StartPrimaryAmbient()
    {
        if (ambientClips == null || ambientClips.Length == 0)
            return;

        int index = Random.Range(0, ambientClips.Length);
        PlayPrimaryAmbient(index);

        float delay =
            primaryAmbientSource.clip.length +
            Random.Range(minWaitTime, maxWaitTime);

        Invoke(nameof(StartPrimaryAmbient), delay);
    }

    private void PlayPrimaryAmbient(int index)
    {
        if (index < 0 || index >= ambientClips.Length)
            return;

        primaryAmbientSource.Stop();
        primaryAmbientSource.loop = false;
        primaryAmbientSource.clip = ambientClips[index];
        primaryAmbientSource.transform.position = Vector3.zero;
        primaryAmbientSource.Play();
    }

    public void StopPrimaryAmbient()
    {
        CancelInvoke(nameof(StartPrimaryAmbient));
        primaryAmbientSource.Stop();
    }


    public void StartSecondaryAmbient(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        secondAmbientSource.Stop();
        secondAmbientSource.transform.position = position;
        secondAmbientSource.clip = clip;
        secondAmbientSource.volume = volume;
        secondAmbientSource.loop = true;
        secondAmbientSource.Play();
    }

    public void StopSecondaryAmbient()
    {
        secondAmbientSource.Stop();
    }
}
