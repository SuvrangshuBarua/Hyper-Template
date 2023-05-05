using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ExposeTagOnHierarchy 
{
    static readonly string IgnoreTag = Tags.Untagged; 
	static ExposeTagOnHierarchy()
	{
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemchanged;
	}
    static GUIStyle style = new GUIStyle()
    {
        font = Resources.Load<Font>("Fonts/Roboto-Thin"),
        fontSize = 10,
        alignment = TextAnchor.MiddleCenter,
    };



    private static void HierarchyWindowItemchanged(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;  

        style.normal.textColor = Color.white;   
        if(gameObject != null) 
        {
            EditorGUI.LabelField(selectionRect, gameObject.tag == IgnoreTag ?"" : gameObject.tag, style);
        }
    }
}
