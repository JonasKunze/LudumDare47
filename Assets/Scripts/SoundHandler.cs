﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public struct AudioData
{
    public AudioClip Clip;
    public string Title;
}

public class SoundHandler : MonoBehaviour
{
    private AudioSource _audioSource;

    public List<AudioData> AudioData { get; private set; }

    private static int _nFilesToLoad;

    public static SoundHandler Instance;

    public UnityEvent<List<AudioData>> OnSoundLoaded;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private void Start()
    {
        AudioData = new List<AudioData>();
        _audioSource = GetComponent<AudioSource>();

        var files = System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Audio", "*.wav").ToList();
        // files.AddRange(System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/Audio", "*.mp3"));

        _nFilesToLoad = files.Count;

        foreach (string file in files)
        {
            StartCoroutine(ConvertFilesToAudioClip(file));
        }
    }

    public void PlayClip(int clipNumber)
    {
        if (clipNumber < AudioData.Count)
        {
            _audioSource.PlayOneShot(AudioData[clipNumber].Clip);
        }
    }

    private IEnumerator ConvertFilesToAudioClip(string name)
    {
        string url = string.Format("file://{0}", name);
        WWW www = new WWW(url);
        yield return www;
        var clip = www.GetAudioClip(false, false);
        if (clip != null)
        {
            Debug.Log($"Loaded file {name}");
            AudioData.Add(new AudioData{Clip = clip, Title = Path.GetFileNameWithoutExtension(name)});
        }
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
        OnSoundLoaded?.Invoke(AudioData);
    }
}