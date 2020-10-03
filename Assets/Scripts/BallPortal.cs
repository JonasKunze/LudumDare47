using DefaultNamespace;
using UnityEngine;

public class BallPortal : MonoBehaviour
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
                ball.transform.position = ball.spawner.transform.position;
                ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
    }
}