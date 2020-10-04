using DefaultNamespace;
using UnityEngine;

public struct PlatformProperties
{
    public int clipIndex;
    public Color color;
    public string name;
}

public class SoundPlatformBluePrint : IBluePrint
{
    private readonly SoundPlatform _prefab;
    public readonly PlatformProperties _properties;
    private readonly int _blueprintIndex;

    public SoundPlatformBluePrint(SoundPlatform prefab, in PlatformProperties properties, int blueprintIndex)
    {
        _prefab = prefab;
        _properties = properties;
        _blueprintIndex = blueprintIndex;
    }

    public IInteractable Build()
    {
        var newGo = Object.Instantiate(_prefab);
        newGo.SetProperties(_properties, _blueprintIndex);

        return newGo;
    }

    public string GetName() => _properties.name;
    public Color GetColor() => _properties.color;
}