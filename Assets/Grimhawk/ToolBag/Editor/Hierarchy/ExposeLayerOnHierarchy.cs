
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ExposeLayerOnHierarchy 
{
    static GUIStyle style = new GUIStyle()
    {
        font = Resources.Load<Font>("Fonts/Roboto-Thin"),
        fontSize = 10,
        alignment = TextAnchor.MiddleRight
    };

    static readonly int IgnoreLayer = Layers.Default;

    static ExposeLayerOnHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemChanged;
    }

    

    static void HierarchyWindowItemChanged(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        style.normal.textColor = Color.yellow;

        if (gameObject != null)
        {
            EditorGUI.LabelField(selectionRect, gameObject.layer == IgnoreLayer ? "": LayerMask.LayerToName(gameObject.layer), style);
        }
    }
}
