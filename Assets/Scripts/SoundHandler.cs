using System;
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
    private float _beatsPerMinute = 120;
    private float _beatCurrentTime;

    [SerializeField] private AudioSource _audioSourcePrefab;

    public float BeatTimeDeltaSeconds => 60 / _beatsPerMinute;
    
    private AudioSource _audioSource;

    public List<AudioData> AudioData { get; private set; }

    private static int _nFilesToLoad;

    public static SoundHandler Instance;

    public UnityEvent<List<AudioData>> OnSoundLoaded;

    public UnityEvent BeatTrigger;
    
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

    public void ResetBeat()
    {
        _beatCurrentTime = 0;
    }

    public void PlayClip(int clipNumber)
    {
        if (clipNumber < AudioData.Count)
        {
            // _audioSource.clip = AudioClips[clipNumber];
            // _audioSource.Play();
            _audioSource.PlayOneShot(AudioData[clipNumber].Clip);
            // StartCoroutine(PlayAudioClip(AudioClips[clipNumber]));
        }
    }

    IEnumerator PlayAudioClip(AudioClip clip)
    {
        var obj = Instantiate(_audioSourcePrefab);
        var source = obj.GetComponent<AudioSource>();
        source.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        Destroy(obj);
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

    private void FixedUpdate()
    {
        _beatCurrentTime += Time.fixedDeltaTime;
        if (_beatCurrentTime > BeatTimeDeltaSeconds)
        {
            _beatCurrentTime = 0;
            BeatTrigger?.Invoke();
        }
    }
}