using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class SerializedTransform
{
    public float[] _position = new float[3];
    public float[] _rotation = new float[4];
    public float[] _scale = new float[3];


    public SerializedTransform(Transform transform)
    {
        _position[0] = transform.localPosition.x;
        _position[1] = transform.localPosition.y;
        _position[2] = transform.localPosition.z;

        _rotation[0] = transform.localRotation.w;
        _rotation[1] = transform.localRotation.x;
        _rotation[2] = transform.localRotation.y;
        _rotation[3] = transform.localRotation.z;

        _scale[0] = transform.localScale.x;
        _scale[1] = transform.localScale.y;
        _scale[2] = transform.localScale.z;
    }
}


public static class TransformExtention
{
    public static void SetTransformFromSerialized(this Transform original, SerializedTransform serialized)
    {
        original.localPosition = new Vector3(serialized._position[0], serialized._position[1],
            serialized._position[2]);
        original.localRotation = new Quaternion(serialized._rotation[1], serialized._rotation[2],
            serialized._rotation[3], serialized._rotation[0]);
        original.localScale = new Vector3(serialized._scale[0], serialized._scale[1],
            serialized._scale[2]);
    }

    public static SerializedTransform Serialized(this Transform original)
    {
        return new SerializedTransform(original);
    }
}