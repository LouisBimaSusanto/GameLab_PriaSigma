using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundEntry
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Library")]
    public List<SoundEntry> bgmList = new List<SoundEntry>();
    private AudioSource bgmSource;

    [Header("SFX Library")]
    public List<SoundEntry> sfxList = new List<SoundEntry>();
    private AudioSource sfxSource;

    private AudioSource engineSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        // Setup jalur audio untuk mesin
        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.loop = true;
        engineSource.playOnAwake = false;
    }

    public void PlayBGM(string bgmName)
    {
        SoundEntry entry = bgmList.Find(b => b.name == bgmName);
        if (entry == null) return;

        if (bgmSource.clip == entry.clip && bgmSource.isPlaying) return;

        bgmSource.clip = entry.clip;
        bgmSource.volume = entry.volume;
        bgmSource.pitch = entry.pitch;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(string soundName)
    {
        SoundEntry entry = sfxList.Find(s => s.name == soundName);
        if (entry == null) return;

        sfxSource.pitch = entry.pitch;
        sfxSource.PlayOneShot(entry.clip, entry.volume);
    }

    public void PlayEngineSFX(string soundName)
    {
        SoundEntry entry = sfxList.Find(s => s.name == soundName);
        if (entry == null) return;

        if (engineSource.clip == entry.clip && engineSource.isPlaying) return;

        engineSource.clip = entry.clip;
        engineSource.volume = entry.volume;
        engineSource.pitch = entry.pitch;
        engineSource.Play();
    }

    public void StopEngineSFX() => engineSource.Stop();
}