using UnityEngine;

public interface IInteractable
{
    void OnCreationStart(Transform parent, Vector3 startPosition);
    void OnCreationFinish();
    void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition);
    Transform GetTransform();
    Interactable GetInteractable();
}