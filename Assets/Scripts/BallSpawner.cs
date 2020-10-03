using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallSpawner : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject ballPrefab;

        private Interactable _interactable;
        
        private void Awake()
        {
            Spawn();

            _interactable = GetComponentInChildren<Interactable>();
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
        
        public void OnCreationStart(Transform parent, Vector3 startPosition)
        {
            var tr = GetTransform();

            tr.SetParent(parent);
            tr.position = startPosition;
            tr.localScale = new Vector3(0, tr.localScale.y, 0);
            GetInteractable().SetActive(false);
        }

        public void OnCreationFinish()
        {
            if (transform.localScale.x > .3)
                _interactable.SetActive(true);
            else
                Destroy(gameObject);
        }

        public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition)
        {
            var dir = newPosition - startPosition;
            var center = (startPosition + newPosition) * 0.5f;

            var position = center;
            var rotation = Quaternion.FromToRotation(Vector3.right, dir);
        
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = new Vector3(dir.magnitude, transform.localScale.y, 0);
        }

        public Transform GetTransform() => transform;
        public Interactable GetInteractable() => _interactable;
    }
}