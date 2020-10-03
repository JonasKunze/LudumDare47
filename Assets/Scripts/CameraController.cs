using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private Vector2 _lastGrabWorldPos;
    private Camera _camera;
    private bool _isMoving;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;

        if (!_isMoving && Input.GetMouseButton(1))
        {
            _isMoving = true;

            var newPos = (Vector2) _camera.ScreenToWorldPoint(mousePosition);
            _lastGrabWorldPos = newPos;
        }
        else if (_isMoving && Input.GetMouseButtonUp(1))
        {
            _isMoving = false;
        }

        if (Math.Abs(Input.GetAxis("Mouse ScrollWheel")) > 1e-3f)
        {
            _camera.orthographicSize *= Mathf.Pow(1.2f, -Input.mouseScrollDelta.y);
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 0.5f, 10f);
            var worldPos = _camera.ScreenToWorldPoint(mousePosition);
            var delta = _camera.ScreenToWorldPoint(mousePosition) - worldPos;
            transform.position -= delta;
        }

        if (_isMoving)
        {
            var newPos = (Vector2) _camera.ScreenToWorldPoint(mousePosition);
            var delta = newPos - _lastGrabWorldPos;
            transform.position -= new Vector3(delta.x, delta.y, 0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            return;

        var newPos = (Vector2) _camera.ScreenToWorldPoint(eventData.position);
        var delta = newPos - _lastGrabWorldPos;
        transform.position -= new Vector3(delta.x, delta.y, 0);
    }

    public void OnScroll(PointerEventData eventData)
    {
        var worldPos = _camera.ScreenToWorldPoint(eventData.position);
        _camera.orthographicSize *= Mathf.Pow(1.2f, -eventData.scrollDelta.y);
        var delta = _camera.ScreenToWorldPoint(eventData.position) - worldPos;
        transform.position -= delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var newPos = (Vector2) _camera.ScreenToWorldPoint(eventData.position);
        _lastGrabWorldPos = newPos;
    }
}