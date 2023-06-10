using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshDuplicator))]
public class MeshDuplicatorInspector : Editor
{
    private MeshDuplicator mesh;

    private void OnSceneGUI()
    {
        mesh = target as MeshDuplicator;
        

        if(mesh != null)
        {
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                Vector3 vertexWorldPosition = mesh.transform.TransformPoint(mesh.vertices[i]);
                Handles.CubeHandleCap(0, vertexWorldPosition, Quaternion.identity, 0.05f, EventType.Repaint);
                Handles.color = Color.blue;
            }
        }
    }
}
