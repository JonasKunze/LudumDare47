using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class Menu : MonoBehaviour
    {
        public bool running = false;
        public static UnityEvent OnGameStarted = new UnityEvent();
        public static UnityEvent OnGamePaused = new UnityEvent();

        private void Awake()
        {
            Time.timeScale = 1;
        }

        public void StartGame()
        {
            if (running)
                OnGamePaused?.Invoke();
            else
                OnGameStarted?.Invoke();
            running = !running;
        }
    }
}