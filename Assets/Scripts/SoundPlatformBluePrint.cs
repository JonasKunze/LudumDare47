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
    private readonly PlatformProperties _properties;

    public SoundPlatformBluePrint(SoundPlatform prefab, in PlatformProperties properties)
    {
        _prefab = prefab;
        _properties = properties;
    }

    public IInteractable Build()
    {
        var newGo = Object.Instantiate(_prefab);
        newGo.SetProperties(_properties);

        return newGo;
    }

    public string GetName() => _properties.name;
    public Color GetColor() => _properties.color;
}