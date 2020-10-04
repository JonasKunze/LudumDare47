using System;
using System.Collections.Generic;
using System.IO;
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

[Serializable]
class SerializationContainer
{
    [SerializeField] private List<SerializedObject> objects;

    public SerializationContainer(List<SerializedObject> objects)
    {
        this.objects = objects;
    }

    public void Instantiate()
    {
        foreach (var serializedObject in objects)
        {
            serializedObject.Instantiate();
        }
    }
}

public class SerializationHandler
{
    public static void SerializeScene()
    {
        List<SerializedObject> serializedObjects = new List<SerializedObject>();
        foreach (var platform in GameObject.FindObjectsOfType<SerializableObject>())
        {
            serializedObjects.Add(new SerializedObject(platform.GetBlueprintIndex(), platform.transform));
        }

        var container = new SerializationContainer(serializedObjects);
        string json = JsonUtility.ToJson(container);

        var file = File.CreateText(Application.persistentDataPath + "lastSong");
        file.Write(json);
        file.Close();
    }

    public static void LoadLastSong()
    {
        var file = Application.persistentDataPath + "lastSong";
        if (File.Exists(file))
        {
            var sr = File.OpenText(file);
            var json = sr.ReadToEnd();
            var obj = JsonUtility.FromJson<SerializationContainer>(json);
            obj.Instantiate();
        }
        else
        {
            Debug.Log("Could not Open the file: " + file + " for reading.");
        }
    }
}