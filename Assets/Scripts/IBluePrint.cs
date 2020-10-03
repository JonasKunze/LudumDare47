using UnityEngine;

public interface IBluePrint
{
    IInteractable Build();
    string GetName();
    Color GetColor();
}