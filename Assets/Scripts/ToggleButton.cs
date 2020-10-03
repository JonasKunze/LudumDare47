using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;

    [SerializeField] private string toggleOnText, toggleOffBText;
    [SerializeField] private Image background = null;
    
    [SerializeField] private bool isOn = false;

    public UnityEvent onClick = new UnityEvent();
    
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
        
        onClick?.Invoke();
    }

    public void SetToggle(bool on)
    {
        isOn = on;
        textComponent.text = isOn ? toggleOnText : toggleOffBText;
        
        onClick?.Invoke();
    }
}
