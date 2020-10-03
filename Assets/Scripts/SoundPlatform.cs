using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundPlatform : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public void ColliderHit(Collider2D other)
    {
        Debug.LogError($"{other.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this && eventData.button == PointerEventData.InputButton.Right)
            Destroy(gameObject);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.delta + "OnDrag");    }
}