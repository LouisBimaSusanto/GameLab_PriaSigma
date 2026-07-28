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

    [Header("BGM")]
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    private AudioSource bgmSource;

    [Header("SFX Library")]
    public List<SoundEntry> sfxList = new List<SoundEntry>();
    private AudioSource sfxSource;

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

    private void Start()
    {
        PlayBGM();
    }


    private void SetupAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    public void PlayBGM()
    {
        if (bgmClip == null) return;
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(string soundName)
    {
        SoundEntry entry = sfxList.Find(s => s.name == soundName);
        if (entry == null)
        {
            Debug.LogWarning($"[AudioManager] SFX '{soundName}' tidak ditemukan!");
            return;
        }

        sfxSource.pitch = entry.pitch;
        sfxSource.PlayOneShot(entry.clip, entry.volume);
    }
}