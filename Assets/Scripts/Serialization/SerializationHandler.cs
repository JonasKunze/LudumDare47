using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
class SerializedObject
{
    [SerializeField] BlueprintIndex type;

    [SerializeField] SerializedTransform trafo;

    public SerializedObject(BlueprintIndex type, IInteractable obj)
    {
        this.type = type;
        trafo = new SerializedTransform(obj);
    }

    public void Instantiate()
    {
        var obj = Creator.Instance.BuildBlueprint((int) type);
        obj.GetInteractable().SetPosition(trafo.GetLeft(), trafo.GetRight());
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
    private static readonly string lastSongFileName = "lastSong.json";

    public static void SerializeScene()
    {
        List<SerializedObject> serializedObjects = new List<SerializedObject>();
        foreach (var platform in Object.FindObjectsOfType<SerializableObject>())
        {
            serializedObjects.Add(new SerializedObject(platform.GetBlueprintIndex(), platform as IInteractable));
        }

        var container = new SerializationContainer(serializedObjects);
        string json = JsonUtility.ToJson(container);

        var file = File.CreateText(Application.persistentDataPath + "/" + lastSongFileName);
        file.Write(json);
        file.Close();
    }

    public static void LoadLastSong()
    {
        var file = Application.persistentDataPath + "/" + lastSongFileName;
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