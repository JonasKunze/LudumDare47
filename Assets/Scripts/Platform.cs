using UnityEngine;


public class Platform : MonoBehaviour
{
    
    public void ColliderHit(Collider2D other)
    {
        Debug.LogError($"{other.name}");
    }
}