using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager> {
    [SerializeField]
    Sound[] characterSounds;

    [SerializeField]
    AudioAsset backgroundSounds;
    
    private void Start()
    {
        for(int i = 0; i < characterSounds.Length; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + characterSounds[i].m_name);
            go.transform.SetParent(transform);
            characterSounds[i].SetSource(go.AddComponent<AudioSource>());
        }

        SetBackgroundSource();
        PlayBackgroundMusic();
    }
    
    private AudioSource backgroundSource;
    private AudioSource secondBackSource;
    public void SetBackgroundSource() {
        GameObject bgo = new GameObject(backgroundSounds.name);
        bgo.transform.SetParent(transform);
        GameObject mixgo = new GameObject(backgroundSounds.name);
        mixgo.transform.SetParent(transform);
        
        backgroundSource = bgo.AddComponent<AudioSource>();
        secondBackSource = mixgo.AddComponent<AudioSource>();
    }
    
    public void PlaySound(string name)
    {
        for(int i = 0; i < characterSounds.Length; i++)
        {
            if(characterSounds[i].m_name == name)
            {
                characterSounds[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager: Sound name not found in list: " + name);
    }

    private int count = 0;
    private void PlayBackgroundMusic() {
        StartCoroutine(PlayOrder());
    }

    private float mixTime = 50f;
    IEnumerator PlayOrder() {
        var currentSource = backgroundSource;
        backgroundSounds.backgroundMusic.Play(count, currentSource);
        count++; 
        while (true) {
            yield return new WaitForSeconds(currentSource.clip.length - mixTime);
            
            float currentTime = 0;
            var start = currentSource.volume;
            var secondSource = currentSource == backgroundSource ? secondBackSource : backgroundSource;
            backgroundSounds.backgroundMusic.Play(count, secondSource);
            secondSource.volume = 0;
            count++;
            
            while (currentTime < mixTime) {
                currentTime += Time.deltaTime;
                currentSource.volume = Mathf.Lerp(start, 0f, currentTime / mixTime);
                secondSource.volume = Mathf.Lerp(0f, backgroundSounds.backgroundMusic.volume, currentTime / mixTime);
                yield return null;
            }

            currentSource = secondSource;
        }
    }
}

[Serializable]
public class Sound
{
    public string m_name;
    public AudioClip[] m_clips;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0f, 1.5f)]
    public float pitch = 1.0f;
    public Vector2 m_randomVolumeRange = new Vector2(1.0f, 1.0f);
    public Vector2 m_randomPitchRange = new Vector2(1.0f, 1.0f);

    private AudioSource m_source;

    public void SetSource(AudioSource source)
    {
        m_source = source;
        int randomClip = Random.Range(0, m_clips.Length - 1);
        m_source.clip = m_clips[randomClip];
    }
    
    public void Play()
    {
        if(m_clips.Length > 1) {
            int randomClip = Random.Range(0, m_clips.Length - 1);
            m_source.clip = m_clips[randomClip];
        }
        m_source.volume = volume * Random.Range(m_randomVolumeRange.x, m_randomVolumeRange.y);
        m_source.pitch = pitch * Random.Range(m_randomPitchRange.x, m_randomPitchRange.y);
        m_source.Play();
    }
}

[Serializable]
public class BackgroundSound {
    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0f, 1.5f)]
    public float pitch = 1.0f;
    
    public AudioClip GetClip(int count) {
        return clips[count];
    }
    
    public void Play(int count, AudioSource source) {
        if (clips.Length > count) {
            source.clip = clips[count];
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
    }
    
}
