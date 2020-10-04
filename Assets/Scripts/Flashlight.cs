using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private GameObject _lightObject;

    private Camera _cam;
    private float _startCamSize;
    private float _startLightSize;
    [SerializeField] private Light2D _light;

    private void Awake()
    {
        _cam = Camera.main;
        _startCamSize = _cam.orthographicSize;
        _startLightSize = _light.pointLightOuterRadius;
    }

    void Update()
    {
        float size = _cam.orthographicSize / _startCamSize;
        _light.pointLightOuterRadius = _startLightSize * size;
        Vector3 mousePosition = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        _lightObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
    }
}