using UnityEngine;
using UnityEngine.EventSystems;

public class Platform : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler
{
    private Vector2 lastGrabWorldPos;
    private GameObject grabbedObject;
    private bool grabbingLeft;

    [SerializeField] private GameObject leftGo, rightGo, centerGo;

    private Vector2 left => leftGo.transform.position;
    private Vector2 right => rightGo.transform.position;
    private Vector2 center => transform.position;

    public void ColliderHit(Collider2D other)
    {
        Debug.LogError($"{other.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this && eventData.button == PointerEventData.InputButton.Right)
            Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter.gameObject.name);
        grabbedObject = eventData.pointerEnter.gameObject;
        lastGrabWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (grabbedObject == centerGo)
        {
            var newPos = (Vector2) Camera.main.ScreenToWorldPoint(eventData.position);
            var delta = newPos - lastGrabWorldPos;
            lastGrabWorldPos = newPos;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }
    }
}