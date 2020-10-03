using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;

        private void Awake()
        {
            Spawn();
        }

        public Ball Spawn(GameObject go = null)
        {
            if (go == null)
                go = Instantiate(ballPrefab, transform.position, quaternion.identity);
            else
                go.transform.position = transform.position;

            var ball = go.GetComponent<Ball>();
            ball.spawner = this;

            var speed = Mathf.Clamp(transform.localScale.x - 0.2f, 0, 10);

            go.gameObject.GetComponent<Rigidbody2D>().velocity = speed * transform.right;
            return ball;
        }
    }
}