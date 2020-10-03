using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlatformCollider : MonoBehaviour
{
    [SerializeField] private SoundPlatform platform;
    [SerializeField] private new Collider2D collider2D;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        platform.ColliderHit(collider2D);
    }
}
