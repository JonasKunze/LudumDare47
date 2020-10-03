using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private float cadence = 1;

        private void Awake()
        {
            Spawn();
        }

        public Ball Spawn()
        {
            var go = Instantiate(ballPrefab, transform.position, quaternion.identity);
            var ball = go.GetComponent<Ball>();
            ball.spawner = this;
            return ball;
        }
    }
}