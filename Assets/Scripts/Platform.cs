using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Platform : SerializableObject, IInteractable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _spriteRendererGlow;

    public PlatformProperties properties;

    private Interactable _interactable = null;
    private float _glowCurrentTime;
    private readonly float _glowMaxLifeTime = 0.4f;

    public int blueprintIndex;

    private Color _defaultGlowColor;

    [SerializeField] private GameObject _particlesNotesPrefab;
    
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

        SpawnNotesParticles(info);
    }

    void SpawnNotesParticles(Collision2D info)
    {
        var collisionPoint2D = info.GetContact(0).point;
        var pos = new Vector3(collisionPoint2D.x, collisionPoint2D.y, 0);
        var go = Instantiate(_particlesNotesPrefab, pos, Quaternion.identity, null);
    }

    public void SetActive(bool b) => _interactable.SetActive(b);

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

    public void OnCreationStart(Transform parent, Vector3 startPosition) => GetInteractable().OnCreationStart(parent, startPosition);

    public void OnCreationFinish() => GetInteractable().OnCreationFinish();

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition) => GetInteractable().OnCreationUpdate(newPosition, startPosition);

    public Transform GetTransform() => transform;

    public Interactable GetInteractable()
    {
        if (_interactable == null)
            _interactable = GetComponentInChildren<Interactable>();
        return _interactable;
    }

    public override BlueprintIndex GetBlueprintIndex()    {
        return (BlueprintIndex) blueprintIndex;
    }
}