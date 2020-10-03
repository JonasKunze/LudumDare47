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
            StartCoroutine(SpawnCoro());
        }

        private IEnumerator SpawnCoro()
        {
            var waiter = new WaitForSeconds(cadence);

            while (true)
            {
                Instantiate(ballPrefab, transform.position, quaternion.identity);
                yield return waiter;
            }
        }
    }
}