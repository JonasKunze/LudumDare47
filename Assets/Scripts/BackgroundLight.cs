using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BackgroundLight : MonoBehaviour
{
    private float _startTime;
    private float _lifeTime = 5;
    private Vector3 _direction = Vector3.up;
    private float _speed = 0.01f;
    private float _maxIntensity;

    private Light2D _light;
    private float _intensitySpeed = 0.01f;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _maxIntensity = _light.intensity;
    }

    void Start()
    {
        _startTime = Time.time;
    }

    public void Init(Vector3 pos, Vector3 dir, float speed, float lifeTime, Color color, float size)
    {
        transform.position = pos;
        _direction = dir;
        _direction.Normalize();
        _speed = speed;
        _lifeTime = lifeTime;
        _light.intensity = 0;
        _light.color = color;
        _light.pointLightOuterRadius = size;
    }

    // Update is called once per frame
    void Update()
    {
        
        var delta = Time.time - _startTime;
        _light.intensity = _maxIntensity * Mathf.Sin(Mathf.PI *delta / _lifeTime);
        if (delta > _lifeTime)
        {
            BackgroundLightSpawner.Instance.Remove(this);
            Destroy(gameObject);
        }

        transform.position += _speed * _direction;
    }
}