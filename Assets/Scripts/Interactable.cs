using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour,  IPointerClickHandler, IDragHandler, IBeginDragHandler
{
    private Vector2 _lastGrabWorldPos;
    private GameObject _grabbedObject;

    [SerializeField] private GameObject leftGo, rightGo, centerGo;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private Vector2 Left
    {
        get => leftGo.transform.position;
        set => SetPosition(value, Right);
    }

    private Vector2 Right
    {
        get => rightGo.transform.position;
        set => SetPosition(Left, value);
    }

    private void SetPosition(Vector2 left, Vector2 right)
    {
        Transform tr = transform;
        
        var delta = right - left;
        tr.position = (left + right) / 2;
       
        tr.rotation = Quaternion.FromToRotation(Vector3.right, delta);
        tr.localScale = new Vector3(delta.magnitude, tr.localScale.y, 1);
    }

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
        _grabbedObject = eventData.pointerEnter.gameObject;
        _lastGrabWorldPos = _camera.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var newPos = (Vector2) _camera.ScreenToWorldPoint(eventData.position);
        var delta = newPos - _lastGrabWorldPos;
        _lastGrabWorldPos = newPos;
        
        if (_grabbedObject == centerGo)
        {
            transform.position += new Vector3(delta.x, delta.y, 0);
        }
        else if (_grabbedObject == rightGo)
        {
            Right += delta;
        }
        else if (_grabbedObject == leftGo)
        {
            Left += delta;
        }
    }
}
