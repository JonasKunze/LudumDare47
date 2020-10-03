using System;
using System.Linq;
using UnityEngine;

public struct PlatformProperties
{
    public int clipIndex;
    public Color color;
}

public class Platform : MonoBehaviour
{
    [SerializeField] private Interactable interactable = null;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public PlatformProperties properties;

    private void Start()
    {
        Debug.Assert(_spriteRenderer);
    }

    public void SetProperties(PlatformProperties p)
    {
        properties = p;
        _spriteRenderer.color = p.color;
    }
    
    public void ColliderHit(GameObject obj, Collision2D info)
    {
        SoundHandler.Instance.PlayClip(properties.clipIndex);
    }

    public void SetActive(bool b) => interactable.SetActive(b);
}