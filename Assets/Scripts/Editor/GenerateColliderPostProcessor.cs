
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class GenerateColliderPostProcessor : AssetPostprocessor
{
    private void OnPostprocessModel(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            GenerateCollider(child);
        }
    }

    private bool DetectingNamingConvention(Transform t, string convention)
    {
        bool result = false;
        if(t.gameObject.TryGetComponent(out MeshFilter meshFilter))
        {
            var lowercaseMeshName = meshFilter.sharedMesh.name.ToLower();
            result = lowercaseMeshName.StartsWith($"{convention}_");
        }
        if(!result)
        {
            var lowercaseName = t.name.ToLower();
            result = lowercaseName.StartsWith($"{convention}_");
        }
        return result;  
    }
    private void GenerateCollider(Transform t)
    {
        // Add a box collider
    }
    T AddCollider<T>(Transform t) where T : Collider
    {
        T collider = t.gameObject.AddComponent<T>();
        T parentCollider = t.parent.gameObject.AddComponent<T>();

        EditorUtility.CopySerialized(collider, parentCollider);
        SerializedObject parentColliderSO = new SerializedObject(parentCollider);
        var parentColliderCenter = parentColliderSO.FindProperty("m_Center");
        if(parentColliderCenter != null)
        {
            SerializedObject colliderSo = new SerializedObject(collider);
            var colliderCenter = colliderSo.FindProperty("m_Center");
            var worldSpaceColliderCenter = t.TransformPoint(colliderCenter.vector3Value);

            parentColliderCenter.vector3Value = t.parent.InverseTransformPoint(worldSpaceColliderCenter);
            parentColliderSO.ApplyModifiedProperties();
        }
        return parentCollider;
    }
}
