using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Camera _cam;
    void Awake()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var orthographicSize = _cam.orthographicSize;
        transform.position = new Vector3(_cam.transform.position.x, _cam.transform.hierarchyCapacity, transform.position.z);
        transform.localScale = new Vector3(10 *orthographicSize, 10*orthographicSize, 1);
    }
}
