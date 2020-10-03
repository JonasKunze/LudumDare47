using DefaultNamespace;
using UnityEngine;

public class BallSpawnerBluePrint : IBluePrint
{
    private readonly BallSpawner _prefab;

    public BallSpawnerBluePrint(BallSpawner prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
    
    public string GetName() => "Ball Spawner";
    public Color GetColor() => Color.white;
}