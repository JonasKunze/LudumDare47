using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Menu : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 1;
        }

        public void StartGame()
        {
            Time.timeScale = 1;

            Creator.Instance.activeBlueprintId++;
        }
    }
}