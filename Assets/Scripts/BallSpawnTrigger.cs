using DefaultNamespace;
using UnityEngine;

public class BallSpawnTrigger : MonoBehaviour
{
    public void ColliderHit(GameObject obj, Collision2D info)
    {
        var ball = info.gameObject.GetComponent<Ball>();
        if (ball)
        {
            if (!ball.spawner)
                Destroy(info.gameObject);
            else
            {
                ball.spawner.Spawn();
            }
        }
    }
}