using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D left, right, center;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider == left)
        {
            Debug.LogError($"{other.collider.name}");
        }
        else if (other.collider == right)
        {
            Debug.LogError($"{other.collider.name}");
        }
        else if (other.collider == center)
        {
            Debug.LogError($"{other.collider.name}"); 
        }
    }
}
