using DefaultNamespace;
using UnityEngine;

public class BallPortalBluePrint : IBluePrint
{
    private readonly BallBin _prefab;

    public BallPortalBluePrint(BallBin prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
}

public class BallSpawnerBluePrint : IBluePrint
{
    private readonly BallSpawner _prefab;

    public BallSpawnerBluePrint(BallSpawner prefab)
    {
        _prefab = prefab;
    }

    public IInteractable Build() => Object.Instantiate(_prefab);
}