using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SerializedTransform
{
    public float[] _left = new float[2];
    public float[] _right = new float[2];

    public SerializedTransform(IInteractable obj)
    {
        _left[0] = obj.GetInteractable().Left.x;
        _left[1] = obj.GetInteractable().Left.y;
        
        _right[0] = obj.GetInteractable().Right.x;
        _right[1] = obj.GetInteractable().Right.y;
    }

    public Vector2 GetLeft() => new Vector2(_left[0], _left[1]);
    public Vector2 GetRight() => new Vector2(_right[0], _right[1]);
}