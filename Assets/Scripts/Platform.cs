using System;
using System.Collections;
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
    
    [SerializeField] private SpriteRenderer _spriteRendererGlow;
    
    public PlatformProperties properties;
    
    private float _glowCurrentTime;
    private readonly float _glowMaxLifeTime = 0.4f;

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
        
        StartCoroutine(GlowCoro());
    }

    public void SetActive(bool b) => interactable.SetActive(b);

    IEnumerator GlowCoro()
    {
        _glowCurrentTime = 0;
        
        while (_glowCurrentTime < _glowMaxLifeTime)
        {
            _glowCurrentTime += Time.deltaTime * Mathf.PI / _glowMaxLifeTime;
            var color = _spriteRendererGlow.color;
            color.a = Mathf.Sin(_glowCurrentTime);
            _spriteRendererGlow.color = color;
            yield return null;
        }
        var c = _spriteRendererGlow.color;
        c.a = 0;
        _spriteRendererGlow.color = c;
    }
}