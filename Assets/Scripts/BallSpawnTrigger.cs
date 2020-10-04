﻿using System;
using DefaultNamespace;
using UnityEngine;

public class BallSpawnTrigger : SerializableObject, IInteractable
{
    private Interactable _interactable;

    private void Awake()
    {
        _interactable = GetComponentInChildren<Interactable>();
    }

    public void ColliderHit(GameObject obj, Collision2D info)
    {
        var ball = info.gameObject.GetComponent<Ball>();
        if (ball)
        {
            if (!ball.spawner)
                Destroy(info.gameObject);
            else
            {
                ball.spawner.Spawn();
            }
        }
    }

    public void OnCreationStart(Transform parent, Vector3 startPosition) => GetInteractable().OnCreationStart(parent, startPosition);

    public void OnCreationFinish() => GetInteractable().OnCreationFinish();

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition) => GetInteractable().OnCreationUpdate(newPosition, startPosition);

    public Transform GetTransform() => transform;
    public Interactable GetInteractable()
    {
        if (_interactable == null)
            _interactable = GetComponentInChildren<Interactable>();
        return _interactable;
    }

    public override BlueprintIndex GetBlueprintIndex()    {
        return BlueprintIndex.SpawnTrigger;
    }
}