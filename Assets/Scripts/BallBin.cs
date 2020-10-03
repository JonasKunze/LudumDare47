using System;
using DefaultNamespace;
using UnityEngine;

public class BallBin : MonoBehaviour, IInteractable
{
    private Interactable _interactable;

    private void Awake()
    {
        _interactable = GetComponentInChildren<Interactable>();
    }

    public void ColliderHit(GameObject obj, Collision2D info)
    {
        Destroy(info.gameObject);
    }

    public void OnCreationStart(Transform parent, Vector3 startPosition)
    {
        var tr = GetTransform();

        tr.SetParent(parent);
        tr.position = startPosition;
        tr.localScale = new Vector3(0, tr.localScale.y, 0);
        GetInteractable().SetActive(false);
    }

    public void OnCreationFinish()
    {
        if (transform.localScale.x > .3)
            _interactable.SetActive(true);
        else
            Destroy(gameObject);
    }

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition)
    {
        var dir = newPosition - startPosition;
        var center = (startPosition + newPosition) * 0.5f;

        var position = center;
        var rotation = Quaternion.FromToRotation(Vector3.right, dir);
        
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = new Vector3(dir.magnitude, transform.localScale.y, 0);
    }

    public Transform GetTransform() => transform;

    public Interactable GetInteractable() => _interactable;
}