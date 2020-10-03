using UnityEngine;
using UnityEngine.Events;

public class CollisionNotifier : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject, Collision2D> onCollisionEnter = null;
    [SerializeField] private UnityEvent<GameObject, Collision2D> onCollisionExit = null;

    private void OnCollisionEnter2D(Collision2D other) => onCollisionEnter?.Invoke(gameObject, other);
    private void OnCollisionExit2D(Collision2D other) => onCollisionExit?.Invoke(gameObject, other);
}
