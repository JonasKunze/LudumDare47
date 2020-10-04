using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IInteractable, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 _lastGrabWorldPos;
    private GameObject _grabbedObject;

    public static bool IsDragging;

    [SerializeField] private GameObject leftGo, rightGo, centerGo;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    
    private Camera _camera;
    private BoxCollider2D _leftCollider, _rightCollider, _centerCollider;

    private void Awake()
    {
        _camera = Camera.main;
      
        _leftCollider = leftGo.GetComponent<BoxCollider2D>();
        _rightCollider = rightGo.GetComponent<BoxCollider2D>();
        _centerCollider = centerGo.GetComponent<BoxCollider2D>();

        Debug.Assert(spriteRenderer != null);
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
    }

    public Vector2 Left
    {
        get => transform.position - transform.right * spriteRenderer.size.x * 0.5f;
        set => SetPosition(value, Right);
    }

    public Vector2 Right
    {
        get => transform.position + transform.right * spriteRenderer.size.x * 0.5f;
        set => SetPosition(Left, value);
    }

    public void SetPosition(Vector2 left, Vector2 right)
    {
        Transform tr = transform;
        
        var delta = right - left;
        tr.position = (left + right) / 2;
       
        tr.rotation = Quaternion.FromToRotation(Vector3.right, delta);
        SetScale(delta.magnitude);
    }

    private void SetScale(float scaleFactor)
    {
        spriteRenderer.size = new Vector2(scaleFactor, spriteRenderer.size.y);

        var sizeX = (scaleFactor * 0.5f);
        
        var leftColliderPosition = -Vector3.right * (sizeX - (0.125f * scaleFactor));
        var rightColliderPosition = Vector3.right * (sizeX - (0.125f * scaleFactor));

        leftGo.transform.localPosition = leftColliderPosition;
        rightGo.transform.localPosition = rightColliderPosition;
        centerGo.transform.position = transform.position;
        
        _leftCollider.size = new Vector2(0.25f * scaleFactor, _leftCollider.size.y);
        _rightCollider.size = new Vector2( 0.25f * scaleFactor, _rightCollider.size.y);
        _centerCollider.size = new Vector2( 0.5f * scaleFactor, _centerCollider.size.y);
    }

    public void SetActive(bool value)
    {
        enabled = value;

        _leftCollider.enabled = value;
        _rightCollider.enabled = value;
        _centerCollider.enabled = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;
        
        if (this)
            Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsDragging)
            return;
        
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        IsDragging = eventData.pointerEnter != null;
        
        if (!IsDragging)
            return;
        
        _grabbedObject = eventData.pointerEnter.gameObject;
        _lastGrabWorldPos = _camera.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsDragging)
            return;
        
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
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

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
    }

    public void OnCreationStart(Transform parent, Vector3 startPosition)
    {
        var tr = GetTransform();

        tr.SetParent(parent);
        tr.position = startPosition;
        SetScale(0);
        SetActive(false);
    }

    public void OnCreationFinish()
    {
        if (spriteRenderer.size.x > .3)
            SetActive(true);
        else
            Destroy(gameObject);
    }

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition)
    {
        var dir = newPosition - startPosition;
        var center = (startPosition + newPosition) * 0.5f;

        var position = center;
        var rotation = Quaternion.FromToRotation(Vector3.right, dir);
        
        transform.SetPositionAndRotation(position, rotation);
        SetScale(dir.magnitude);
    }

    public Transform GetTransform() => transform;
    public Interactable GetInteractable() => this;
}
