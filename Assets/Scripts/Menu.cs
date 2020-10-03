using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private ToggleGroup parent;
        [SerializeField] private ToggleButton toggleButtonPrefab;
        
        public bool running = false;
        public static UnityEvent OnGameStarted = new UnityEvent();
        public static UnityEvent OnGameStopped = new UnityEvent();

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void Start()
        {
            Creator.OnSetup.AddListener((activeBluePrint, soundPlatformCount) =>
            {
                int index = 0;
                
                foreach (var bluePrint in Creator.Instance.BluePrints)
                {
                    var go = Instantiate(toggleButtonPrefab, parent.transform);
                    var toggle = go.GetComponentInChildren<Toggle>();
                    toggle.group = parent;
                    var text = bluePrint.GetName();
                    go.SetText(text, text);
                    go.SetColor(bluePrint.GetColor());
                    var index1 = index;
                    go.onClick.AddListener((() =>
                    {
                        Creator.Instance.activeBlueprintId = index1;
                    }));

                    index++;
                }
            });
        }

        public void OnActiveIndexChanged(int newActiveIndex)
        {
            
        }

        public void StartGame()
        {
            if (running)
                OnGameStopped?.Invoke();
            else
                OnGameStarted?.Invoke();
            running = !running;
        }

        public void Save()
        {
            SerializationHandler.SerializeScene();
        }
    }
}