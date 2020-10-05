using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverFeedback : MonoBehaviour
{
    [SerializeField] private GameObject _hoverRepresentation;

    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        UpdateScale();
    }

    private void OnMouseEnter()
    {
        if (_hoverRepresentation)
            _hoverRepresentation.SetActive(true);
    }

    private void OnMouseOver()
    {
        UpdateScale();
    }

    void UpdateScale()
    {
        if (_boxCollider && _hoverRepresentation)
        {
            var scale = _hoverRepresentation.transform.localScale;
            scale.x = _boxCollider.size.x;
            _hoverRepresentation.transform.localScale = scale;
        }
    }
    
    private void OnMouseExit()
    {
        if (_hoverRepresentation)
            _hoverRepresentation.SetActive(false);
    }
}
