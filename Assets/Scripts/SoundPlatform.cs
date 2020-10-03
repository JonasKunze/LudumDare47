using UnityEngine;

public class SoundPlatform : MonoBehaviour
{
    public void ColliderHit(Collider2D other)
    {
        Debug.LogError($"{other.name}"); 
    }
}
