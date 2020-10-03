using DefaultNamespace;
using UnityEngine;

public class BouncerPlatformBluePrint : IBluePrint
{
    private readonly BouncerPlatform _prefab;

    public BouncerPlatformBluePrint(BouncerPlatform prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
}