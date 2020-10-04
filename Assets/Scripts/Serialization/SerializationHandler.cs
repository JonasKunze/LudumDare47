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
    private static readonly string lastSongFileName = "Level.json";
    public static readonly string Dir = Application.persistentDataPath;

    public static void SerializeScene()
    {
        List<SerializedObject> serializedObjects = new List<SerializedObject>();
        foreach (var platform in Object.FindObjectsOfType<SerializableObject>())
        {
            serializedObjects.Add(new SerializedObject(platform.GetBlueprintIndex(), platform as IInteractable));
        }

        var container = new SerializationContainer(serializedObjects);
        string json = JsonUtility.ToJson(container);

        var fileName = $"Level {DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.json";
        var fullPath = GetFullPath(fileName);
        var file = File.CreateText(fullPath);
        file.Write(json);
        file.Close();
        
        Creator.Instance.SaveLevel(in fullPath);
    }

    public static string GetFullPath(in string fileName) => Path.Combine(Dir, fileName);
    
    public static string[] GetUserLevels()
    {
        var files = Directory.GetFiles(Dir, "*.json");
        return files;
    }
}