using System.IO;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    [SerializeField] private ToggleGroup parent;
    [SerializeField] private ToggleButton toggleButtonPrefab;
    [SerializeField] private TMP_Dropdown levelDropdown;

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
                go.onActivated.AddListener((() =>
                {
                    Creator.Instance.activeBlueprintId = index1;
                    if (bluePrint is SoundPlatformBluePrint soundBp)
                    {
                        SoundHandler.Instance.PlayClip(soundBp._properties.clipIndex);
                    }
                }));

                index++;
            }
        });
        
        levelDropdown.ClearOptions();

        Creator.OnLevelSetup.AddListener(levels =>
        {
            levelDropdown.AddOptions(levels);
        });
        
        Creator.OnLevelAdded.AddListener(level =>
        {
            levelDropdown.options.Add(new TMP_Dropdown.OptionData(level));
        });
        
        levelDropdown.onValueChanged.AddListener(index => Creator.Instance.LoadLevel(index));
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

    public void Clear() => Creator.Instance.Clear();
}