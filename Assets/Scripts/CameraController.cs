using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IDragHandler, IScrollHandler

{
    private Vector2 _lastGrabWorldPos;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPos = (Vector2) _camera.ScreenToWorldPoint(eventData.position);
        var delta = newPos - _lastGrabWorldPos;
        _lastGrabWorldPos = newPos;
        transform.position -= new Vector3(delta.x, delta.y, 0);
    }

    public void OnScroll(PointerEventData eventData)
    {
        var worldPos = _camera.ScreenToWorldPoint(eventData.position);
        _camera.orthographicSize *= Mathf.Pow(1.2f, -eventData.scrollDelta.y);
        var delta = _camera.ScreenToWorldPoint(eventData.position) - worldPos;
        transform.position -= delta;
    }
}