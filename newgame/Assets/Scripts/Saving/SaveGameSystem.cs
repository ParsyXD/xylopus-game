using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveGameSystem : MonoBehaviour
{
    // Utillity class to make Saving easy

    public static string saveDirectory = "/saves/";
    public static SaveGameSystem Instance;
    [HideInInspector] public List<int> destroyedItems;
    [HideInInspector] public List<int> destroyedTriggers;

    private void Awake()
    {
        Instance = this;
        try { destroyedItems = Load<WorldSpecificSaveObject>(saveDirectory + "save").destroyedItems; }
        catch { destroyedItems = new List<int>(); }

        try { destroyedTriggers = Load<WorldSpecificSaveObject>(saveDirectory + "save").destroyedTriggers; }
        catch { destroyedTriggers = new List<int>(); }
    }

    private static string CorrectedDirectory(string path, bool isFile = false)
    {
        string returning = path;

        if(!path.StartsWith('/'))
        {
            returning = "/" + returning;
        }
        if(isFile)
        {
            if (!path.EndsWith(".json"))
            {
                returning += ".json";
            }
        }
        else
        {
            if (!path.EndsWith('/'))
            {
                returning += "/";
            }
            if (!Directory.Exists(Application.dataPath + returning))
            {
                Directory.CreateDirectory(Application.dataPath + path);
            }
        }
        return Application.dataPath + returning;
    }
    public static void Save(string file, object content)
    {
        string json = JsonUtility.ToJson(content);
        File.WriteAllText(CorrectedDirectory(file, true), json);
    }

    public T Load<T>(string path)
    {
        return JsonUtility.FromJson<T>(File.ReadAllText(CorrectedDirectory(path, true)));
    }

    public class MainSaveObject
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public int inv_prototypeSpheres;
        public int inv_prototypeCubes;
        public int inv_money;
    }

    public class WorldSpecificSaveObject
    {
        public List<int> destroyedItems;
        public List<int> destroyedTriggers;
        public int music_song;
        public int music_layer;
    }
}
