using DefaultNamespace;
using UnityEngine;

public class BallSpawnTriggerBluePrint : IBluePrint
{
    private readonly BallSpawnTrigger _prefab;

    public BallSpawnTriggerBluePrint(BallSpawnTrigger prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
    
    public string GetName() => "Ball Spawn Trigger";
    public Color GetColor() => Color.white;
}