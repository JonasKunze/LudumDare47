using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AutoDestructParticleSystem : MonoBehaviour
{
    private void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        if (ps)
           Destroy(gameObject, ps.main.duration);
        else
            Destroy(gameObject);
    }
}
