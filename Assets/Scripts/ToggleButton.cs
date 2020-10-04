using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    [SerializeField] private Sprite toggleOnSprite, toggleOffSprite;
    [SerializeField] private string toggleOnText, toggleOffBText;
    [SerializeField] private Image background = null;

    [SerializeField] private bool isOn = false;

    [FormerlySerializedAs("onClick")] public UnityEvent onActivated = new UnityEvent();

    private void Awake()
    {
        isOn = true;
        Toggle();
    }

    public void SetText(string onText, string offText)
    {
        toggleOnText = onText;
        toggleOffBText = offText;
        
        textComponent.text = isOn ? toggleOnText : toggleOffBText;
    }

    public void SetColor(Color color) => background.color = color;

    public void Toggle()
    {
        isOn = !isOn;
        textComponent.text = isOn ? toggleOnText : toggleOffBText;
        background.sprite = isOn ? toggleOnSprite : toggleOffSprite;

        if (isOn)
            onActivated?.Invoke();
    }

    public void SetToggle(bool on)
    {
        isOn = on;
        textComponent.text = isOn ? toggleOnText : toggleOffBText;

        if (isOn)
            onActivated?.Invoke();
    }
}