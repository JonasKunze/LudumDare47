using UnityEngine;

public class BallBinBluePrint : IBluePrint
{
    private readonly BallBin _prefab;

    public BallBinBluePrint(BallBin prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
    public string GetName() => "Bin";
    public Color GetColor() => Color.white;
}