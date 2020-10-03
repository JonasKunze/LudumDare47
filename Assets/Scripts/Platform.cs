using UnityEngine;


public class Platform : MonoBehaviour
{
    [SerializeField] private Interactable interactable = null;
    
    public void ColliderHit(GameObject obj, Collision2D info)
    {
        Debug.LogError($"{obj.name}, {info.gameObject}");
    }

    public void SetActive(bool b) => interactable.SetActive(b);
}