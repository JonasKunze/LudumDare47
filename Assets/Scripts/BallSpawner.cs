using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private float speedFactor = 10;


        private void Start()
        {
            Spawn();
        }

        public Ball Spawn(GameObject go = null)
        {
            if (go == null)
                go = Instantiate(ballPrefab, transform.position, quaternion.identity);


            go.transform.position = transform.position + (go.transform.localScale.x + transform.localScale.x) / 2 * transform.right;

            var ball = go.GetComponent<Ball>();
            ball.spawner = this;

            var speed = speedFactor * Mathf.Clamp(transform.localScale.x - 0.2f, 0, 10);

            go.gameObject.GetComponent<Rigidbody2D>().velocity = speed * transform.right;
            return ball;
        }
    }
}