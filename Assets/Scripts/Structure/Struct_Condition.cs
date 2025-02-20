using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class Struct_Condition : MonoBehaviour
{
    public List<GameObject> gameobjs;
    public World world;
    public void PreSetVoxel()
    {
        foreach (GameObject voxel in gameobjs)
        {
            TraverseHierarchy(voxel.gameObject);
            voxel.GetComponentInChildren<BulidVoxel>().Set(world);
        }
    }
    private void TraverseHierarchy(GameObject obj)
    {
        // 检查该对象是否是预制体的实例
        if (PrefabUtility.IsPartOfPrefabInstance(obj))
        {
            // 将预制体实例转换为普通游戏对象
            PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Debug.Log("Prefab instance converted to game object: " + obj.name);
        }

        // 递归遍历子对象
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            TraverseHierarchy(obj.transform.GetChild(i).gameObject);
        }
    }
}
[CustomEditor(typeof(Struct_Condition))]
public class Struct_Condition_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Struct_Condition struct_ = (Struct_Condition)target;
        base.OnInspectorGUI();
        GUILayout.Label("for Conditon voxel");
        if(GUILayout.Button("Update Mesh"))
        {
            struct_.PreSetVoxel();
        }
    }
}


#endif