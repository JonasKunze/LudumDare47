using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformHandler : MonoBehaviour
{
    [SerializeField] private Platform platformPrefab;

    [SerializeField] private List<Color> colors;
    
    private readonly List<PlatformProperties> _platformProperties = new List<PlatformProperties>(10);

    void Start()
    {
        SoundHandler.Instance.OnSoundLoaded.AddListener(CreatePlatforms);
    }

    private void CreatePlatforms(int nAudioClips)
    {
        for (int i = 0; i < nAudioClips; i++)
        {
            PlatformProperties properties = new PlatformProperties{clipIndex = i, color = colors[i % colors.Count]};
            _platformProperties.Add(properties);
        }
    }

    public int GetNPlatforms()
    {
        return _platformProperties.Count;
    }
    
    public Platform CreatePlatform(int index)
    {
        if (index > _platformProperties.Count)
        {
            Debug.LogError($"Platform {index} does not exist");
            return null;
        }
        
        var newGo = Instantiate(platformPrefab);
        var platform = newGo.GetComponent<Platform>();
        platform.SetProperties(_platformProperties[index]);
        return platform;
    }

    public Platform CreateRandomPlatform()
    {
        return CreatePlatform(Random.Range(0, GetNPlatforms()));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateRandomPlatform();
        }
    }
}
