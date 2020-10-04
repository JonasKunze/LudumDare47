using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public enum BlueprintIndex
{
    Spawner = 0,
    Bouncer = 1,
    Bin = 2,
    SpawnTrigger = 3,
    PlatformStart = 4
}

public class Creator : MonoBehaviour
{
    [SerializeField] private SoundPlatform soundPlatformPrefab = null;
    [SerializeField] private BouncerPlatform bouncerPlatformPrefab = null;
    [SerializeField] private BallBin ballPortalPrefab = null;
    [SerializeField] private BallSpawner ballSpawnerPrefab = null;
    [SerializeField] private BallSpawnTrigger ballSpawnTriggerPrefab = null;

    [SerializeField] private Transform spawnParent = null;
    [SerializeField] private LayerMask mask;

    private readonly List<IBluePrint> _bluePrints = new List<IBluePrint>();
    private List<string> _levels;
    
    [SerializeField] private List<Color> colors;

    public static Creator Instance;

    private bool _inCreationMode;
    private IInteractable _current;

    private Camera _camera;
    private Vector3 _startMouseWorldPosition;

    [NonSerialized] public int activeBlueprintId = (int) BlueprintIndex.Spawner;
    public List<IBluePrint> BluePrints => _bluePrints;

    public static UnityEvent<int, int> OnSetup = new UnityEvent<int, int>();
    public static UnityEvent<List<string>> OnLevelSetup = new UnityEvent<List<string>>();
    public static UnityEvent<string> OnLevelAdded = new UnityEvent<string>();

    private void Awake()
    {
        Instance = this;

        _camera = Camera.main;
    }

    private void Start()
    {
        _bluePrints.Add(new BallSpawnerBluePrint(ballSpawnerPrefab));
        _bluePrints.Add(new BouncerPlatformBluePrint(bouncerPlatformPrefab));
        _bluePrints.Add(new BallBinBluePrint(ballPortalPrefab));
        _bluePrints.Add(new BallSpawnTriggerBluePrint(ballSpawnTriggerPrefab));

        _levels = new List<string>();
        
        SoundHandler.Instance.OnSoundLoaded.AddListener(CreateSoundPlatforms);
    }

    private void CreateSoundPlatforms(List<AudioData> audioData)
    {
        for (int i = 0; i < audioData.Count; i++)
        {
            _bluePrints.Add(new SoundPlatformBluePrint(soundPlatformPrefab,
                new PlatformProperties {clipIndex = i, color = colors[i % colors.Count], name = audioData[i].Title},
                (int) BlueprintIndex.PlatformStart + i));
        }

        OnSetup?.Invoke(activeBlueprintId, _bluePrints.Count - 3);
        
        var defaultLevels = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "DefaultLevels"), "*.json");
        var userLevels = SerializationHandler.GetUserLevels();
        
        _levels.AddRange(defaultLevels);
        _levels.AddRange(userLevels);

        var levelNames = _levels.Select(Path.GetFileNameWithoutExtension).ToList();
        OnLevelSetup?.Invoke(levelNames);
        
        if (_levels.Count != 0)
            LoadLevel(0);
    }
    
    public void SaveLevel(in string fullPath)
    {
        if (_levels.Contains(fullPath))
            return;
        
        _levels.Add(fullPath);
        
        OnLevelAdded?.Invoke(Path.GetFileNameWithoutExtension(fullPath));
    }
    
    public void LoadLevel(int index)
    {
        LoadLevel(_levels[index]);
    }
    
    public void LoadLevel(in string fullFilePath)
    {
        if (File.Exists(fullFilePath))
        {
            Clear();
            
            var sr = File.OpenText(fullFilePath);
            var json = sr.ReadToEnd();
            var obj = JsonUtility.FromJson<SerializationContainer>(json);
            obj.Instantiate();
        }
        else
        {
            Debug.LogError("Could not Open the file: " + fullFilePath + " for reading.");
        }
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, 100, mask);

        var success = !(hit || Interactable.IsDragging || EventSystem.current.IsPointerOverGameObject());

        if (success && !_inCreationMode && Input.GetMouseButtonDown(0))
        {
            _inCreationMode = true;

            var mousePosition = Input.mousePosition;
            mousePosition.z = -_camera.transform.position.z;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);

            _startMouseWorldPosition = mouseWorldPosition;

            _current = _bluePrints[activeBlueprintId].Build();
            _current.OnCreationStart(spawnParent, mouseWorldPosition);
        }
        else if (_inCreationMode && Input.GetMouseButtonUp(0))
        {
            _inCreationMode = false;

            _current.OnCreationFinish();
            _current = null;
        }

        if (_inCreationMode)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = -_camera.transform.position.z;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);

            _current.OnCreationUpdate(mouseWorldPosition, _startMouseWorldPosition);
        }
    }

    public IInteractable BuildBlueprint(int bluePrint)
    {
        var obj = _bluePrints[bluePrint].Build();
        obj.GetTransform().parent = spawnParent;
        return obj;
    }
    
    public void Clear()
    {
        foreach (var platform in FindObjectsOfType<SerializableObject>())
        {
            Destroy(platform.gameObject);
        }
    }
}