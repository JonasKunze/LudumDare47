using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Platform : SerializableObject, IInteractable
{
    [SerializeField] private Interactable interactable = null;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private SpriteRenderer _spriteRendererGlow;

    public PlatformProperties properties;

    private float _glowCurrentTime;
    private readonly float _glowMaxLifeTime = 0.4f;

    public int blueprintIndex;

    private Color _defaultGlowColor;
    
    private void Start()
    {
        _defaultGlowColor = _spriteRendererGlow.material.GetColor("Color_D1776063");
    }

    public void SetProperties(PlatformProperties p, int blueprintIndex)
    {
        properties = p;

        _spriteRenderer.color = p.color;
        this.blueprintIndex = blueprintIndex;
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


    
    public void SetGlow(bool value, Color color, float intensity)
    {
        if (value)
        {
            color.r *= intensity;
            color.g *= intensity;
            color.b *= intensity;
            _spriteRendererGlow.material.SetColor("Color_D1776063", color);
        }
        else
        {
            _spriteRendererGlow.material.SetColor("Color_D1776063", _defaultGlowColor);
        }

        var c = _spriteRendererGlow.color;
        c.a = value ? 1 : 0;
        _spriteRendererGlow.color = c;
    }

    public void OnCreationStart(Transform parent, Vector3 startPosition)
    {
        var tr = GetTransform();

        tr.SetParent(parent);
        tr.position = startPosition;
        tr.localScale = new Vector3(0, tr.localScale.y, 0);
        GetInteractable().SetActive(false);
    }

    public void OnCreationFinish()
    {
        if (transform.localScale.x > .3)
            SetActive(true);
        else
            Destroy(gameObject);
    }

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition)
    {
        var dir = newPosition - startPosition;
        var center = (startPosition + newPosition) * 0.5f;

        var position = center;
        var rotation = Quaternion.FromToRotation(Vector3.right, dir);

        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = new Vector3(dir.magnitude, transform.localScale.y, 0);
    }

    public Transform GetTransform() => transform;

    public Interactable GetInteractable() => interactable;

    public override BlueprintIndex GetBlueprintIndex()    {
        return (BlueprintIndex) blueprintIndex;
    }
}