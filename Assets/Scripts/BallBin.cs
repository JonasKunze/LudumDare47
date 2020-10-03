using DefaultNamespace;
using UnityEngine;

public class BallBin : MonoBehaviour
{
    public void ColliderHit(GameObject obj, Collision2D info)
    {
        Destroy(info.gameObject);
    }
}