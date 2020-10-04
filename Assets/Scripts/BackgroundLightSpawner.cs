using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundLightSpawner : MonoBehaviour
{
    [SerializeField] private BackgroundLight _backgroundLightPrefab;

    [SerializeField] private List<Color> _colors;
    
    public static BackgroundLightSpawner Instance;

    private List<BackgroundLight> _lights;

    private Camera _cam;

    private int _maxLights = 10;

    private float _minLifetime = 3;
    private float _maxLifeTime = 6;

    private float _minSpeed = 0.005f;
    private float _maxSpeed = 0.01f;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
        _lights = new List<BackgroundLight>();
        _cam = Camera.main;
    }

    public void Remove(BackgroundLight light)
    {
        _lights.Remove(light);
    }

    void SpawnLight()
    {
        float borderX = 0f;
        float borderY = 0f;
        var rndPos = _cam.ViewportToWorldPoint(new Vector3(Random.value * (1 - 2 * borderX) + borderX,
            Random.value * (1 - 2 * borderY) + borderY, 0));
        rndPos.z = 0;

        var go = Instantiate(_backgroundLightPrefab, transform);
        var light = go.GetComponent<BackgroundLight>();
        var lifetime = Random.Range(_minLifetime, _maxLifeTime);
        Vector3 dir = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
        var speed = Random.Range(_minSpeed, _maxSpeed);
        var color = _colors[Random.Range(0, _colors.Count)];
        light.Init(rndPos, dir, speed, lifetime, color, _cam.orthographicSize);
        _lights.Add(light);
    }

    private void Update()
    {
        if (_lights.Count < _maxLights)
        {
            SpawnLight();
        }
    }
}