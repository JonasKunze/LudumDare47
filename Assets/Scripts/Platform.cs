﻿using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler
{
    private Vector2 lastGrabWorldPos;
    private GameObject grabbedObject;
    private bool grabbingLeft;

    [SerializeField] private GameObject leftGo, rightGo, centerGo;

    private Vector2 left
    {
        get => leftGo.transform.position;
        set => SetPosition(value, right);
    }

    private Vector2 right
    {
        get => rightGo.transform.position;
        set => SetPosition(left, value);
    }

    private void SetPosition(Vector2 left, Vector2 right)
    {
        var delta = right - left;
        transform.position = (left + right) / 2;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, delta);
        transform.localScale = new Vector3(delta.magnitude, transform.localScale.y, 1);
    }

    private Vector2 center => transform.position;

    public void ColliderHit(Collider2D other)
    {
        Debug.LogError($"{other.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this && eventData.button == PointerEventData.InputButton.Right)
            Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject.name);
        grabbedObject = eventData.pointerEnter.gameObject;
        lastGrabWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPos = (Vector2) Camera.main.ScreenToWorldPoint(eventData.position);
        var delta = newPos - lastGrabWorldPos;
        lastGrabWorldPos = newPos;
        if (grabbedObject == centerGo)
        {
            transform.position += new Vector3(delta.x, delta.y, 0);
        }
        else if (grabbedObject == rightGo)
        {
            right += delta;
        }
        else if (grabbedObject == leftGo)
        {
            left += delta;
        }
    }
}