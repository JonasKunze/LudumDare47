using System;
using DefaultNamespace;
using UnityEngine;

[Serializable]
class SerializedObject
{
    [SerializeField] BlueprintIndex type;

    [SerializeField] SerializedTransform trafo;

    public SerializedObject(BlueprintIndex type, Transform trafo)
    {
        this.type = type;
        this.trafo = trafo.Serialized();
    }

    public void Instantiate()
    {
        var obj = Creator.Instance.BuildBlueprint((int) type);
        obj.GetTransform().SetTransformFromSerialized(trafo);
    }
}

public class SerializationHandler
{
    public static void SerializeScene()
    {
        foreach (var platform in GameObject.FindObjectsOfType<SerializableObject>())
        {
            SerializedObject obj = new SerializedObject(platform.GetBlueprintIndex(), platform.transform);

            string json = JsonUtility.ToJson(obj);
            Debug.Log(json);

            var deserialized = JsonUtility.FromJson<SerializedObject>(json);
            deserialized.Instantiate();
        }
    }

    public static void Load(string json)
    {
        json =
            "{\"type\":2,\"trafo\":{\"_position\":[3.0640547275543215,0.5406702756881714,0.0],\"_rotation\":[1.0,0.0,0.0,0.0],\"_scale\":[0.0,1.0,0.0]}}";
        var obj = JsonUtility.FromJson<SerializedObject>(json);
        obj.Instantiate();
    }
}