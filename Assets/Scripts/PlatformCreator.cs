using System;
using UnityEngine;

public class PlatformCreator : MonoBehaviour
{
    [SerializeField] private Platform platformPrefab = null;

    private static PlatformCreator Instance;
    private static Platform ActivePlatformPrefab;

    private bool _inCreationMode;
    private Platform _platform;

    private Camera _camera;
    private Vector3 _startMouseWorldPosition;

    private void Awake()
    {
        Instance = this;
        ActivePlatformPrefab = platformPrefab;
        
        _camera = Camera.main;
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit || Interactable.IsDragging)
            return;
        
        if (!_inCreationMode && Input.GetMouseButton(0))
        {
            _inCreationMode = true;

            var mousePosition = Input.mousePosition;
            mousePosition.z = -_camera.transform.position.z;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);

            _startMouseWorldPosition = mouseWorldPosition;
            
            _platform = Instantiate(ActivePlatformPrefab);
            _platform.transform.position = mouseWorldPosition;
            _platform.transform.localScale = new Vector3(0, _platform.transform.localScale.y, 0);
            _platform.SetActive(false);
        }
        else if (_inCreationMode && Input.GetMouseButtonUp(0))
        {
            _inCreationMode = false;
            _platform.SetActive(true);
        }

        if (_inCreationMode)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = -_camera.transform.position.z;
            var mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
        
            var dir = mouseWorldPosition - _startMouseWorldPosition;
            var center = (_startMouseWorldPosition + mouseWorldPosition) * 0.5f;
        
            _platform.transform.position = center;
            _platform.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            _platform.transform.localScale = new Vector3(dir.magnitude, _platform.transform.localScale.y, 0);
        }
    }
}
