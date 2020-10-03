using UnityEngine;


public class Platform : MonoBehaviour
{
    
    public void ColliderHit(GameObject obj, Collision2D info)
    {
        Debug.LogError($"{obj.name}, {info.gameObject}");
    }
}