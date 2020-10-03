using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using UnityEngine;

public enum BlueprintIndex
{
    Spawner = 0,
    Bouncer = 1,
    Bin = 2,
    PlatformStart = 3
}

public class Creator : MonoBehaviour
{
    [SerializeField] private SoundPlatform soundPlatformPrefab = null;
    [SerializeField] private BouncerPlatform bouncerPlatformPrefab = null;
    [SerializeField] private BallBin ballPortalPrefab = null;
    [SerializeField] private BallSpawner ballSpawnerPrefab = null;

    [SerializeField] private Transform spawnParent = null;
    [SerializeField] private LayerMask mask;

    private readonly List<IBluePrint> _bluePrints = new List<IBluePrint>();

    [SerializeField] private List<Color> colors;

    public static Creator Instance;

    private bool _inCreationMode;
    private IInteractable _current;

    private Camera _camera;
    private Vector3 _startMouseWorldPosition;

    [NonSerialized] public int activeBlueprintId = (int) BlueprintIndex.Spawner;

    private void Awake()
    {
        Instance = this;

        _camera = Camera.main;
    }

    private void Start()
    {
        SoundHandler.Instance.OnSoundLoaded.AddListener(CreateSoundPlatforms);
    }

    private void CreateSoundPlatforms(int nAudioClips)
    {
        _bluePrints.Add(new BallSpawnerBluePrint(ballSpawnerPrefab));
        _bluePrints.Add(new BouncerPlatformBluePrint(bouncerPlatformPrefab));
        _bluePrints.Add(new BallPortalBluePrint(ballPortalPrefab));

        for (int i = 0; i < nAudioClips; i++)
        {
            _bluePrints.Add(new SoundPlatformBluePrint(soundPlatformPrefab,
                new PlatformProperties {clipIndex = i, color = colors[i % colors.Count]}));
        }
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, 100, mask);
        if (hit || Interactable.IsDragging)
            return;

        if (!_inCreationMode && Input.GetMouseButton(0))
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
}