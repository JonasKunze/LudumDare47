using System;
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
        }

        void OnBeatTrigger()
        {
            Instantiate(_beatParticlePrefab, transform.position, Quaternion.identity, null);
        }
    }
}