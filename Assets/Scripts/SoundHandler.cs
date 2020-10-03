using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SoundHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    public List<AudioClip> AudioClips { get; private set; }

    private static int _nFilesToLoad;

    public static SoundHandler Instance;

    public UnityEvent<int> OnSoundLoaded;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private void Start()
    {
        AudioClips = new List<AudioClip>();
        _audioSource = GetComponent<AudioSource>();
        
        var files = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Audio", "*.wav").ToList();
        // files.AddRange(System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Audio", "*.mp3"));

        _nFilesToLoad = files.Count;
        
        foreach (string file in files)
        {
            Debug.Log("Loading wav file " + file);
            StartCoroutine(ConvertFilesToAudioClip(files.First()));
        }
    }

    public void PlayClip(int clipNumber)
    {
        if (clipNumber < AudioClips.Count)
        {
            _audioSource.PlayOneShot(AudioClips[clipNumber]);
        }

    }
    
    private IEnumerator ConvertFilesToAudioClip(string name)
    {
        string url = string.Format("file://{0}", name);
        WWW www = new WWW(url);
        yield return www;
        var clip = www.GetAudioClip(false,false);
        if (clip != null)
            AudioClips.Add(clip);
        else
            Debug.LogError($"Failed loading {name}");

        _nFilesToLoad--;

        if (_nFilesToLoad == 0)
        {
            OnAudioClipsLoaded();
        }
    }

    void OnAudioClipsLoaded()
    {
        Debug.Log("All audio clips loaded");
        OnSoundLoaded?.Invoke(AudioClips.Count);
    }
}
