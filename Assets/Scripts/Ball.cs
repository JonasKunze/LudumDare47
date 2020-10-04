using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Ball : MonoBehaviour
    {
        public BallSpawner spawner;

        [SerializeField] private GameObject _beatParticlePrefab;

        private void Start()
        {
            SoundHandler.Instance.BeatTrigger.AddListener(OnBeatTrigger);
            StartCoroutine(CheckDeathCoro());
        }

        void OnBeatTrigger()
        {
            Instantiate(_beatParticlePrefab, transform.position, Quaternion.identity, null);
        }

        private IEnumerator CheckDeathCoro()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (transform.position.sqrMagnitude > 100 * 100)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}