using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveFunc
{
    public static void SaveJson(string name,object obj)
    {
        var json = JsonUtility.ToJson(obj);
        string path = Application.persistentDataPath + "/" + name + ".rec";
        try
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(path, json);
#if UNITY_EDITOR
            Debug.Log("´æµµ³É¹¦" + path);
#endif
        }
        catch (System.Exception exc)
        {
#if UNITY_EDITOR
            Debug.Log("´æµµÊ§°Ü" + path + exc);
#endif
        }
    }
    public static T LoadJson<T>(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".rec";
        try
        {
            var json = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(json);
#if UNITY_EDITOR
            Debug.Log("¶Áµµ³É¹¦" + path);
#endif
            return data;
        }
        catch (System.Exception exc)
        {
#if UNITY_EDITOR
            Debug.Log("¶ÁµµÊ§°Ü" + path + exc);
#endif      
            return default(T);
        }
    }
    public static bool DeleteJson(string name)
    {
        if(name == "")return false;
        string path = Application.persistentDataPath + "/" + name + ".rec";
        try
        {
            File.Delete(path);
            return true;
        }
        catch (System.Exception exc)
        {
#if UNITY_EDITOR
            Debug.Log("É¾³ýÊ§°Ü" + path + exc);
#endif
            return false;
        }
    }
}
